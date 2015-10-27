using GK.Library.Business;
using GK.Library.Utility;

using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GK.Web.FileUpload
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var operation = context.Request.QueryString["operation"];

                if (context.Request.QueryString["upload"] != null && operation != null && operation != "3")
                {
                    string pathrefer = context.Request.UrlReferrer.ToString();
                    //string Serverpath = @"C:\DO\Web\GK.Web.GkPortal\attachments";
                    //string Serverpath = HttpContext.Current.Server.MapPath("attachments");
                    string Serverpath = @Globals.AttachmentFolder;

                    var postedFile = context.Request.Files[0];

                    string file;

                    //For IE to get file name
                    if (HttpContext.Current.Request.Browser.Browser.ToLower() == "internetexplorer")
                    {
                        string[] files = postedFile.FileName.Split(new char[] { '\\' });
                        file = files[files.Length - 1];
                    }
                    else
                    {
                        file = postedFile.FileName;
                    }


                    if (!Directory.Exists(Serverpath))
                        Directory.CreateDirectory(Serverpath);

                    string fileDirectory = Serverpath;
                    if (context.Request.QueryString["fileName"] != null)
                    {
                        file = context.Request.QueryString["fileName"];
                        if (File.Exists(fileDirectory + "\\" + file))
                        {
                            File.Delete(fileDirectory + "\\" + file);
                        }
                    }

                    string ext = Path.GetExtension(fileDirectory + "\\" + file);
                    file = Guid.NewGuid() + ext;

                    fileDirectory = Serverpath + "\\" + file;

                    postedFile.SaveAs(fileDirectory);

                    context.Response.AddHeader("Vary", "Accept");
                    try
                    {
                        if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                            context.Response.ContentType = "application/json";
                        else
                            context.Response.ContentType = "text/plain";
                    }
                    catch
                    {
                        context.Response.ContentType = "text/plain";
                    }

                    #region | CRM OPERATIONS AFTER SAVE |

                    if (context.Request.QueryString["operation"] != null)
                    {
                        var relationName = context.Request.QueryString["relationname"];
                        var entityName = context.Request.QueryString["entityname"];
                        var entityId = context.Request.QueryString["entityid"];
                        var portalId = context.Request.QueryString["portalid"];
                        var isImage = context.Request.QueryString["isimage"];

                        IOrganizationService service = MSCRM.GetOrgService(true);
                        SqlDataAccess sda = new SqlDataAccess();
                        sda.openConnection(Globals.ConnectionString);

                        Annotation note = new Annotation();

                        if (operation != "3")
                        {
                            note.FileName = postedFile.FileName;
                            note.FilePath = file;
                            note.MimeType = ext;
                            note.Object = new EntityReference(entityName, new Guid(entityId));

                            if (operation == "1")
                            {
                                note.Portal = new EntityReference("new_portal", new Guid(portalId));
                            }
                        }

                        if (operation == "1") //Save attachment to entity
                        {
                            MsCrmResult resultCreate = AttachmentFileHelper.CreateAttachmentFile(note, service);

                            AttachmentFileHelper.AssociateAttachmentToEntity(resultCreate.CrmId, note.Object, relationName, service);
                        }
                        else if (operation == "2") //Save Entity Profile Image
                        {
                            string fieldName = "new_imageurl";

                            if (entityName == "new_video" && isImage != "true")
                            {
                                fieldName = "new_videourl";
                            }

                            #region | DELETE PREVIOUS IMAGE FILE |

                            string fileName = AttachmentFileHelper.GetEntityProfileImageFileName(entityId, entityName, fieldName, sda);

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                string filePath = @Globals.AttachmentFolder + @"\" + fileName;

                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                            }

                            #endregion

                            #region | UPDATE CRM ENTITY |

                            Entity ent = new Entity(entityName);
                            ent.Id = new Guid(entityId);
                            ent[fieldName] = note.FilePath;

                            service.Update(ent);

                            #endregion
                        }
                        //else if (operation == "3") //Delete profile image
                        //{
                        //    string fieldName = "new_imageurl";

                        //    if (entityName == "new_video")
                        //    {
                        //        fieldName = "new_imageurl";
                        //    }

                        //    string fileName = AttachmentFileHelper.GetEntityProfileImageFileName(entityId, entityName, fieldName, sda);

                        //    AttachmentFileHelper.DeleteProfileImage(entityId, entityName, fieldName, fileName);
                        //}
                    }

                    #endregion

                    context.Response.Write(file);
                }
                else if (operation == "3")
                {
                    var entityName = context.Request.QueryString["entityname"];
                    var entityId = context.Request.QueryString["entityid"];

                    string fieldName = "new_imageurl";

                    if (entityName == "new_video")
                    {
                        fieldName = "new_imageurl";
                    }

                    SqlDataAccess sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    string fileName = AttachmentFileHelper.GetEntityProfileImageFileName(new Guid(entityId).ToString(), entityName, fieldName, sda);

                    AttachmentFileHelper.DeleteProfileImage(new Guid(entityId).ToString(), entityName, fieldName, fileName);

                    context.Response.Write("success");
                }

            }
            catch (Exception exp)
            {
                context.Response.Write(exp.StackTrace);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}