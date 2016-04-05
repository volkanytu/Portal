using System.Globalization;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Web;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace GK.ServiceLibrary.GiftServiceLayer
{
    /// <summary>
    /// Sahibinden admin servislerinin kullanılarak yapılan işlemlerin gerçekleştirildiği servistir
    /// </summary>
    public class BaseAdminService
    {
        public BaseAdminService()
        {

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        protected WebHeaderCollection GetWebRequestHeaders()
        {
            string apiKey = "6r9ZTok8zp4yZxKq";

            WebHeaderCollection whc = new WebHeaderCollection();
            whc.Add("Cache-Control", "max-age=0");
            whc.Add("Accept-Charset", "utf-8");
            whc.Add("x-api-key", apiKey);
            return whc;
        }

        protected TResult GetResponseOfWebRequest<TResult>(string requestUrl, string jsonData, string functionName) where TResult : new()
        {
            TResult returnValue = new TResult();

            WebRequest request = WebRequest.Create(requestUrl);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            //request.Headers = GetWebRequestHeaders();

            byte[] postBytes = Encoding.UTF8.GetBytes(jsonData);
            request.ContentLength = postBytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                returnValue = serializer.Deserialize<TResult>(result);
            }

            httpResponse.Close();

            return returnValue;
        }

        protected TResult GetResponseOfWebRequest<TResult>(string requestUrl, string functionName) where TResult : new()
        {
            TResult returnValue = new TResult();

            WebRequest request = WebRequest.Create(requestUrl);

            request.ContentType = "application/json;charset=UTF-8";
            request.Method = "GET";
            //request.Headers = GetWebRequestHeaders();

            HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                returnValue = serializer.Deserialize<TResult>(result);
            }

            httpResponse.Close();

            return returnValue;
        }
    }
}
