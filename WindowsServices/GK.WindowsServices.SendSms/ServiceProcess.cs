using GK.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using GK.Library.Business;
using System.Data.SqlClient;

namespace GK.WindowsServices.SendSms
{
    public class ServiceProcess
    {
        private SqlDataAccess _sda;
        private IOrganizationService _service;
        private smsService.smsservice _smsApi;

        private string LOG_PATH;
        private string ERROR_LOG_PATH;
        private SmsConfiguration SMS_CONFIG;

        public ServiceProcess(SqlDataAccess sda, IOrganizationService service)
        {
            _sda = sda;
            _service = service;

            LOG_PATH = Globals.FileLogPath;
            ERROR_LOG_PATH = Globals.FileLogPath;

            MsCrmResultObject resultSmsConf = SMSHelper.GetSmsConfigurationInfo(new Guid(Globals.SmsConfigurationDoluHayatId), sda);

            if (!resultSmsConf.Success)
            {
                FileLogHelper.LogFunction(this.GetType().Name,"SendSms_ServiceProcess_NOCONFIG_DEFINATION", LOG_PATH);
            }

            SMS_CONFIG = (SmsConfiguration)resultSmsConf.ReturnObject;

            _smsApi = new smsService.smsservice();
        }

        public void Process(SqlDataAccess sda)
        {
            FileLogHelper.LogFunction(this.GetType().Name,"SendSms_ServiceProcess_Process_TRIGGERED", LOG_PATH);

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@statusCode",(int)SmsStatusCode.WaitingSend)
            };

            try
            {
                DataTable dt = _sda.getDataTable(Constants.QUERY_GET_SMS_RECORDS_BY_STATUSCODE, parameters);

                FileLogHelper.LogFunction(this.GetType().Name,"SendSms_ServiceProcess_Process_SmsRecordCount:" + dt.Rows.Count.ToString(), LOG_PATH);

                foreach (DataRow row in dt.Rows)
                {
                    SendSms((Guid)row["Id"], row["PhoneNumber"].ToString(), row["Message"].ToString());
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.LogFunction(this.GetType().Name,"SendSms_ServiceProcess_Process_EXCEPTION:" + ex.Message, ERROR_LOG_PATH);
            }
        }

        private void SendSms(Guid smsId, string phoneNumber, string message)
        {
            TelephoneNumber telNumber = ValidationHelper.CheckTelephoneNumber(phoneNumber);

            if (!telNumber.isFormatOK)
            {
                UpdateSmsEntity(smsId, SmsStatusCode.NotSent, "WRONGFORMAT", "Hatalı Telefon formatı");

                return;
            }

            string[] result = _smsApi.SmsInsert_1_N(SMS_CONFIG.UserName, SMS_CONFIG.Password, null, null, new string[] { phoneNumber }, message);

            if (result == null || result.Length == 0)
            {
                FileLogHelper.LogFunction(this.GetType().Name,"SendSms_ServiceProcess_SendSms_NO_RESULT", ERROR_LOG_PATH);

                UpdateSmsEntity(smsId, SmsStatusCode.NotSent, "NORESULT", "Sonuç bilgisi yok.");

                return;
            }

            string errorText = string.Empty;
            bool hasError = SmsProviderCodes.SmsErrorCodes.TryGetValue(result[0], out errorText);

            if (string.IsNullOrWhiteSpace(errorText))
            {
                UpdateSmsEntity(smsId, SmsStatusCode.Sent, result[0], "Mesaj Gönderildi.");
            }
            else
            {
                string statusText = string.Empty;
                bool hasStatusText = SmsProviderCodes.SmsStatusCodes.TryGetValue(result[0], out statusText);

                if (string.IsNullOrWhiteSpace(statusText))
                {
                    statusText = "UNKNOWN STATUS";
                }

                UpdateSmsEntity(smsId, SmsStatusCode.Sent, result[0], statusText);
            }
        }

        private void UpdateSmsEntity(Guid smsId, SmsStatusCode statusCode, string messageState, string messageStatus)
        {

            try
            {
                Entity ent = new Entity("new_sms");
                ent.Id = smsId;
                ent["statuscode"] = new OptionSetValue((int)statusCode);
                ent["new_messagestate"] = messageState;
                ent["new_messagestatustext"] = messageStatus;

                _service.Update(ent);
            }
            catch (Exception ex)
            {
                FileLogHelper.LogFunction(this.GetType().Name,"SendSms_ServiceProcess_UpdateSmsEntity_EXCEPTION:" + ex.Message, ERROR_LOG_PATH);
            }
        }
    }
}
