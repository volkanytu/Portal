using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Constants.SqlQueries
{
    public class EducationQueries : BaseQueries
    {
        private static string BASE_SELECT = string.Format(@" new_educationId AS Id
	                                                        ,new_name AS Name
                                                            ,new_summary AS Summary
                                                            ,new_content AS Content
                                                            ,new_imageurl AS ImageUrl
                                                            ,new_categoryid AS CategoryId
                                                            ,new_categoryidName AS CategoryIdName
                                                            ,'new_category' AS CategoryIdTypeName
	                                                        ,{0} ", ENTITY_BASE_SELECT);

        public static string GET_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_education (NOLOCK)
                                                            WHERE
	                                                            new_educationId=@Id", BASE_SELECT);

        public static string GET_LIST_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_education (NOLOCK)
                                                            WHERE
	                                                            statecode=0", BASE_SELECT);
    }
}
