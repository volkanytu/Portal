using GK.Library.Constants.SqlQueries;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Data.SqlDataLayer
{
    public class UserDao : BaseDao<User>, IUserDao
    {
        private IMsCrmAccess _msCrmAccess;
        private ISqlAccess _sqlAccess;

        public UserDao(IMsCrmAccess msCrmAccess, ISqlAccess sqlAccess)
            : base(msCrmAccess, sqlAccess)
        {
            _msCrmAccess = msCrmAccess;
            _sqlAccess = sqlAccess;
        }

        public User GetByName(string name)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name",name)
            };

            DataTable dt = _sqlAccess.GetDataTable(UserQueries.GET_USER_BY_NAME, parameters);

            return dt.ToList<User>().FirstOrDefault();
        }

        public override string EntityName
        {
            get { return "new_user"; }
        }

        public override string GetQuery
        {
            get { return UserQueries.GET_QUERY; }
        }

        public override string GetListQuery
        {
            get { return PortalQueries.GET_LIST_QUERY; }
        }
    }
}
