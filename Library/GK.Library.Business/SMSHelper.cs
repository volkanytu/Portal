using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class SMSHelper
    {
        public static string CreateSMSPassword(int size)
        {
            char[] cr = "0123456789".ToCharArray();
            string result = string.Empty;
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                result += cr[r.Next(0, cr.Length - 1)].ToString();
            }
            return result;
        }

        public static MsCrmResult CreateSmsRecord(IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                returnValue.Success = true;
                returnValue.CrmId = Guid.NewGuid();
                returnValue.Result = "Sms kaydı oluşturuldu.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;

        }

        public static MsCrmResultObject GetSmsConfigurationInfo(Guid configurationId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    sc.new_name AS Name
	                                    ,sc.new_accountnumber AS AccountNumber
	                                    ,sc.new_gsmoperator AS GsmOperator
	                                    ,sc.new_orginator AS Orginator
	                                    ,sc.new_password AS Password
	                                    ,sc.new_shortnumber AS ShortNumber
	                                    ,sc.new_smsconfigurationId AS Id
	                                    ,sc.new_username AS UserName
                                        ,sc.StatusCode
                                    FROM
	                                    new_smsconfiguration AS sc (NOLOCK)
                                    WHERE
	                                    sc.new_smsconfigurationId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, configurationId));

                if (dt.Rows.Count > 0)
                {
                    SmsConfiguration smsConf = new SmsConfiguration();

                    smsConf.Id = (Guid)dt.Rows[0]["Id"];
                    smsConf.Name = dt.Rows[0]["Name"].ToString();

                    if (dt.Rows[0]["GsmOperator"] != DBNull.Value)
                    {
                        smsConf.Operator = (GsmOperators)(int)dt.Rows[0]["GsmOperator"];
                    }

                    smsConf.AccountNumber = dt.Rows[0]["AccountNumber"].ToString();
                    smsConf.UserName = dt.Rows[0]["UserName"].ToString();
                    smsConf.Password = dt.Rows[0]["Password"].ToString();
                    smsConf.ShortNumber = dt.Rows[0]["ShortNumber"].ToString();
                    smsConf.Orginator = dt.Rows[0]["Orginator"].ToString();
                    smsConf.StatusCode = (int)dt.Rows[0]["StatusCode"];

                    returnValue.ReturnObject = smsConf;
                    returnValue.Success = true;
                }
                else
                {
                    returnValue.Result = "Konfigürasyon bilgisi alınamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetSendSsmsRecordInfo(Guid sendSmsId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"DECLARE @now DATETIME
                                SET @now=GETUTCDATE()

                                SELECT
	                                s.new_sentsmsId AS Id
	                                ,s.new_name AS Name
	                                ,s.statuscode AS StatusCode
	                                ,s.new_contactId AS ContactId
	                                ,s.new_contactIdName AS ContactIdName
	                                ,s.new_content AS Content
	                                ,s.new_errortext AS ErrorText
	                                ,s.new_issent AS IsSent
	                                ,s.new_MesajId AS MessageId
	                                ,s.new_phonenumber AS PhoneNumber
	                                ,s.new_prefferedsenddate AS PrefferedSendDate
	                                ,s.new_resultcode AS ResultCode
	                                ,s.new_smsconfigurationId AS SmsConfigurationId
	                                ,s.new_smsconfigurationIdName AS SmsConfigurationIdName
                                FROM
	                                new_sentsms AS s (NOLOCK)
                                WHERE
	                                s.new_sentsmsId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, sendSmsId));

                if (dt.Rows.Count > 0)
                {
                    SendSmsRecord sendRec = new SendSmsRecord();

                    sendRec.Id = (Guid)dt.Rows[0]["Id"];
                    sendRec.Name = dt.Rows[0]["Name"].ToString();
                    sendRec.StatusCode = (int)dt.Rows[0]["StatusCode"];


                    if (dt.Rows[0]["ContactId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["ContactId"],
                            Name = dt.Rows[0]["ContactIdName"].ToString(),
                            LogicalName = "contact"
                        };

                        sendRec.Contact = er;
                    }

                    sendRec.Content = dt.Rows[0]["Content"].ToString();
                    sendRec.IsSent = (bool)dt.Rows[0]["IsSent"];
                    sendRec.PhoneNumber = dt.Rows[0]["PhoneNumber"].ToString();
                    sendRec.PrefferedSendDate = (DateTime)dt.Rows[0]["PrefferedSendDate"];
                    sendRec.ResultCode = dt.Rows[0]["ResultCode"].ToString();

                    if (dt.Rows[0]["SmsConfigurationId"] != DBNull.Value)
                    {
                        //EntityReference er = new EntityReference()
                        //{
                        //    Id = (Guid)dt.Rows[0]["SmsConfigurationId"],
                        //    Name = dt.Rows[0]["SmsConfigurationId"].ToString(),
                        //    LogicalName = "new_smsconfiguration"
                        //};

                        //sendRec.SmsConfiguration = er;

                        MsCrmResultObject resultConf = SMSHelper.GetSmsConfigurationInfo((Guid)dt.Rows[0]["SmsConfigurationId"], sda);

                        if (resultConf.Success)
                        {
                            sendRec.SmsConfiguration = (SmsConfiguration)resultConf.ReturnObject;
                        }
                    }

                    returnValue.ReturnObject = sendRec;
                    returnValue.Success = true;
                    returnValue.Result = "Sms gönderme kaydı bilgisi çekildi.";
                }
                else
                {
                    returnValue.Result = "Sms Gönderim kaydı bilgisi çekilemedi.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetSendSmsList(Guid portalId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            List<SendSmsRecord> lstSendList = new List<SendSmsRecord>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"DECLARE @now DATETIME
                                SET @now=GETUTCDATE()

                                SELECT
	                                s.new_sentsmsId AS Id
                                FROM
	                                new_sentsms AS s (NOLOCK)
                                WHERE
	                                s.new_prefferedsenddate<=@now
                                AND
	                                s.statuscode=1 --İşlem Bekliyor
                                AND
                                    s.new_portalId='{0}'
                                AND
	                                s.statecode=0";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId));

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MsCrmResultObject result = SMSHelper.GetSendSsmsRecordInfo((Guid)dt.Rows[i]["Id"], sda);

                        if (result.Success)
                        {
                            lstSendList.Add((SendSmsRecord)result.ReturnObject);
                        }
                    }

                    returnValue.ReturnObject = lstSendList;
                    returnValue.Success = true;
                    returnValue.Result = "Gönderim Listesi çekilidi.";
                }
                else
                {
                    returnValue.Result = "Gönderim listesi boş.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult UpdateSendSmsRecord(SendSmsRecord rec, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_sentsms");
                ent["new_sentsmsid"] = rec.Id;
                ent["new_issent"] = rec.IsSent;
                ent["new_mesajid"] = rec.MessageId;
                ent["new_resultcode"] = rec.ResultCode;
                ent["statuscode"] = new OptionSetValue(rec.StatusCode);//işlendi
                ent["new_errortext"] = rec.Error;

                service.Update(ent);

                returnValue.Success = true;
                returnValue.Result = "Güncellendi...";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult CreateOrUpdateSendSmsRecord(SendSmsRecord rec, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_sentsms");
                ent["new_name"] = rec.Name;

                if (rec.Id != null && rec.Id != Guid.Empty)
                {
                    ent["new_sentsmsid"] = rec.Id;
                }

                ent["new_issent"] = rec.IsSent;

                if (!string.IsNullOrEmpty(rec.MessageId))
                {
                    ent["new_mesajid"] = rec.MessageId;
                }

                if (!string.IsNullOrEmpty(rec.ResultCode))
                {
                    ent["new_resultcode"] = rec.ResultCode;
                }

                if (rec.StatusCode != null && rec.StatusCode != 0)
                {
                    ent["statuscode"] = new OptionSetValue(rec.StatusCode);
                }

                ent["new_errortext"] = rec.Error;

                if (rec.Contact != null)
                {
                    ent["new_contactid"] = rec.Contact;
                }
                ent["new_phonenumber"] = rec.PhoneNumber;

                if (rec.SmsConfiguration != null)
                {
                    ent["new_smsconfigurationid"] = new EntityReference("new_smsconfiguration", rec.SmsConfiguration.Id);
                }

                ent["new_prefferedsenddate"] = rec.PrefferedSendDate;
                ent["new_portalid"] = rec.Portal;
                ent["new_content"] = rec.Content;

                if (rec.Id == null || rec.Id == Guid.Empty)
                {
                    returnValue.CrmId = service.Create(ent);
                }
                else
                {
                    returnValue.CrmId = rec.Id;
                    service.Update(ent);
                }

                returnValue.Success = true;
                returnValue.Result = "SMS kaydı oluşturuldu...";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }
    }
}
