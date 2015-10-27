using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class AnswerHelper
    {
        public static MsCrmResult SaveOrUpdateAnswer(Answer answer, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_questionanswers");

                ent["new_name"] = answer.User.Name + "-" + DateTime.Now.ToString("dd.MM.yyyy");

                ent["new_istrust"] = answer.IsTrust;
                ent["new_istimeover"] = answer.IsTimeOverlap;
                ent["new_isrefreshorback"] = answer.IsRefreshOrBack;

                #region | SET POINT |
                ent["new_point"] = 0;

                if (answer.IsCorrect)
                {
                    if (answer.IsTrust)
                    {
                        ent["new_point"] = 2 * answer.Point;
                    }
                    else
                    {
                        ent["new_point"] = answer.Point;
                    }
                }

                if (answer.IsTrust && (!answer.IsCorrect || answer.IsRefreshOrBack || answer.IsTimeOverlap))
                {
                    ent["new_point"] = -1 * answer.Point;
                }

                #endregion


                if (answer.Question != null)
                {
                    ent["new_questionid"] = answer.Question;
                }

                if (answer.Portal != null)
                {
                    ent["new_portalid"] = answer.Portal;
                }

                if (answer.Choice != null)
                {
                    ent["new_questionchoiceid"] = answer.Choice;
                }

                if (answer.User != null)
                {
                    ent["new_userid"] = answer.User;
                }

                if (answer.Id != Guid.Empty)
                {
                    ent["new_questionanswersid"] = answer.Id;

                    service.Update(ent);
                    returnValue.Success = true;
                    returnValue.Result = "Cevap kaydı güncellendi.";
                }
                else
                {
                    returnValue.CrmId = service.Create(ent);
                    returnValue.Success = true;
                    returnValue.Result = "M031"; //"Cevap kaydı oluşturuldu.";
                }

            }
            catch (Exception)
            {

                throw;
            }
            return returnValue;
        }
    }
}
