using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GK.Library.Utility
{
    public class SelectValue
    {
        public string htmlField { get; set; }
        public string text { get; set; }
        public string value { get; set; }
    }

    public class TelephoneNumber
    {
        public Guid PhoneNumberID { get; set; }
        public string TelephoneNo { get; set; }
        public string countryCode { get; set; }
        public string phoneCode { get; set; }
        public string phoneNo { get; set; }
        public bool IsPreffered { get; set; }
        public bool IsDefault { get; set; }
        public bool isFormatOK { get; set; }
        public EntityReference Account { get; set; }
        public string CreatedOnString { get; set; }
        public int RecordSource { get; set; }
    }

    public class ModuleCount
    {
        public int RecCount { get; set; }
        public int RecType { get; set; }
    }

    public class Score
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference User { get; set; }
        public int Point { get; set; }
        public ScoreType ScoreType { get; set; }
        public string ScoreTypeString { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
    }

    public class ScoreLimit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ScorePeriod Period { get; set; }
        public int Point { get; set; }
        public int Frequency { get; set; }
        public ScoreType ScoreType { get; set; }
    }

    public class SendSmsResult
    {
        public Guid SendSmsCrmId { get; set; }
        public string StatusCode { get; set; }
        public string MessageId { get; set; }
    }
    public class SendSmsRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Contact { get; set; }
        public string PhoneNumber { get; set; }
        public SmsConfiguration SmsConfiguration { get; set; }
        public DateTime? PrefferedSendDate { get; set; }
        public string Content { get; set; }
        public string ResultCode { get; set; }
        public string MessageId { get; set; }
        public bool IsSent { get; set; }
        public int StatusCode { get; set; }
        public string Error { get; set; }
        public EntityReference Portal { get; set; }
    }

    public class SmsConfiguration
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GsmOperators Operator { get; set; }
        public string AccountNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ShortNumber { get; set; }
        public string Orginator { get; set; }
        public int StatusCode { get; set; }
    }

    public class LikeInfo
    {
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public EntityReference Entity { get; set; }
    }

    public class LoginSession
    {
        public string Token { get; set; }
        public Guid PortalId { get; set; }
        public Guid PortalUserId { get; set; }
        public Guid LanguageId { get; set; }
    }

    public class PasswordSession
    {
        public string Token { get; set; }
        public string PortalId { get; set; }
        public string PortalUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string SmsCode { get; set; }
    }

    public class PortalInfo
    { //test
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Language { get; set; }
        public string ImagePath { get; set; }
        public int StatusCode { get; set; }
        public Annotation PortalLoginImage { get; set; }
        public Annotation LogoImage { get; set; }
    }

    public class Contact
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string FunctionName { get; set; }
        public EntityReference ParentAccount { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string IdentityNumber { get; set; }
        public int? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Description { get; set; }
        public EntityReference CityId { get; set; }
        public EntityReference TownId { get; set; }
        public string AddressDetail { get; set; }
        public bool MarkContact { get; set; }
    }

    public class PortalUser
    {
        public Guid PortalUserId { get; set; }
        public Contact ContactInfo { get; set; }
        public EntityReference Contact { get; set; }
        public EntityReference Language { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public bool IsPasswordChangeEver { get; set; }
        public bool IsUserLoginEver { get; set; }
        public bool IsDisclaimerApproved { get; set; }
        public DateTime? FirstLoginDate { get; set; }
        public DateTime? PasswordChangeDate { get; set; }
        public DateTime? DisclaimerApproveDate { get; set; }
        public bool IsWelcomeMessageGenerate { get; set; }
        public bool IsWelcomeSmsGenerate { get; set; }
        public List<Role> RoleList { get; set; }
    }

    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
    }

    public class LoginLog
    {
        public Guid LoginLogId { get; set; }
        public string Name { get; set; }
        public EntityReference Contact { get; set; }
        public int LoginTime { get; set; }
    }

    public class Education
    {
        public Guid EducationId { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference Language { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public List<Comment> CommentList { get; set; }
        public List<AttachmentFile> AttachmentFileList { get; set; }
        public int? CommentCount { get; set; }
        public EntityReference Category { get; set; }
        public LikeInfo LikeDetail { get; set; }
    }

    public class Article
    {
        public Guid ArticleId { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference Language { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public List<Comment> CommentList { get; set; }
        public List<AttachmentFile> AttachmentFileList { get; set; }
        public int? CommentCount { get; set; }
        public EntityReference Category { get; set; }
        public LikeInfo LikeDetail { get; set; }
    }

    public class Video
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference Language { get; set; }
        public string Summary { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }
        public string YouTubeUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public List<Comment> CommentList { get; set; }
        public int? CommentCount { get; set; }
        public EntityReference Category { get; set; }
        public LikeInfo LikeDetail { get; set; }
    }


    public class Graffiti
    {
        public Guid GraffitiId { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference PortalUser { get; set; }
        public string PortalUserImage { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public string ImagePath { get; set; }
        public List<Comment> CommentList { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool HasMedia { get; set; }
    }

    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Description { get; set; }
        public EntityReference PortalUser { get; set; }
        public EntityReference Education { get; set; }
        public EntityReference Article { get; set; }
        public EntityReference Graffiti { get; set; }
        public EntityReference Video { get; set; }
        public EntityReference ForumSubject { get; set; }
        public string CreatedOnString { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string PortalUserImage { get; set; }
        public EntityReference Portal { get; set; }
    }

    public class Announcement
    {
        public Guid AnnouncementId { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public List<AttachmentFile> AttachmentFileList { get; set; }
    }

    //public class Question
    //{
    //    public Guid QuestionId { get; set; }
    //    public string Name { get; set; }
    //    public int Point { get; set; }
    //}

    //public class Choice
    //{
    //    public Guid ChoiceId { get; set; }
    //    public string Name { get; set; }
    //    public EntityReference Question { get; set; }
    //    public bool IsAnswer { get; set; }
    //}

    public class Answer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference User { get; set; }
        public EntityReference Question { get; set; }
        public EntityReference Choice { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference Participator { get; set; }
        public int Point { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsTrust { get; set; }
        public bool IsTimeOverlap { get; set; }
        public bool IsRefreshOrBack { get; set; }
    }

    public class Language
    {
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }

    public class AttachmentFile
    {
        public Guid AttachmentFileId { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Data { get; set; }
        public string IconFile { get; set; }
        public string FilePath { get; set; }
    }

    public class Annotation
    {
        public Guid AnnotationId { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference AttachmentFile { get; set; }
        public EntityReference Object { get; set; }
        public string Subject { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Text { get; set; }
        public string FilePath { get; set; }

    }

    public class MultiLanguage
    {
        public Guid MultiLanguageId { get; set; }
        public string Name { get; set; }
        public EntityReference Language { get; set; }
        public string Code { get; set; }
    }

    [DataContract]
    public class MsCrmResult
    {
        [DataMember]
        public Guid CrmId { get; set; }
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Result { get; set; }
        [DataMember]
        public bool HasException { get; set; }
    }

    [DataContract]
    public class MsCrmResultObj<T>
    {
        [DataMember]
        public Guid CrmId { get; set; }
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Result { get; set; }
        [DataMember]
        public bool HasException { get; set; }
        [DataMember]
        public T ReturnObject { get; set; }
    }

    public class StringMap
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

    public class UserCubeStatus
    {
        public EntityReference UserId { get; set; }
        public int Point { get; set; }
        public int Rank { get; set; }
    }

    [Serializable]
    public class MsCrmResultObject
    {
        public Guid CrmId { get; set; }
        public bool Success { get; set; }
        public string Result { get; set; }
        public string ExtraInformation { get; set; }
        public object ReturnObject { get; set; }
        public int RecordCount { get; set; }
    }

    #region | MESSAGE INFO |
    [CrmSchemaName("new_message")]
    public class MessageInfo
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_messageid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_portalid")]
        public EntityReferenceWrapper Portal { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_from")]
        public EntityReferenceWrapper FromId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_to")]
        public EntityReferenceWrapper ToId { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_content")]
        public string Content { get; set; }

        [CrmFieldDataType(CrmDataType.DATETIME)]
        public DateTime? CreatedOn
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;

                if (value != null)
                {
                    CreatedOnString = ((DateTime)value).ToString("dd.MM.yyyy HH:mm");
                }
            }
        }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("statuscode")]
        public OptionSetValueWrapper StatusCode { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        public string FromImageUrl { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        public string ToImageUrl { get; set; }

        public string CreatedOnString { get; set; }

        private DateTime? _createDate = null;

        public enum Status
        {
            NotSeen = 1,
            Seen = 100000000
        }
    }
    #endregion

    #region | QUESTION CLASSES |

    public class QuestionLevel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public string ImagePath { get; set; }
    }

    public class Question
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference QuestionLevel { get; set; }
        public EntityReference QuestionDefination { get; set; }
        public int Time { get; set; }
        public int Point { get; set; }
        public StringMap QuestionCategory { get; set; }
        public List<QuestionChoices> QuestionChoices { get; set; }
    }

    public class QuestionChoices
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Question { get; set; }
        public bool IsCorrect { get; set; }
    }
    #endregion

    #region | FRIENDSHIP CLASSES |
    public class Friendship
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference PartyOne { get; set; }
        public EntityReference PartyTwo { get; set; }
        public EntityReference FriendshipRequest { get; set; }
    }

    public class FriendshipRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference From { get; set; }
        public EntityReference To { get; set; }
    }


    public class UserFriends
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public int UserType { get; set; }
    }

    #endregion

    #region | FORUM CLASSES |
    public class Forum
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference ParentForum { get; set; }
        public List<Forum> SubForums { get; set; }
        public List<ForumSubject> ForumSubjects { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ForumSubject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Forum { get; set; }
        public EntityReference User { get; set; }
        public string PortalUserImage { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public List<Comment> CommentList { get; set; }
        public EntityReference Portal { get; set; }
        public LikeInfo LikeDetail { get; set; }
    }

    #endregion

    #region | SURVEY CLASSES |

    public class Survey
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<SurveyChoices> SurveyChoices { get; set; }
    }

    public class SurveyChoices
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Survey { get; set; }
    }

    public class SurveyAnswer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReference Portal { get; set; }
        public EntityReference PortalUser { get; set; }
        public EntityReference Survey { get; set; }
        public EntityReference SurveyChoice { get; set; }
    }

    #endregion

    #region | POINT CODE CLASSESES |
    [CrmSchemaName("new_usercodeusage")]
    public class UserCodeUsage
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_usercodeusageid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_portalid")]
        public EntityReferenceWrapper PortalId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_pointcodeid")]
        public EntityReferenceWrapper PointCodeId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_userid")]
        public EntityReferenceWrapper UserId { get; set; }

        [CrmFieldDataType(CrmDataType.INT)]
        [CrmFieldName("new_point")]
        public int? Point { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
    }

    public class PointCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReferenceWrapper PortalId { get; set; }
        public string GroupCode { get; set; }
        public string Code { get; set; }
        public int Point { get; set; }
        public OptionSetValueWrapper Status { get; set; }
    }
    #endregion

    #region | GIFT CLASSES |

    public class GiftCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReferenceWrapper PortalId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Gift
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityReferenceWrapper PortalId { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string GiftCode { get; set; }
        public int Stock { get; set; }
        public int Point { get; set; }
        public EntityReferenceWrapper CategoryId { get; set; }
    }

    [CrmSchemaName("new_giftrequest")]
    public class UserGiftRequest
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_giftrequestid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_potalid")]
        public EntityReferenceWrapper PortalId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_giftid")]
        public EntityReferenceWrapper GiftId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_userid")]
        public EntityReferenceWrapper UserId { get; set; }

        [CrmFieldDataType(CrmDataType.INT)]
        [CrmFieldName("new_point")]
        public int? Point { get; set; }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("statuscode")]
        public OptionSetValueWrapper Status { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_errordesc")]
        public string ErrorDesc { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_ordercode")]
        public string OrderCode { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
    }

    #endregion

    #region | DISCOVERY FORM |
    [CrmSchemaName("new_discoveryform")]
    public class DiscoveryForm
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_discoveryformid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_firstname")]
        public string FirstName { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_lastname")]
        public string LastName { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_phonenumber")]
        public string PhoneNumber { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_email")]
        public string Email { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_cityid")]
        public EntityReferenceWrapper CityId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_townid")]
        public EntityReferenceWrapper TownId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_portalid")]
        public EntityReferenceWrapper PortalId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_userid")]
        public EntityReferenceWrapper UserId { get; set; }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("new_hometype")]
        public OptionSetValueWrapper HomeType { get; set; }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("new_informedby")]
        public OptionSetValueWrapper InformedBy { get; set; }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("new_visithour")]
        public OptionSetValueWrapper VisitHour { get; set; }

        [CrmFieldDataType(CrmDataType.DATETIME)]
        [CrmFieldName("new_visitdate")]
        public DateTime? VisitDate { get; set; }

        [CrmFieldDataType(CrmDataType.INT)]
        [CrmFieldName("new_formcode")]
        public int? FormCode { get; set; }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("statuscode")]
        public OptionSetValueWrapper Status { get; set; }

        public DateTime? CreatedOn
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;

                if (value != null)
                {
                    CreatedOnString = ((DateTime)value).ToString("dd.MM.yyyy HH:mm");
                }
            }
        }

        private DateTime? _createDate = null;

        public string CreatedOnString { get; set; }

    }
    #endregion

    #region | LOCATION CLASSES |

    [DataContract]
    [CrmSchemaName("new_city")]
    public class City
    {
        [DataMember]
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_cityid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_citycode")]
        public string Code { get; set; }

    }

    [DataContract]
    [CrmSchemaName("new_town")]
    public class Town
    {
        [DataMember]
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_townid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_towncode")]
        public string Code { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_cityid")]
        public EntityReferenceWrapper CityId { get; set; }

    }
    #endregion

    #region | EMOS CLASSESE |

    [DataContract]
    public class AssemblerInfo
    {
        [DataMember]
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        public string FirstName { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        public string LastName { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        public string CompanyName { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        public string MobilePhoneNumber { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        public string WorkPhoneNumber { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        public EntityReferenceWrapper CityId { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        public EntityReferenceWrapper TownId { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        public string AddressDetail { get; set; }

    }

    [DataContract]
    [Serializable]
    [CrmSchemaName("new_assemblyrequest")]
    public class AssemblyRequestInfo
    {
        [DataMember]
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_assemblyrequestid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_firstname")]
        public string FirstName { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_lastname")]
        public string LastName { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_mobilephone")]
        public string MobilePhoneNumber { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_emailaddress")]
        public string EmailAddress { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_workphone")]
        public string WorkPhoneNumber { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_cityid")]
        public EntityReferenceWrapper CityId { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_townid")]
        public EntityReferenceWrapper TownId { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_addressdetail")]
        public string AddressDetail { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_userid")]
        public EntityReferenceWrapper AssemblerId { get; set; }

        [DataMember]
        public Guid AssemblerUserId
        {
            get { return AssemblerId == null ? Guid.Empty : AssemblerId.Id; }
            set
            {
                AssemblerId = new EntityReferenceWrapper();
                AssemblerId.LogicalName = "new_user";
                AssemblerId.Id = value;
            }
        }

        [DataMember]
        public Guid RequestTownId
        {
            get { return TownId == null ? Guid.Empty : TownId.Id; }
            set
            {
                TownId = new EntityReferenceWrapper();
                TownId.LogicalName = "new_town";
                TownId.Id = value;
            }
        }

        [DataMember]
        public Guid RequestCityId
        {
            get { return CityId == null ? Guid.Empty : CityId.Id; }
            set
            {
                CityId = new EntityReferenceWrapper();
                CityId.LogicalName = "new_city";
                CityId.Id = value;
            }

        }

        [DataMember]
        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("statuscode")]
        public OptionSetValueWrapper StatusCode { get; set; }

        [DataMember]
        [CrmFieldDataType(CrmDataType.DATETIME)]
        public DateTime? CreatedOn
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;

                if (value != null)
                {
                    CreatedOnString = ((DateTime)value).ToLocalTime().ToString("dd.MM.yyyy HH:mm");
                }
            }
        }

        [DataMember]
        public string CreatedOnString { get; set; }
        private DateTime? _createDate = null;
    }

    #endregion

    [DataContract]
    public class EntityReferenceWrapper
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        public string LogicalName { get; set; }
    }

    [DataContract]
    public class OptionSetValueWrapper
    {
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public int? AttributeValue { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CrmFieldName : Attribute
    {
        private string _fieldName;
        public CrmFieldName(string fieldName)
        {
            this._fieldName = fieldName;
        }

        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CrmFieldDataType : Attribute
    {
        private CrmDataType _dataType;
        public CrmFieldDataType(CrmDataType dataType)
        {
            this._dataType = dataType;
        }

        public CrmDataType CrmDataType
        {
            get
            {
                return _dataType;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CrmSchemaName : Attribute
    {
        private string _schemaName;
        public CrmSchemaName(string schemaName)
        {
            this._schemaName = schemaName;
        }

        public string SchemaName
        {
            get
            {
                return _schemaName;
            }
        }
    }

    public enum CrmDataType
    {
        UNIQUEIDENTIFIER,
        STRING,
        INT,
        DATETIME,
        ENTITYREFERENCE,
        OPTIONSETVALUE,
        MONEY,
        DECIMAL
    }
}
