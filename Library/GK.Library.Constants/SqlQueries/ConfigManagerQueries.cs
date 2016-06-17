using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Constants.SqlQueries
{
    public class ConfigManagerQueries
    {
        #region | GET_CONFIG_VARS |

        public const string GET_CONFIG_VARS = @"SELECT
	                                                cnf.new_name AS [Key]
	                                                ,cnf.new_value AS [Value]
                                                FROM
                                                new_config AS cnf (NOLOCK)";

        #endregion
    }
}
