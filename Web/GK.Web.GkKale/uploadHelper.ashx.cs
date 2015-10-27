using GK.Library.Business;
using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace GK.Web.GkKale
{
    /// <summary>
    /// Summary description for uploadHelper
    /// </summary>
    public class uploadHelper : IHttpHandler
    {

        IOrganizationService _service = null;
        SqlDataAccess _sda = null;

        public void ProcessRequest(HttpContext context)
        {
            FileLogHelper.LogEvent("Kale-uploadHelper", @"C:\DO\");

            context.Response.ContentType = "application/json";

            object returnValue = null;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                string operation = context.Request.QueryString["operation"];
                string userId = context.Request.QueryString["userid"];
                string graffitiId = context.Request.QueryString["graffitiId"];

                if (operation == "1" && !string.IsNullOrEmpty(userId)) //Profile resmi güncelleme
                {
                    returnValue = new MsCrmResult();

                    _service = MSCRM.GetOrgService(true);
                    HttpPostedFile file = context.Request.Files[0];

                    returnValue = SaveProfileImage(userId, file, context);
                }
                else if (operation == "2" && !string.IsNullOrEmpty(graffitiId)) //Duvar yazısı resim ekleme
                {
                    returnValue = new MsCrmResult();

                    _service = MSCRM.GetOrgService(true);
                    HttpPostedFile file = context.Request.Files[0];

                    returnValue = SaveGraffitiImage(new Guid(graffitiId), file, context);
                }
                else
                {
                    ((MsCrmResultObject)returnValue).Result = "Eksik veya yanlış parametre.";
                }
            }
            catch (Exception ex)
            {
                returnValue = new MsCrmResult();
                ((MsCrmResult)returnValue).HasException = true;
                ((MsCrmResult)returnValue).Result = ex.Message;

            }
            finally
            {
                if (_sda != null)
                {
                    _sda.closeConnection();

                }
            }

            var dataRes = serializer.Serialize(returnValue);
            context.Response.Write(dataRes);

        }

        protected MsCrmResult SaveProfileImage(string userId, HttpPostedFile file, HttpContext context)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                string filePath = @Globals.AttachmentFolder;
                //string filePath = context.Server.MapPath("attachments\\");

                FileInfo fi = new FileInfo(file.FileName);
                string ext = fi.Extension;

                string newFileName = Guid.NewGuid().ToString() + ext;

                FileInfo imageFile = new FileInfo(filePath + newFileName);

                file.SaveAs(imageFile.FullName);

                returnValue = AttachmentFileHelper.UpdateProfileImage(new Guid(userId), newFileName, "new_imageurl", _sda, _service);

            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;

            }
            return returnValue;
        }

        protected MsCrmResult SaveGraffitiImage(Guid graffitiId, HttpPostedFile file, HttpContext context)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                string filePath = @Globals.AttachmentFolder;
                //string filePath = context.Server.MapPath("attachments\\");

                FileInfo fi = new FileInfo(file.FileName);
                string ext = fi.Extension;

                string newFileName = Guid.NewGuid().ToString() + ext;

                FileInfo imageFile = new FileInfo(filePath + newFileName);

                file.SaveAs(imageFile.FullName);

                returnValue = AttachmentFileHelper.SaveGraffitiImage(graffitiId, newFileName, "new_imageurl", _sda, _service);

            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;

            }
            return returnValue;
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