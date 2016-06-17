using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Constants.SqlQueries
{
    public class AnnotationQueries : BaseQueries
    {
        private static string BASE_SELECT = string.Format(@" AnnotationId AS Id
	                                                        ,Subject AS Name
                                                            ,ObjectId AS ObjectId
                                                            ,ObjectId AS ObjectIdName
                                                            ,'new_portal' AS ObjectIdTypeName
                                                            ,DocumentBody
                                                            ,FileName
                                                            ,MimeType
                                                            ,NoteText ");

        public static string GET_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            Annotation (NOLOCK)
                                                            WHERE
	                                                            AnnotationId=@Id", BASE_SELECT);

        public static string GET_LIST_QUERY = string.Format(@"SELECT
	                                                           {0}
                                                            FROM
	                                                            Annotation (NOLOCK)
                                                            WHERE
	                                                            statecode=0", BASE_SELECT);

        public static string GET_LIST_QUERY_BY_OBJECT_ID = string.Format(@"SELECT
	                                                                           {0}
                                                                            FROM
	                                                                            Annotation (NOLOCK)
                                                                            WHERE
	                                                                            ObjectId=@objectId", BASE_SELECT);
    }
}
