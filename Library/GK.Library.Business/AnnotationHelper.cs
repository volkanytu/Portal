using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class AnnotationHelper
    {
        public static MsCrmResult CreateAnnotation(Annotation note, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("annotation");

                if (note.AttachmentFile != null)
                {
                    ent["objectid"] = note.AttachmentFile;
                }

                if (!string.IsNullOrEmpty(note.Subject))
                {
                    ent["subject"] = note.Subject;
                }

                if (!string.IsNullOrEmpty(note.File))
                {
                    ent["documentbody"] = note.File;
                }

                if (!string.IsNullOrEmpty(note.FileName))
                {
                    ent["filename"] = note.FileName;
                }

                if (!string.IsNullOrEmpty(note.MimeType))
                {
                    ent["mimetype"] = note.MimeType;
                }

                if (!string.IsNullOrEmpty(note.Text))
                {
                    ent["notetext"] = note.Text;
                }

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "Dosya Ekleme başarıyla gerçekleşti.";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static List<Annotation> GetAnnotationListByObject(Guid objectId, SqlDataAccess sda)
        {
            List<Annotation> returnList = new List<Annotation>();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                A.AnnotationId
	                                ,A.MimeType
	                                ,A.FileName
                                FROM
	                                Annotation A
                                WHERE
	                                A.ObjectId = '{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, objectId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Annotation note = new Annotation();
                        note.AnnotationId = (Guid)dt.Rows[i]["AnnotationId"];
                        note.MimeType = dt.Rows[i]["MimeType"] != DBNull.Value ? dt.Rows[i]["MimeType"].ToString() : string.Empty;
                        note.FileName = dt.Rows[i]["FileName"] != DBNull.Value ? dt.Rows[i]["FileName"].ToString() : string.Empty;
                        returnList.Add(note);
                    }
                }
            }
            catch (Exception)
            {

            }
            return returnList;
        }

        public static Annotation GetAnnotationDetail(Guid annotationId, SqlDataAccess sda)
        {
            Annotation returnValue = new Annotation();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                A.AnnotationId
	                                ,A.MimeType
	                                ,A.FileName
                                    ,A.DocumentBody
                                FROM
	                                Annotation A
                                WHERE
	                                A.AnnotationId = '{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, annotationId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    returnValue.AnnotationId = (Guid)dt.Rows[0]["AnnotationId"];
                    returnValue.MimeType = dt.Rows[0]["MimeType"] != DBNull.Value ? dt.Rows[0]["MimeType"].ToString() : string.Empty;
                    returnValue.FileName = dt.Rows[0]["FileName"] != DBNull.Value ? dt.Rows[0]["FileName"].ToString() : string.Empty;
                    returnValue.File = dt.Rows[0]["DocumentBody"] != DBNull.Value ? dt.Rows[0]["DocumentBody"].ToString() : string.Empty;
                }
            }
            catch (Exception)
            {

            }
            return returnValue;
        }

        public static MsCrmResultObject GetAnnotationDetailByObject(Guid objectId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                A.AnnotationId
	                                ,A.DocumentBody
                                FROM
	                                Annotation A
                                WHERE
	                                A.ObjectId = '{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, objectId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    Annotation _annotation = new Annotation();
                    _annotation.AnnotationId = (Guid)dt.Rows[0]["AnnotationId"];
                    _annotation.File = dt.Rows[0]["DocumentBody"] != DBNull.Value ? dt.Rows[0]["DocumentBody"].ToString() : string.Empty;

                    returnValue.Success = true;
                    returnValue.ReturnObject = _annotation;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Herhangi bir dosya bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

    }
}
