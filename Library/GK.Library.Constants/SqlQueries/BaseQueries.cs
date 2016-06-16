using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Constants.SqlQueries
{
    public class BaseQueries
    {
        public const string ENTITY_BASE_SELECT = @" statuscode AS Status
	                                                ,statecode AS State
	                                                ,createdon AS CreatedOn
	                                                ,modifiedon AS ModifiedOn
	                                                ,OwnerId AS OwnerId
	                                                ,OwnerIdName AS OwnerIdName
	                                                ,'systemuser' AS OwnerIdTypeName ";
    }
}
