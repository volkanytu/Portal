using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Utility
{
    public enum NpsSurveyStatus
    {
        Created = 100000000,
        Sent,
        NotSent,
        Answered
    }

    public enum AssemblyRequestStatus
    {
        Waiting = 1,
        Completed = 100000000
    }

    public enum DiscoveryFormStatus
    {
        Waiting = 1,
        ServiceSent = 100000000,
        ServiceError = 100000001,
        LotusConfirmed = 100000002,
        CrmConfirmed = 100000003,
        Result_Negative = 100000004
    }

    public enum SteelDoorStatus
    {
        Waiting = 1,
        CrmConfirmed = 100000000,
        Result_Negative = 100000001
    }

    public enum GiftStatus
    {
        Requested = 1,
        Confirmed = 100000000,
        ServiceSent = 100000001,
        Completed = 2,
        Cancelled = 100000002,
        ServiceError = 100000003
    }

    public enum GsmOperators
    {
        Turkcell,
        Avea,
        Vodafone,
    }

    public enum ScoreType
    {
        Question = 100000000,
        EditEducation,
        EditArticle,
        EditVideo,
        Login,
        UpdateProfile,
        Survey,
        PostComment,
        GraffitiText,
        GraffitiMedia,
        ForumSubject,
        PointCode,
        UsePoint,
        KGSSales,
        SteelDoor
    }

    public enum ScorePeriod
    {
        Daily = 100000000,
        Weekly,
        Monthly,
        JustOne = 1
    }

    public enum PortalUserStatus
    {
        Active = 1,
        Pending = 100000000
    }

    public enum FriendshipRequestStatus
    {
        Active = 1,
        Accepted = 2,
        Denied = 100000000,
        Cancelled = 100000001
    }

    public enum PageNames
    {
        Welcome = 100000000,
        Contract,
        PlatformNedir,
        Manifesto
    }


}
