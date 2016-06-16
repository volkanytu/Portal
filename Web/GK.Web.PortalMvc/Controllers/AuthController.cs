using GK.Library.Entities.CrmEntities;
using GK.Web.PortalMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GK.Web.PortalMvc.Controllers
{
    public class AuthController : Controller
    {
        public AuthController()
        {

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User userData, string returnUrl)
        {
            SessionData sessionMock = new SessionData()
            {

            };

            if (true)
            {
                LoggedInUser.Current = sessionMock;

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.ErrorMessage = "HATA!";
            }

            return View();
        }

        public ActionResult Logout()
        {
            LoggedInUser.Current = null;

            return RedirectToAction("login");
        }
    }
}