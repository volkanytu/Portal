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
    public class AnnotationDao : BaseDao<Annotation>, IAnnotationDao
    {
        private IMsCrmAccess _msCrmAccess;
        private ISqlAccess _sqlAccess;

        public AnnotationDao(IMsCrmAccess msCrmAccess, ISqlAccess sqlAccess)
            : base(msCrmAccess, sqlAccess)
        {
            _msCrmAccess = msCrmAccess;
            _sqlAccess = sqlAccess;

        }

        public List<Annotation> GetObjectAnnotationList(Guid objectId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@objectId",objectId)
            };

            //TODO: Get By Object Id Query
            DataTable dt = _sqlAccess.GetDataTable("", parameters);

            return dt.ToList<Annotation>();
        }

        public override string EntityName
        {
            get { return "annotation"; }
        }

        public override string GetQuery
        {
            get { return AnnotationQueries.GET_QUERY; }
        }

        public override string GetListQuery
        {
            get { return AnnotationQueries.GET_LIST_QUERY; }
        }
    }
}
