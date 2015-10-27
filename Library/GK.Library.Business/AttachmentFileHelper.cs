using GK.Library.Utility;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace GK.Library.Business
{
    public static class AttachmentFileHelper
    {
        public static MsCrmResult CreateAttachmentFile(Annotation note, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_attachment");

                if (!string.IsNullOrEmpty(note.FileName))
                {
                    ent["new_name"] = note.FileName;
                }

                if (!string.IsNullOrEmpty(note.MimeType))
                {
                    ent["new_mimetype"] = note.MimeType;
                }

                if (note.Portal != null)
                {
                    ent["new_portalid"] = note.Portal;
                }

                if (!string.IsNullOrEmpty(note.FilePath))
                {
                    ent["new_filepath"] = note.FilePath;
                }

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "Dosya ekleme başarıyla gerçekleşti.";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult AssociateAttachmentToEntity(Guid attachmentId, EntityReference entity, string relationShipName, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | ASSOCIATE PROCESS |
                AssociateEntitiesRequest request = new AssociateEntitiesRequest();
                request.Moniker1 = new EntityReference("new_attachment", attachmentId);
                request.Moniker2 = entity;
                request.RelationshipName = relationShipName;
                service.Execute(request);
                #endregion

                returnValue.Success = true;
                returnValue.Result = "Dosya eki ilişkilendirme başarıyla gerçekleşti.";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetArticleAttachmentFiles(Guid articleId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                F.new_attachmentId Id
	                                ,F.new_name Name
	                                ,F.new_mimetype MimeType
                                    ,F.new_filepath FilePath
                                FROM
                                new_new_article_new_attachment AS EA (NOLOCK)
	                                JOIN
		                                new_attachment AS F (NOLOCK)
			                                ON
			                                EA.new_attachmentid =F.new_attachmentId
                                            AND
                                            F.new_filepath IS NOT NULL
			                                AND
			                                F.statuscode=1 --Active
			                                AND
			                                F.statecode=0
                                WHERE
                                EA.new_articleid='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, articleId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<AttachmentFile> returnList = new List<AttachmentFile>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        AttachmentFile _attachment = new AttachmentFile();
                        _attachment.AttachmentFileId = (Guid)dt.Rows[i]["Id"];
                        _attachment.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _attachment.MimeType = dt.Rows[i]["MimeType"] != DBNull.Value ? dt.Rows[i]["MimeType"].ToString() : string.Empty;
                        _attachment.FilePath = dt.Rows[i]["FilePath"] != DBNull.Value ? dt.Rows[i]["FilePath"].ToString() : string.Empty;

                        if (_attachment.Name.Contains(".doc") || _attachment.Name.Contains(".docx"))
                        {
                            _attachment.IconFile = "icon-file-word fg-blue";
                        }
                        else if (_attachment.Name.Contains(".xls") || _attachment.Name.Contains(".xlsx"))
                        {
                            _attachment.IconFile = "icon-file-excel fg-green";
                        }
                        else if (_attachment.Name.Contains(".ppt") || _attachment.Name.Contains(".pptx"))
                        {
                            _attachment.IconFile = "icon-file-powerpoint fg-orange";
                        }
                        else if (_attachment.Name.Contains(".pdf"))
                        {
                            _attachment.IconFile = "icon-file-pdf fg-red";
                        }
                        else if (_attachment.Name.Contains(".zip") || _attachment.Name.Contains(".rar"))
                        {
                            _attachment.IconFile = "icon-file-zip fg-red";
                        }
                        else if (_attachment.Name.Contains(".jpg") || _attachment.Name.Contains(".jpeg") || _attachment.Name.Contains(".png") || _attachment.Name.Contains(".gif"))
                        {
                            _attachment.IconFile = "icon-image";
                        }
                        else
                        {
                            _attachment.IconFile = "icon-file";
                        }

                        returnList.Add(_attachment);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
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

        public static MsCrmResultObject GetEducationAttachmentFiles(Guid educationId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                F.new_attachmentId Id
	                                ,F.new_name Name
	                                ,F.new_mimetype MimeType
                                    ,F.new_filepath FilePath
                                FROM
                                new_new_education_new_attachment AS EA (NOLOCK)
	                                JOIN
		                                new_attachment AS F (NOLOCK)
			                                ON
			                                EA.new_attachmentid =F.new_attachmentId
                                            AND
                                            F.new_filepath IS NOT NULL
			                                AND
			                                F.statuscode=1 --Active
			                                AND
			                                F.statecode=0
                                WHERE
                                EA.new_educationid='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, educationId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<AttachmentFile> returnList = new List<AttachmentFile>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        AttachmentFile _attachment = new AttachmentFile();
                        _attachment.AttachmentFileId = (Guid)dt.Rows[i]["Id"];
                        _attachment.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _attachment.MimeType = dt.Rows[i]["MimeType"] != DBNull.Value ? dt.Rows[i]["MimeType"].ToString() : string.Empty;
                        _attachment.FilePath = dt.Rows[i]["FilePath"] != DBNull.Value ? dt.Rows[i]["FilePath"].ToString() : string.Empty;

                        if (_attachment.Name.Contains(".doc") || _attachment.Name.Contains(".docx"))
                        {
                            _attachment.IconFile = "icon-file-word fg-blue";
                        }
                        else if (_attachment.Name.Contains(".xls") || _attachment.Name.Contains(".xlsx"))
                        {
                            _attachment.IconFile = "icon-file-excel fg-green";
                        }
                        else if (_attachment.Name.Contains(".ppt") || _attachment.Name.Contains(".pptx"))
                        {
                            _attachment.IconFile = "icon-file-powerpoint fg-orange";
                        }
                        else if (_attachment.Name.Contains(".pdf"))
                        {
                            _attachment.IconFile = "icon-file-pdf fg-red";
                        }
                        else if (_attachment.Name.Contains(".zip") || _attachment.Name.Contains(".rar"))
                        {
                            _attachment.IconFile = "icon-file-zip fg-red";
                        }
                        else if (_attachment.Name.Contains(".jpg") || _attachment.Name.Contains(".jpeg") || _attachment.Name.Contains(".png") || _attachment.Name.Contains(".gif"))
                        {
                            _attachment.IconFile = "icon-image";
                        }
                        else
                        {
                            _attachment.IconFile = "icon-file";
                        }


                        returnList.Add(_attachment);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
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

        public static MsCrmResultObject GetAnnouncementAttachmentFiles(Guid announcementId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                F.new_attachmentId Id
	                                ,F.new_name Name
	                                ,F.new_mimetype MimeType
                                    ,F.new_filepath FilePath
                                FROM
                                new_new_announcement_new_attachment AS EA (NOLOCK)
	                                JOIN
		                                new_attachment AS F (NOLOCK)
			                                ON
			                                EA.new_attachmentid =F.new_attachmentId
                                            AND
                                            F.new_filepath IS NOT NULL
			                                AND
			                                F.statuscode=1 --Active
			                                AND
			                                F.statecode=0
                                WHERE
                                EA.new_announcementid ='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, announcementId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<AttachmentFile> returnList = new List<AttachmentFile>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        AttachmentFile _attachment = new AttachmentFile();
                        _attachment.AttachmentFileId = (Guid)dt.Rows[i]["Id"];
                        _attachment.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _attachment.MimeType = dt.Rows[i]["MimeType"] != DBNull.Value ? dt.Rows[i]["MimeType"].ToString() : string.Empty;
                        _attachment.FilePath = dt.Rows[i]["FilePath"] != DBNull.Value ? dt.Rows[i]["FilePath"].ToString() : string.Empty;

                        if (_attachment.Name.Contains(".doc") || _attachment.Name.Contains(".docx"))
                        {
                            _attachment.IconFile = "icon-file-word fg-blue";
                        }
                        else if (_attachment.Name.Contains(".xls") || _attachment.Name.Contains(".xlsx"))
                        {
                            _attachment.IconFile = "icon-file-excel fg-green";
                        }
                        else if (_attachment.Name.Contains(".ppt") || _attachment.Name.Contains(".pptx"))
                        {
                            _attachment.IconFile = "icon-file-powerpoint fg-orange";
                        }
                        else if (_attachment.Name.Contains(".pdf"))
                        {
                            _attachment.IconFile = "icon-file-pdf fg-red";
                        }
                        else if (_attachment.Name.Contains(".zip") || _attachment.Name.Contains(".rar"))
                        {
                            _attachment.IconFile = "icon-file-zip fg-red";
                        }
                        else if (_attachment.Name.Contains(".jpg") || _attachment.Name.Contains(".jpeg") || _attachment.Name.Contains(".png") || _attachment.Name.Contains(".gif"))
                        {
                            _attachment.IconFile = "icon-image";
                        }
                        else
                        {
                            _attachment.IconFile = "icon-file";
                        }


                        returnList.Add(_attachment);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "No documents attached!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetAttachmentInfo(Guid attachmentId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"";
                #endregion
            }
            catch (Exception)
            {

            }
            return returnValue;
        }

        #region | ATTACHMENT FOR ASHX |
        public static MsCrmResult SaveAnnotation(Annotation note, string relationName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                int splitLength = note.FileName.Split('.').Length;
                note.FilePath = Guid.NewGuid() + "." + note.FileName.Split('.')[splitLength - 1].ToLower();

                returnValue = AttachmentFileHelper.CreateAttachmentFile(note, service);

                if (returnValue.Success)
                {

                    byte[] data = Convert.FromBase64String(note.File);
                    //File.WriteAllBytes(HostingEnvironment.MapPath("~/attachments") + "/" + note.FilePath, data);
                    File.WriteAllBytes(@Globals.AttachmentFolder + @"\" + note.FilePath, data);

                    note.AttachmentFile = new EntityReference("new_attachment", returnValue.CrmId);
                    returnValue = AttachmentFileHelper.AssociateAttachmentToEntity(returnValue.CrmId, note.Object, relationName, service);
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult SaveObjectProfileImage(Annotation note, string fieldName, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                int splitLength = note.FileName.Split('.').Length;
                note.FilePath = Guid.NewGuid() + "." + note.FileName.Split('.')[splitLength - 1].ToLower();

                byte[] data = Convert.FromBase64String(note.File);

                //File.WriteAllBytes(HostingEnvironment.MapPath("~/attachments") + "/" + note.FilePath, data);
                File.WriteAllBytes(@Globals.AttachmentFolder + @"\" + note.FilePath, data);

                #region | DELETE PREVIOUS IMAGE FILE |

                string fileName = AttachmentFileHelper.GetEntityProfileImageFileName(note.Object.Id.ToString(), note.Object.LogicalName, fieldName, sda);

                if (!string.IsNullOrEmpty(fileName))
                {
                    //string filePath = HostingEnvironment.MapPath("~/attachments") + "/" + fileName;
                    string filePath = @Globals.AttachmentFolder + @"\" + fileName;

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                #endregion

                #region | UPDATE CRM ENTITY |

                Entity ent = new Entity(note.Object.LogicalName.ToLower());
                ent[note.Object.LogicalName.ToLower() + "id"] = note.Object.Id;
                ent[fieldName] = note.FilePath;

                service.Update(ent);

                #endregion

                returnValue.Success = true;
                returnValue.Result = note.FilePath;
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult DeleteProfileImage(string entityId, string entityName, string fieldName, string fileName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                //string filePath = HostingEnvironment.MapPath("~/attachments") + "/" + fileName;
                string filePath = @Globals.AttachmentFolder + @"\" + fileName;

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);

                    Entity ent = new Entity(entityName);
                    ent[entityName + "id"] = new Guid(entityId);
                    ent[fieldName] = null;

                    service.Update(ent);
                }

                returnValue.Success = true;
                returnValue.Result = "Dosya silindi!";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static string GetEntityProfileImageFileName(string entityId, string entityName, string fieldName, SqlDataAccess sda)
        {
            string returnValue = string.Empty;

            #region | SQL QUERY |
            string sqlQuery = @"SELECT c.{0} AS FileName FROM Filtered{1} AS c WHERE c.{1}id='{2}' AND c.{0} IS NOT NULL";
            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, fieldName.ToLower(), entityName.ToLower(), entityId.ToLower()));

            if (dt.Rows.Count > 0)
            {
                returnValue = dt.Rows[0]["FileName"].ToString();
            }
            return returnValue;
        }

        public static MsCrmResult UpdateProfileImage(Guid userId, string fileName, string fieldName, SqlDataAccess sda, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | DELETE PREVIOUS IMAGE FILE |

                string oldFileName = AttachmentFileHelper.GetEntityProfileImageFileName(userId.ToString(), "new_user", fieldName, sda);

                if (!string.IsNullOrEmpty(oldFileName))
                {
                    //string filePath = HostingEnvironment.MapPath("~/attachments") + "/" + oldFileName;
                    string filePath = @Globals.AttachmentFolder + @"\" + oldFileName;

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                #endregion

                #region | UPDATE CRM ENTITY |

                Entity ent = new Entity("new_user");
                ent.Id = userId;
                ent[fieldName] = fileName;

                service.Update(ent);

                #endregion

                returnValue.Success = true;
                returnValue.Result = fileName;
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult SaveGraffitiImage(Guid graffitiId, string fileName, string fieldName, SqlDataAccess sda, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | UPDATE CRM ENTITY |

                Entity ent = new Entity("new_graffiti");
                ent.Id = graffitiId;
                ent[fieldName] = fileName;

                service.Update(ent);

                #endregion

                returnValue.Success = true;
                returnValue.Result = fileName;
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }
        #endregion
    }
}
