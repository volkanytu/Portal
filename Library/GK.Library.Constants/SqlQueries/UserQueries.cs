using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Constants.SqlQueries
{
    public class UserQueries : BaseQueries
    {
        private static string BASE_SELECT = string.Format(@" new_userId AS Id
	                                                        ,new_name AS Name
                                                            ,new_imageurl AS ImageUrl
                                                            ,new_password AS Password
                                                            ,new_contactId AS ContactId
                                                            ,new_contactIdName AS ContactIdName
                                                            ,'contact' AS ContactIdTypeName
	                                                        ,{0} ", ENTITY_BASE_SELECT);

        public static string GET_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_user (NOLOCK)
                                                            WHERE
	                                                            new_userId=@Id", BASE_SELECT);

        public static string GET_LIST_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_user (NOLOCK)
                                                            WHERE
	                                                            statecode=0", BASE_SELECT);

        public static string GET_USER_BY_NAME = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_user (NOLOCK)
                                                            WHERE
	                                                            new_name=@name
                                                                AND
                                                                statecode=0", BASE_SELECT);

    }
}
