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
	                                                v.[Key]
	                                                ,v.Value AS [Value]
                                                FROM
                                                t_Config AS v (NOLOCK)";

        #endregion
    }
}
