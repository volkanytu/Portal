using GK.Library.Constants.SqlQueries;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Data.SqlDataLayer
{
    public class SessionDao : BaseDao<SessionData>
    {
        private IMsCrmAccess _msCrmAccess;
        private ISqlAccess _sqlAccess;

        public SessionDao(IMsCrmAccess msCrmAccess, ISqlAccess sqlAccess)
            : base(msCrmAccess, sqlAccess)
        {
            _msCrmAccess = msCrmAccess;
            _sqlAccess = sqlAccess;
        }

        public override string EntityName
        {
            get { return "new_session"; }
        }

        public override string GetQuery
        {
            get { return SessionQueries.GET_QUERY; }
        }

        public override string GetListQuery
        {
            get { return SessionQueries.GET_LIST_QUERY; }
        }
    }
}
