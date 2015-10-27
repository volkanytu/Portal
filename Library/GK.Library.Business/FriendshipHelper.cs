using GK.Library.Utility;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class FriendshipHelper
    {
        public static MsCrmResult CreateFriendshipRequest(FriendshipRequest request, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_friendshiprequest");

                if (request.From != null && request.From.Id != Guid.Empty)
                {
                    ent["new_fromuserid"] = request.From;
                }

                if (request.To != null && request.To.Id != Guid.Empty)
                {
                    ent["new_touserid"] = request.To;
                }

                if (request.Portal != null && request.Portal.Id != Guid.Empty)
                {
                    ent["new_portalid"] = request.Portal;
                }

                ent["new_name"] = request.From.Name + "-" + request.To.Name;

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "M038"; //"Arkadaşlık talebiniz oluşturulmuştur.";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetFriendshipRequestInfo(Guid requestId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                fr.new_friendshiprequestId AS Id
	                                ,fr.new_name AS Name
	                                ,fr. new_fromuserId AS FromId
	                                ,new_fromuserIdName AS fromIdName
	                                ,fr.new_touserId AS ToId
	                                ,fr.new_touserIdName AS ToIdName
	                                ,fr.new_portalId AS PortalId
	                                ,fr.new_portalIdName AS PortalIdName
                                FROM
	                                new_friendshiprequest AS fr (NOLOCK)
                                WHERE
	                                fr.new_friendshiprequestId='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, requestId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    FriendshipRequest _request = new FriendshipRequest();

                    _request.Id = (Guid)dt.Rows[0]["Id"];
                    if (dt.Rows[0]["Name"] != DBNull.Value) { _request.Name = dt.Rows[0]["Name"].ToString(); }

                    if (dt.Rows[0]["PortalId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["PortalId"],
                            Name = dt.Rows[0]["PortalIdName"].ToString(),
                            LogicalName = "new_portal"
                        };

                        _request.Portal = er;
                    }

                    if (dt.Rows[0]["FromId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["FromId"],
                            Name = dt.Rows[0]["FromIdName"].ToString(),
                            LogicalName = "new_user"
                        };

                        _request.From = er;
                    }

                    if (dt.Rows[0]["ToId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["ToId"],
                            Name = dt.Rows[0]["ToIdName"].ToString(),
                            LogicalName = "new_user"
                        };

                        _request.To = er;
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = _request;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M042"; //"Arkadaşlık isteğine ait bir bilgi bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult CreateFriendship(Friendship friendship, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_friendship");

                ent["new_name"] = friendship.PartyOne.Name + "-" + friendship.PartyTwo.Name;

                if (friendship.PartyOne != null && friendship.PartyOne.Id != Guid.Empty)
                {
                    ent["new_partyoneid"] = friendship.PartyOne;
                }

                if (friendship.PartyTwo != null && friendship.PartyTwo.Id != Guid.Empty)
                {
                    ent["new_partytwoid"] = friendship.PartyTwo;
                }

                if (friendship.Portal != null && friendship.Portal.Id != Guid.Empty)
                {
                    ent["new_portalid"] = friendship.Portal;
                }

                if (friendship.FriendshipRequest != null && friendship.FriendshipRequest.Id != Guid.Empty)
                {
                    ent["new_friendshiprequestid"] = friendship.FriendshipRequest;
                }

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "M043"; //"Arkadaşlığınız başlamıştır.";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CloseFriendshipRequest(Guid requestId, FriendshipRequestStatus statusCode, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                SetStateRequest setStateReq = new SetStateRequest();
                setStateReq.EntityMoniker = new EntityReference("new_friendshiprequest", requestId);
                setStateReq.State = new OptionSetValue(1);
                setStateReq.Status = new OptionSetValue((int)statusCode);

                SetStateResponse response = (SetStateResponse)service.Execute(setStateReq);

                returnValue.Success = true;
                returnValue.Result = "M041"; //"Arkadaşlık talebiniz durumu güncellendi.";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CloseFriendship(Guid friendshipId, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                SetStateRequest setStateReq = new SetStateRequest();
                setStateReq.EntityMoniker = new EntityReference("new_friendship", friendshipId);
                setStateReq.State = new OptionSetValue(1);
                setStateReq.Status = new OptionSetValue(2);

                SetStateResponse response = (SetStateResponse)service.Execute(setStateReq);

                returnValue.Success = true;
                returnValue.Result = "M044"; //"Arkadaşlığınız iptal edilmiştir.";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetUserFriendList(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"--ARKADAŞLARIM
                                    SELECT
	                                    u.new_userId AS UserId
	                                    ,u.new_name AS UserName
                                        ,u.new_imageurl AS ImageUrl
	                                    ,c.FullName
	                                    ,A.UserType
                                        ,c.JobTitle
                                    FROM
                                    (
	                                    SELECT
		                                    CASE
			                                    WHEN
				                                    f.new_partyoneId!='{1}'
			                                    THEN
				                                    f.new_partyoneId
			                                    ELSE
				                                    f.new_partytwoId
		                                    END  AS UserId
		                                    ,1 AS UserType
	                                    FROM
		                                    new_friendship AS f (NOLOCK)
	                                    WHERE
		                                    f.new_portalId='{0}'
	                                    AND
		                                    f.statecode=0
	                                    AND
	                                    (
		                                    f.new_partyoneId='{1}' OR f.new_partytwoId='{1}'
	                                    )

	                                    UNION ALL

	                                    SELECT
		                                    r.new_touserId AS UserId
		                                    ,2 AS UserType
	                                    FROM
		                                    new_friendshiprequest AS r (NOLOCK)
	                                    WHERE
		                                    r.new_portalId='{0}'
	                                    AND
		                                    r.statecode=0
	                                    AND
		                                    r.new_fromuserId='{1}'

	                                    UNION ALL

	                                    SELECT
		                                    r.new_fromuserId AS UserId
		                                    ,3 AS UserType
	                                    FROM
		                                    new_friendshiprequest AS r (NOLOCK)
	                                    WHERE
		                                    r.new_portalId='{0}'
	                                    AND
		                                    r.statecode=0
	                                    AND
		                                    r.new_touserId='{1}'


                                    ) AS A
	                                    JOIN
		                                    new_user AS u (NOLOCK)
			                                    ON
			                                    u.new_userId=A.UserId
			                                    AND
			                                    u.statecode=0
			                                    AND
			                                    u.statuscode=1 --Active
	                                    JOIN
		                                    Contact AS c (NOLOCK)
			                                    ON
			                                    c.ContactId=u.new_contactId";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<UserFriends> lstUser = new List<UserFriends>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        UserFriends uf = new UserFriends()
                        {
                            UserId = (Guid)dt.Rows[i]["UserId"],
                            UserName = dt.Rows[i]["UserName"].ToString(),
                            FullName = dt.Rows[i]["FullName"].ToString(),
                            UserType = Convert.ToInt32(dt.Rows[i]["UserType"]),
                            ImageUrl = dt.Rows[i]["ImageUrl"] != DBNull.Value ? dt.Rows[i]["ImageUrl"].ToString() : "nouserprofile.jpg",
                            JobTitle = dt.Rows[i]["JobTitle"] != DBNull.Value ? dt.Rows[i]["JobTitle"].ToString() : "---"
                        };

                        lstUser.Add(uf);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = lstUser;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M045"; //"Harhangi bir arkadaşlık işleminiz yok!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject HasUserRequestWithYou(Guid portalId, Guid portalUserId, Guid selectedUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                fr.new_friendshiprequestId AS Id
                                FROM
	                                new_friendshiprequest AS fr (NOLOCK)
                                WHERE
	                                fr.new_portalId='{0}'
	                                AND
	                                fr.statecode=0
	                                AND
                                (
	                                (fr.new_fromuserId='{1}' AND fr.new_touserId='{2}')
	                                OR
	                                (fr.new_fromuserId='{2}' AND fr.new_touserId='{1}')
                                )";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId, selectedUserId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    returnValue = FriendshipHelper.GetFriendshipRequestInfo((Guid)dt.Rows[0]["Id"], sda);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M040"; //"Aranızda herhangi bir arkadaşlık isteği yoktur!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CheckIsUserYourFriend(Guid portalId, Guid portalUserId, Guid selectedUserId, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                f.new_friendshipId AS Id
                                FROM
	                                new_friendship AS f (NOLOCK)
                                WHERE
	                                f.new_portalId='{0}'
                                AND
	                                f.statecode=0
                                AND
                                (
	                                (f.new_partyoneId='{1}' AND f.new_partytwoId='{2}')
                                OR
	                                (f.new_partyoneId='{2}' AND f.new_partytwoId='{1}')
                                )";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId, selectedUserId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    returnValue.Success = true;
                    returnValue.CrmId = (Guid)dt.Rows[0]["Id"];
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M039"; //"Kullanıcı arkadaşınız değildir!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static List<SelectValue> SearchFriend(string portalId, string portalUserId, string searchKey, SqlDataAccess sda)
        {
            List<SelectValue> selectValues = new List<SelectValue>();

            try
            {
                string str = @"SELECT
	                                *
                                FROM
                                (
	                                SELECT
		                                CASE WHEN f.new_partyoneId!='{1}' THEN f.new_partyoneId ELSE f.new_partytwoId END  AS UserId
		                                ,CASE WHEN f.new_partyoneId!='{1}' THEN f.new_partyoneIdName ELSE f.new_partytwoIdName END AS UserIdName
	                                FROM
		                                new_friendship AS f (NOLOCK)
	                                WHERE
		                                f.new_portalId='{0}'
	                                AND
		                                f.statecode=0
	                                AND
		                                (f.new_partyoneId='{1}' OR f.new_partytwoId='{1}')
                                ) AS A
                                WHERE
	                                A.UserIdName LIKE '%{2}%'";

                DataTable dataTable = sda.getDataTable(string.Format(str, portalId, portalUserId, searchKey));

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SelectValue selectValue = new SelectValue()
                        {
                            text = dataTable.Rows[i]["UserIdName"].ToString(),
                            @value = dataTable.Rows[i]["UserId"].ToString()
                        };
                        selectValues.Add(selectValue);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return selectValues;
        }
    }
}
