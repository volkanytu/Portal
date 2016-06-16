using System;
using System.Reflection;

namespace GK.WebServices.WEBAPI.CrmApi.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}