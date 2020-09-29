using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoggingTracker.Controllers {
    [Authorize]
    public class HomeController : Controller {
        [AllowAnonymous]
        public ActionResult Index() {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login() {
            return PartialView();
        }

        public ActionResult Users() {
            ViewBag.Message = "Your application description page.";

            return PartialView("UserList");
        }

        [Route("EditUser/{id}")]
        public ActionResult EditUser() {
            return PartialView("EditUser");
        }

        [Route("CreateUser")]
        public ActionResult CreateUser() {
            return PartialView("CreateUser");
        }

        [Route("ChangePassword")]
        public ActionResult ChangePassword() {
            return PartialView("ChangePass");
        }

        public ActionResult JoggingTrack() {
            ViewBag.Message = "JoggingTrack";

            return PartialView("JoggingTrackList");
        }

        [Route("EditJoggingTrack/{id}")]
        public ActionResult EditJoggingTrack() {
            return PartialView("EditJoggingTrack");
        }

        [Route("CreateJoggingTrack")]
        public ActionResult CreateJoggingTrack() {
            return PartialView("CreateJoggingTrack");
        }

        [Route("AccessDenied")]
        public ActionResult AccessDenied() {
            return PartialView("AccessDenied");
        }
    }
}