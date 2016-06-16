using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Constants.SqlQueries
{
    public class SessionQueries : BaseQueries
    {
        private static string BASE_SELECT = string.Format(@" new_sessionId AS Id
	                                                        ,new_name AS Name
                                                            ,new_ipaddress AS IpAddress
                                                            ,new_isauthenticated AS IsAuthenticated
                                                            ,new_expiredate AS ExpireDate
                                                            ,new_userid AS UserId
                                                            ,new_useridName AS UserIdName
                                                            ,'new_user' AS UserIdTypeName
	                                                        ,{0} ", ENTITY_BASE_SELECT);

        public static string GET_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_session (NOLOCK)
                                                            WHERE
	                                                            new_sessionId=@Id", BASE_SELECT);

        public static string GET_LIST_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_session (NOLOCK)
                                                            WHERE
	                                                            statecode=0", BASE_SELECT);
    }
}
