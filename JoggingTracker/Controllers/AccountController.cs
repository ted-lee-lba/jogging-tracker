using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using JoggingTracker.Models;
using JoggingTracker.Db;

namespace JoggingTracker.Controllers {
    public class AccountController : ApiController {
        private IUnitOfWork _unitOFWork;
        private ISimpleUserManager _simpleUserManager;

        public AccountController(IUnitOfWork unitOfWork, ISimpleUserManager userManager) {
            this._simpleUserManager = userManager;
            this._unitOFWork = unitOfWork;
        }

        [HttpGet]
        [Route("api/Login")]
        public IHttpActionResult Get(string UserName, string Password) {
            var signInStatus = _simpleUserManager.PasswordSignInAsync(UserName, Password, true).Result;
            if (signInStatus != Microsoft.AspNet.Identity.Owin.SignInStatus.Success) {
                return Json(new {
                    IsValidUser = false,
                    IsLoggedIn = false,
                    UserName = ""
                });
            }
            return Json(new {
                UserName = UserName,
                IsValidUser = true,
                IsLoggedIn = true
            });
        }

        [HttpGet]
        [Route("api/IsLoggedIn")]
        public IHttpActionResult IsLoggedIn() {
            if (User.Identity.IsAuthenticated) {
                return Json(new {
                    IsLoggedIn = true,
                    UserName = User.Identity.GetUserName(),
                });
            } else {
                return Json(new {
                    IsLoggedIn = false,
                    USerName = "",
                });
            }
        }

        [HttpPost]
        [Route("api/Logoff")]
        public IHttpActionResult Logoff() {
            this._simpleUserManager.SignOut();
            return Json(new {
                IsLoggedIn = false,
                USerName = "",
            });
        }

        [Authorize]
        [HttpPost]
        [Route("api/ChangePass")]
        public IHttpActionResult ChangePass(ChangePasswordModel model) {
            var user = this._unitOFWork.UserRepository.getByUserName(User.Identity.GetUserName());
            if (user == null) {
                return Json(new {
                    ChangeSuccess = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }

            if (model.NewPassword != model.ConfirmPassword) {
                return Json(new {
                    ChangeSuccess = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.PASSWORD_NOT_MATCH
                    }
                });
            }

            var hashPassword = this._simpleUserManager.HashPassword(model.CurrentPassword);
            if (!hashPassword.Equals(user.Password)) {
                return Json(new {
                    ChangeSuccess = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.INVALID_CURRENT_PASSWORD
                    }
                });
            }

            user.Password = this._simpleUserManager.HashPassword(model.NewPassword);
            user.UpdatedBy = User.Identity.GetUserName();
            user.UpdatedDate = DateTime.Now;
            this._unitOFWork.UserRepository.Update(user);
            this._unitOFWork.SaveChanges();
            return Json(new {
                ChangeSuccess = true
            });
        }
    }
}