using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CustomEntities
{
    public class Response
    {
        public bool Success { get; set; }
        public ResponseMessage Message { get; set; }

        public string FullMessage { get { return string.Format("{0} ({1})", Message.Text, Message.Code); } }

        public Response()
        {
            Message = new ResponseMessage();
        }

        public Response(ResponseMessageDefinitionEnum definitionEnum, params object[] param)
            : this()
        {
            SetMessage(definitionEnum, param);
        }

        public Response SetSuccess()
        {
            SetMessage(ResponseMessageDefinitionEnum.Success);
            Success = true;
            return this;
        }

        public Response SetGeneralException()
        {
            return SetMessage(ResponseMessageDefinitionEnum.GeneralException);
        }

        public Response SetMessage(ResponseMessageDefinitionEnum definitionEnum, params object[] param)
        {
            Success = false;
            Message.Code = (int)definitionEnum;
            Message.Text = string.Format(definitionEnum.GetDescription(), param);
            return this;
        }
    }
}
