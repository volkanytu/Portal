using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.WindowsServices.SendNpsSurvey
{
    public class Constants
    {
        public const string QUERY_GET_NPS_SURVEY_RECORDS_BY_STATUSCODE = @"SELECT
	                                                                    s.new_npssurveyId AS Id
	                                                                    ,er.new_emailaddress AS EmailAddress
                                                                        ,s.new_asseblyrequestid AS RequestId
                                                                    FROM
                                                                    new_npssurvey AS s (NOLOCK)
                                                                        JOIN
                                                                            new_assemblyrequest AS er (NOLOCK)
                                                                                ON
                                                                                er.new_AssemblyrequestId=s.new_asseblyrequestid
                                                                    WHERE
                                                                    s.StateCode=0
                                                                    AND
                                                                    s.StatusCode=@statusCode
                                                                    AND
                                                                    er.new_emailaddress IS NOT NULL
                                                                    --AND
                                                                    --DATEDIFF(DD,s.CreatedOn,GETUTCDATE())>6";

        public const string SURVEY_MAILD_BODY = @"Sayın {0}, </br>
                                                    almış olduğunuz hizmeti aşağıdaki linkten değerlendirebilirsiniz. </br>
                                                    Değerlendirme Anketi için <a href='kaleanahtarcilarkulubu.com.tr/nps.html?objectid={1}'>Tıklayınız.</a>";
    }
}
