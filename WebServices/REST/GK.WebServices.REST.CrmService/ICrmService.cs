using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GK.WebServices.REST.CrmService
{
    [ServiceContract]
    public interface ICrmService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetPortal")]
        string GetPortal(string url);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CheckLogin")]
        MsCrmResult CheckLogin(string portalId, string userName, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserInfo")]
        string GetUserInfo(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SendMyPassword")]
        MsCrmResult SendMyPassword(string userName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/UpdateMyProfile")]
        MsCrmResult UpdateMyProfile(string token, string newPassword, string oldPassword, Contact contact);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetGraffitiList")]
        string GetGraffitiList(string token, int commentCount, int start, int end);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetGraffitiComments")]
        string GetGraffitiComments(string token, string graffitiId, int start, int end);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetEducationList")]
        string GetEducationList(string token, string categoryId, int start, int end);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetEducationInfo")]
        string GetEducationInfo(string token, string educationId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetarticleList")]
        string GetarticleList(string token, string categoryId, int start, int end);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetArticleInfo")]
        string GetArticleInfo(string token, string articleId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetAnnouncementList")]
        string GetAnnouncementList(string token, int start, int end);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetAnnouncementInfo")]
        string GetAnnouncementInfo(string token, string announcementId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetAnnotationDetail")]
        string GetAnnotationDetail(string objectId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveAnnotation")]
        MsCrmResult SaveAnnotation(Annotation note, string relationName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveAnswer")]
        MsCrmResult SaveAnswer(string token, Answer answer);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveComment")]
        MsCrmResult SaveComment(string token, Comment comment);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveArticle")]
        MsCrmResult SaveArticle(Article article);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveGraffiti")]
        MsCrmResult SaveGraffiti(string token, Graffiti graffiti);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetAnnotations")]
        List<Annotation> GetAnnotations(string objectId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetQuestionLevels")]
        string GetQuestionLevels(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetQuestionInfo")]
        string GetQuestionInfo(string questionId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SelectQuestion")]
        string SelectQuestion(string token, string questionLevelId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserCubeStatus")]
        string GetUserCubeStatus(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetCubeStatusList")]
        string GetCubeStatusList(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/HasUserQuestionLimit")]
        MsCrmResult HasUserQuestionLimit(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/UpdateLogoutDate")]
        MsCrmResult UpdateLogoutDate(string loginLogId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/LogEducation")]
        MsCrmResult LogEducation(string token, string educationId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/LogArticle")]
        MsCrmResult LogArticle(string token, string articleId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveObjectProfileImage")]
        MsCrmResult SaveObjectProfileImage(Annotation note, string fieldName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/DeleteProfileImage")]
        MsCrmResult DeleteProfileImage(string entityId, string entityName, string fieldName, string fileName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetPortalInfo")]
        string GetPortalInfo(string portalId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetVideoList")]
        string GetVideoList(string token, string categoryId, int start, int end);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetVideoInfo")]
        string GetVideoInfo(string token, string videoId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/LogVideo")]
        MsCrmResult LogVideo(string token, string videoId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CreateFriendshipRequest")]
        MsCrmResult CreateFriendshipRequest(string token, FriendshipRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CheckIsUserYourFriend")]
        MsCrmResult CheckIsUserYourFriend(string token, string selectedUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/HasUserRequestWithYou")]
        string HasUserRequestWithYou(string token, string selectedUserId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CloseFriendshipRequest")]
        MsCrmResult CloseFriendshipRequest(string token, string requestId, int statusCode);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CloseFriendship")]
        MsCrmResult CloseFriendship(string token, string friendshipId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserFriendList")]
        string GetUserFriendList(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SearchContact")]
        string SearchContact(string token, string key);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserForums")]
        string GetUserForums(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetForumSubjects")]
        string GetForumSubjects(string token, string forumId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetForumSubjectInfo")]
        string GetForumSubjectInfo(string token, string forumSubjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CreateForumSubject")]
        MsCrmResult CreateForumSubject(string token, ForumSubject forumSubject);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetPageContent")]
        MsCrmResult GetPageContent(string token, int pageNo);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/UpdateWelcomePageGenerated")]
        MsCrmResult UpdateWelcomePageGenerated(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/UpdateContractApprove")]
        MsCrmResult UpdateContractApprove(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SelectSurvey")]
        string SelectSurvey(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/AnswerSurvey")]
        MsCrmResult AnswerSurvey(string token, SurveyAnswer answer);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CheckSession")]
        string CheckSession(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetFriendUserInfo")]
        string GetFriendUserInfo(string token, string userId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CloseSession")]
        MsCrmResult CloseSession(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetEntityComments")]
        string GetEntityComments(string token, string entityId, string entityName, int start, int end);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/LikeEntity")]
        MsCrmResult LikeEntity(string token, string entityId, string entityName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/DislikeEntity")]
        MsCrmResult DislikeEntity(string token, string entityId, string entityName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetEntityLikeInfo")]
        string GetEntityLikeInfo(string token, string entityId, string entityName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetVideoCategoryList")]
        string GetVideoCategoryList(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetArticleCategoryList")]
        string GetArticleCategoryList(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetEducationCategoryList")]
        string GetEducationCategoryList(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CheckPhoneNumberMatch")]
        MsCrmResult CheckPhoneNumberMatch(string userName, string phoneNumber, string portalId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SendPasswordSMS")]
        MsCrmResult SendPasswordSMS(string portalUserId, string portalId, string phoneNumber, string contactId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/ConfirmPasswordCode")]
        MsCrmResult ConfirmPasswordCode(string token, string code);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/UpdateUserPassword")]
        MsCrmResult UpdateUserPassword(string token, string newPassword);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/ReportImproperContent")]
        MsCrmResult ReportImproperContent(string token, string entityId, string entityName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetEducationCategoryInfo")]
        string GetEducationCategoryInfo(string token, string categoryId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetVideoCategoryInfo")]
        string GetVideoCategoryInfo(string token, string categoryId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetArticleCategoryInfo")]
        string GetArticleCategoryInfo(string token, string categoryId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserPointDetail")]
        string GetUserPointDetail(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetModuleRecordCounts")]
        string GetModuleRecordCounts(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/UsePointCode")]
        MsCrmResult UsePointCode(string token, string pointCode);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetGiftCategoryList")]
        string GetGiftCategoryList(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetGiftList")]
        string GetGiftList(string token, string categoryId, string sortType);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetGiftInfo")]
        string GetGiftInfo(string token, string giftId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetOldMessages")]
        List<MessageInfo> GetOldMessages(string portalId, string from, string to);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/UpdateMessageAsSeen")]
        MsCrmResult UpdateMessageAsSeen(string messageId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserRecentContacts")]
        string GetUserRecentContacts(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CreateUserGiftRequest")]
        MsCrmResult CreateUserGiftRequest(string token, string giftId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetCities")]
        List<EntityReference> GetCities();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetTowns")]
        List<EntityReference> GetTowns(string cityId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "?token={token}&search={search}")]
        List<SelectValue> SearchUserTokenize(string token, string search);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CreateMessage")]
        MessageInfo CreateMessage(string token, MessageInfo message);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveDiscoveryForm")]
        MsCrmResult SaveDiscoveryForm(string token, DiscoveryForm discForm);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetRequests")]
        MsCrmResultObj<List<AssemblyRequestInfo>> GetRequests(string token, string userId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetRequestInfo")]
        MsCrmResultObj<AssemblyRequestInfo> GetRequestInfo(string token, string requestId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/CompleteRequest")]
        MsCrmResult CompleteRequest(string token, string requestId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/AnswerNpsSurvey")]
        MsCrmResult AnswerNpsSurvey(string npsSurveyId, int suggest, int suggestPoint);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserDiscoveryFormList")]
        MsCrmResultObj<List<DiscoveryForm>> GetUserDiscoveryFormList(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUnReadMessages")]
        MsCrmResultObj<List<Message>> GetUnReadMessages(string token, string requestId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/RegisterUser")]
        MsCrmResult RegisterUser(Contact contact);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/SaveSteelDoorForm")]
        MsCrmResult SaveSteelDoorForm(string token, SteelDoor steelDoor);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/GetUserSteelDoors")]
        MsCrmResultObj<List<SteelDoor>> GetUserSteelDoors(string token);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , UriTemplate = "/DeleteGraffiti")]
        MsCrmResult DeleteGraffiti(string token, string graffitiId);
    }
}
