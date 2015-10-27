using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.WindowsServices.SendSms
{
    public class Constants
    {
        public const string QUERY_GET_SMS_RECORDS_BY_STATUSCODE = @"SELECT
	                                                                    s.new_smsId AS Id
	                                                                    ,s.new_phonenumber AS PhoneNumber
	                                                                    ,s.new_message AS Message
                                                                    FROM
                                                                    new_sms AS s (NOLOCK)
                                                                    WHERE
                                                                    s.StateCode=0
                                                                    AND
                                                                    s.StatusCode=@statusCode
                                                                    AND
                                                                    s.new_phonenumber IS NOT NULL
                                                                    AND
                                                                    s.new_message IS NOT NULL" ;
    }
}
