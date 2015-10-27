using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using GK.Library.Utility;

namespace GK.Library.Business
{
    /// <summary>
    /// Geçerlilik kontrollerinin gerçekleştirildiği sınıftır
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Telefon numarası parse edilir
        /// </summary>
        /// <param name="phoneNo">Telefon numarası</param>
        /// <returns>TelephoneNumber Class döner</returns>
        public static TelephoneNumber ParseTelephoneNumber(string phoneNo)
        {
            TelephoneNumber tn = new TelephoneNumber();

            string countryCode = string.Empty;
            string phoneCode = string.Empty;
            string phoneNumber = string.Empty;

            if (phoneNo.Length == 15)
            {
                countryCode = phoneNo.Substring(1, 2);
                phoneCode = phoneNo.Substring(4, 3);
                phoneNumber = phoneNo.Substring(8, 7);

                tn.countryCode = countryCode;
                tn.phoneCode = phoneCode;
                tn.phoneNo = phoneNumber;
                tn.TelephoneNo = phoneNo;
            }

            return tn;

        }

        /// <summary>
        /// Telefon numarasının formatını kontrol eder
        /// </summary>
        /// <param name="phoneNo">Telefon numarası</param>
        /// <returns>TelephoneNumber Class döner</returns>
        public static TelephoneNumber CheckTelephoneNumber(string phoneNo)
        {
            string countryCode = string.Empty;
            string phoneCode = string.Empty;
            string phoneNumber = string.Empty;

            if (phoneNo.Length == 15)
            {
                countryCode = phoneNo.Substring(1, 2);
                phoneCode = phoneNo.Substring(4, 3);
                phoneNumber = phoneNo.Substring(8, 7);

                TelephoneNumber tn = new TelephoneNumber();
                tn.TelephoneNo = phoneNo;
                tn.countryCode = countryCode;
                tn.phoneCode = phoneCode;
                tn.phoneNo = phoneNumber;

                //if (IsPhoneCodeCorrect(phoneCode))
                //{
                if (IsPhoneNumberCorrect(phoneNumber))
                    tn.isFormatOK = true;
                else
                    tn.isFormatOK = false;
                //}
                //else
                //    tn.isFormatOK = false;

                return tn;
            }
            else
            {
                return new TelephoneNumber()
                {
                    countryCode = countryCode,
                    phoneCode = phoneCode,
                    phoneNo = phoneNumber,
                    isFormatOK = false
                };
            }
        }

        /// <summary>
        /// Telefon kodunu kontrol eder
        /// </summary>
        /// <param name="phoneCode">Telefon kodu</param>
        /// <returns>bool döner</returns>
        public static bool IsPhoneCodeCorrect(string phoneCode)
        {
            bool returnValue = false;
            SqlDataAccess sda = new SqlDataAccess();
            sda.openConnection(Globals.ConnectionString);

            #region |SQL QUERY|
            string sqlQuery = @"SELECT 		
                                 COUNT(0) AS RecCount
                                FROM
                                  MetadataSchema.LocalizedLabel OptionSetValue (NOLOCK)
	                                 JOIN 
                                        MetadataSchema.AttributePicklistValue AS AttributeOptionSetMapping (NOLOCK)
	                                        ON 
                                            OptionSetValue.ObjectId = AttributeOptionSetMapping.AttributePicklistValueId
	                                 JOIN 
                                        MetadataSchema.OptionSet AS OptionSets (NOLOCK)
	                                        ON 
                                            AttributeOptionSetMapping.OptionSetId = OptionSets.OptionSetId
                                WHERE 
                                    OptionSets.Name = 'new_telefonkodlari'
                                AND 
                                    OptionSetValue.ObjectColumnName = 'DisplayName'
                                AND 
                                    AttributeOptionSetMapping.Value={0}";
            #endregion

            int count = (int)sda.ExecuteScalar(string.Format(sqlQuery, phoneCode));

            if (count > 0)
                returnValue = true;

            sda.closeConnection();
            return returnValue;
        }

        /// <summary>
        /// Telefon numarasında tekrarlayan rakamları kontrol eder
        /// </summary>
        /// <param name="phoneNo">Telefon numarası</param>
        /// <returns>bool döner</returns>
        public static bool IsPhoneNumberCorrect(string phoneNo)
        {
            bool returnValue = true;

            string[] wrongForms = new string[] { "1111111", "2222222", "3333333", "4444444", "5555555", "6666666", "7777777", "8888888", "9999999", "0000000", "1234567", "2345678", "3456789", "9876543", "0123456", "7654321", "8765432" };

            if (phoneNo.Length != 7)
            {
                returnValue = false;
            }
            else
            {
                for (int i = 0; i < wrongForms.Length; i++)
                {
                    if (wrongForms[i] == phoneNo)
                        returnValue = false;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneCode">Telefon kodu</param>
        /// <param name="cityCode">Şehir kodu</param>
        /// <returns>bool döner</returns>
        public static bool IsPhoneCodeBelongsToCityCode(string phoneCode, string cityCode)
        {
            bool returnValue = false;
            SqlDataAccess sda = new SqlDataAccess();
            sda.openConnection(Globals.ConnectionString);

            #region |SQL QUERY|
            string sqlQuery = @"SELECT 		
                                --OptionSetValue.Label,AttributeOptionSetMapping.Value
                                    COUNT(0) AS RecCount
                                FROM
                                  MetadataSchema.LocalizedLabel OptionSetValue (NOLOCK)
                                    JOIN 
                                        MetadataSchema.AttributePicklistValue AS AttributeOptionSetMapping (NOLOCK)
                                            ON 
                                            OptionSetValue.ObjectId = AttributeOptionSetMapping.AttributePicklistValueId
                                    JOIN 
                                        MetadataSchema.OptionSet AS OptionSets (NOLOCK)
                                            ON 
                                                AttributeOptionSetMapping.OptionSetId = OptionSets.OptionSetId
                                 WHERE 
                                        OptionSets.Name = 'new_telefonkodlari'
                                 AND 
                                        OptionSetValue.ObjectColumnName = 'DisplayName'
                                 AND 
                                        OptionSetValue.Label='{0}'
                                 AND 
                                        AttributeOptionSetMapping.Value={1}";
            #endregion

            int count = (int)sda.ExecuteScalar(string.Format(sqlQuery, cityCode, phoneCode));

            if (count > 0)
                returnValue = true;

            sda.closeConnection();
            return returnValue;
        }

        /// <summary>
        /// Email numarasının formatını kontrol eder
        /// </summary>
        /// <param name="EmailAddress">Email adresi</param>
        /// <returns>bool döner</returns>
        public static bool CheckEmail(string EmailAddress)
        {
            //string strPattern = "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            //Volkan 07.11.2014 --Sahibinden.com RegEx
            string strPattern = "^([a-zA-Z0-9_-]+)(([a-zA-Z0-9._-]+))*[@]([a-zA-Z0-9]+)([._-]([a-zA-Z0-9]+))*[.]([a-zA-Z0-9]){2,3}([a-zA-Z0-9])?$";

            if (System.Text.RegularExpressions.Regex.IsMatch(EmailAddress, strPattern))
            { return true; }

            return false;
        }
    }
}
