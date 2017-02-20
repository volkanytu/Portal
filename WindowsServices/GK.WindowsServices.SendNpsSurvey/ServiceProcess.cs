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
using System.Net.Mail;
using System.Net;

namespace GK.WindowsServices.SendNpsSurvey
{
    public class ServiceProcess
    {
        private SqlDataAccess _sda;
        private IOrganizationService _service;

        private string LOG_PATH;
        private string ERROR_LOG_PATH;

        private string FROM_MAILADDRESS;
        private string SMTP_USERNAME;
        private string SMTP_PASSWORD;
        private string MAIL_HOST;
        private string PORT;

        public ServiceProcess(SqlDataAccess sda, IOrganizationService service)
        {
            _sda = sda;
            _service = service;

            LOG_PATH = Globals.FileLogPath;
            ERROR_LOG_PATH = Globals.FileLogPath;

            SMTP_USERNAME = ConfigurationManager.AppSettings["username"].ToString();
            SMTP_PASSWORD = ConfigurationManager.AppSettings["password"].ToString();
            FROM_MAILADDRESS = ConfigurationManager.AppSettings["mailaddress"].ToString();
            MAIL_HOST = ConfigurationManager.AppSettings["hostaddress"].ToString();
            PORT = ConfigurationManager.AppSettings["port"].ToString();
        }

        public void Process(SqlDataAccess sda)
        {
            FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_ServiceProcess_Process_TRIGGERED", LOG_PATH);

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@statusCode",(int)NpsSurveyStatus.Created)
            };

            try
            {
                DataTable dt = _sda.getDataTable(Constants.QUERY_GET_NPS_SURVEY_RECORDS_BY_STATUSCODE, parameters);

                FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_ServiceProcess_Process_SurveyRecordCount:" + dt.Rows.Count.ToString(), LOG_PATH);

                foreach (DataRow row in dt.Rows)
                {
                    SendSurveyMail((Guid)row["Id"], row["EmailAddress"].ToString(), (Guid)row["RequestId"]);
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_ServiceProcess_Process_EXCEPTION:" + ex.Message, ERROR_LOG_PATH);
            }
        }

        private void SendSurveyMail(Guid npsSurveyId, string emailAddress, Guid requestId)
        {
            MsCrmResultObj<AssemblyRequestInfo> resultRequestInfo = AssemblyRequestHelper.GetAssemblyRequestInfo(requestId, _sda);

            AssemblyRequestInfo requestInfo;

            if (resultRequestInfo.Success)
            {
                requestInfo = resultRequestInfo.ReturnObject;
            }
            else
            {
                FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_ServiceProcess_SenSurveydMail_REQUEST_INFO_NULL:" + resultRequestInfo.Result, ERROR_LOG_PATH);
                UpdateNpsSurveyEntityStatusCode(npsSurveyId, NpsSurveyStatus.NotSent);

                return;
            }


            if (!string.IsNullOrWhiteSpace(requestInfo.EmailAddress) && !string.IsNullOrWhiteSpace(requestInfo.FirstName) && !string.IsNullOrWhiteSpace(requestInfo.LastName))
            {
                string fullName = requestInfo.FirstName + " " + requestInfo.LastName;
                string mailBody = string.Format(Constants.SURVEY_MAILD_BODY, fullName, npsSurveyId.ToString());

                FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_ServiceProcess_SendSurveydMail_INFO:" + requestInfo.EmailAddress, ERROR_LOG_PATH);
                MsCrmResult resultSendMail = SendMail(requestInfo.EmailAddress, "Kale Anahtar Değerlendirme Anketi", mailBody);

                if (resultSendMail.Success)
                {
                    UpdateNpsSurveyEntityStatusCode(npsSurveyId, NpsSurveyStatus.Sent);
                }
                else
                {
                    FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_ServiceProcess_SendMail_EXCEPTION:" + resultSendMail.Result, ERROR_LOG_PATH);
                    UpdateNpsSurveyEntityStatusCode(npsSurveyId, NpsSurveyStatus.NotSent);
                }
            }

        }

        private void UpdateNpsSurveyEntityStatusCode(Guid surveyId, NpsSurveyStatus statusCode)
        {

            try
            {
                Entity ent = new Entity("new_npssurvey");
                ent.Id = surveyId;
                ent["statuscode"] = new OptionSetValue((int)statusCode);

                _service.Update(ent);
            }
            catch (Exception ex)
            {
                FileLogHelper.LogFunction(this.GetType().Name, "SendNpsSurvey_ServiceProcess_UpdateNpsSurveyEntityStatusCode_EXCEPTION:" + ex.Message, ERROR_LOG_PATH);
            }
        }

        private MsCrmResult SendMail(string emailAddress, string subject, string mailBody)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                MailAddress to = new MailAddress(emailAddress);
                MailAddress from = new MailAddress(FROM_MAILADDRESS);

                MailMessage mail = new MailMessage(from, to);

                mail.Subject = subject;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = MAIL_HOST;
                smtp.Port = Convert.ToInt32(PORT);

                smtp.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                smtp.EnableSsl = false;

                smtp.Send(mail);

                returnValue.Success = true;

            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }
    }
}
