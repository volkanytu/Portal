using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Constants.SqlQueries
{
    public class PortalQueries : BaseQueries
    {
        private static string BASE_SELECT = string.Format(@" new_portalId AS Id
	                                                        ,new_name AS Name
	                                                        ,{0} ", ENTITY_BASE_SELECT);

        public static string GET_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_portal (NOLOCK)
                                                            WHERE
	                                                            new_portalId=@Id", BASE_SELECT);

        public static string GET_LIST_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            new_portal (NOLOCK)
                                                            WHERE
	                                                            statecode=0", BASE_SELECT);
    }
}
