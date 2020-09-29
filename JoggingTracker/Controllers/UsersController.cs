using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using JoggingTracker.Models;
using JoggingTracker.Db;
using JoggingTracker.Db.Repository;

namespace JoggingTracker.Controllers {
    [Authorize]
    public class UsersController : ApiController {
        private IUnitOfWork _unitOfWork;
        private ISimpleUserManager _simpleUserManager;

        public UsersController(IUnitOfWork unitOfWork, ISimpleUserManager userManager) {
            this._unitOfWork = unitOfWork;
            this._simpleUserManager = userManager;
            this.InitAutoMap();
        }

        [HttpGet]
        [Authorize(Roles = "ADMINISTRATOR,MANAGER")]
        public IHttpActionResult Get() {
            var rolesList = this.getExcludeRole();

            var userList = this._unitOfWork.UserRepository.getAllUser(User.Identity.GetUserName(), rolesList.ToArray()).
                ProjectTo<UserModel>().ToList();
            return Json(new {
                DataList = userList,
                Count = userList.Count
            });
        }

        [HttpGet]
        [Route("api/getuser")]
        [Authorize(Roles = "ADMINISTRATOR,MANAGER")]
        public IHttpActionResult Get(int id) {
            var user = Mapper.Map<User, UserModel>(this._unitOfWork.UserRepository.getById(id));
            return Json(new {
                user = user
            });
        }

        [HttpGet]
        [Route("api/getmeandalluser")]
        public IHttpActionResult GetMeAndAll() {
            var rolesList = this.getExcludeRole();

            var userList = new List<UserModel>();

            if (User.IsInRole("USER")) {
                userList.Add(Mapper.Map<User, UserModel>(
                    this._unitOfWork.UserRepository.getById(User.Identity.GetUserId<int>()))
                    );

            } else {
                userList = this._unitOfWork.UserRepository.getAllUser(rolesList.ToArray()).ProjectTo<UserModel>().ToList();
                var userModel = userList.Where(c => c.Users_Id == User.Identity.GetUserId<int>()).FirstOrDefault();
                if (userModel == null) {
                    var user = this._unitOfWork.UserRepository.getById(User.Identity.GetUserId<int>());
                    userModel = Mapper.Map<User, UserModel>(user);
                } else {
                    userList.Remove(userModel);
                }
                userList.Insert(0, userModel);
                userModel.FirstName = userModel.LastName = "";
                userModel.FirstName = JoggingTracker.Localization.Label.ME;

            }
            return Json(new {
                userList = userList
            });
        }

        [HttpPost]
        [Route("api/saveuser")]
        [Authorize(Roles = "ADMINISTRATOR,MANAGER")]
        public IHttpActionResult Update(UserModel model) {
            if (model.Users_Id == 0) {
                if (this._unitOfWork.UserRepository.getByUserName(model.UserName) != null) {
                    return Json(new {
                        Save = true,
                        Message = new string[] {
                            "User name already exist !"
                        }
                    });
                }
            }

            if (model.Users_Id != 0 && !string.IsNullOrEmpty(model.Password)) {
                if (model.Password != model.ConfirmPassword) {
                    return Json(new {
                        Save = true,
                        Message = new string[] {
                            "Password not match !"
                        }
                    });
                }
            }
            
            if (this._unitOfWork.UserRepository.getByUserName(model.UserName, model.Users_Id) != null) {
                return Json(new {
                    Save = true,
                    Message = new string[] {
                        "User name already exist !"
                    }
                });
            }

            var user = Mapper.Map<UserModel, User>(model);
            if (user.Users_Id == 0 || (user.Users_Id != 0 && !string.IsNullOrEmpty(user.Password))) {
                user.Password = this._simpleUserManager.HashPassword(user.Password);
            }

            if (user.Users_Id == 0) {
                user.CreatedBy = User.Identity.GetUserName();
                user.CreatedDate = DateTime.Now;
                user.UpdatedBy = User.Identity.GetUserName();
                user.UpdatedDate = DateTime.Now;
                this._unitOfWork.UserRepository.Save(user);

            } else {
                user.UpdatedBy = User.Identity.GetUserName();
                user.UpdatedDate = DateTime.Now;
                this._unitOfWork.UserRepository.Update(user);

            }
            this._unitOfWork.SaveChanges();

            return Json(new { Save = true });
        }

        [HttpPost]
        [Route("api/deleteuser")]
        [Authorize(Roles = "ADMINISTRATOR,MANAGER")]
        public IHttpActionResult Delete(UserModel model) {

            var userToDelete = this._unitOfWork.UserRepository.getById(model.Users_Id);
            if (userToDelete == null) {
                return Json(new {
                    Delete = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }

            if (userToDelete.JoggingTracks.Any()) {
                return Json(new {
                    Delete = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.RECORD_REFERENCED_BY_OTHER_ELEMENT
                    }
                });
            }

            if (!(userToDelete.UserRole.RolesName == "MANAGER" && User.IsInRole("ADMINISTRATOR")) &&
                !(userToDelete.UserRole.RolesName == "USER")) {
                return Json(new {
                    Delete = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }

            var joggingTrack = Mapper.Map<UserModel, User>(model);
            this._unitOfWork.UserRepository.Delete(joggingTrack.Users_Id);
            this._unitOfWork.SaveChanges();

            var rolesList = this.getExcludeRole();
            var userList = this._unitOfWork.UserRepository.getAllUser(User.Identity.GetUserName(), rolesList.ToArray()).
                ProjectTo<UserModel>().ToList();
            return Json(new {
                Delete = true,
                DataList = userList
            });
        }

        [HttpGet]
        [Route("api/getShowUserListFlag")]
        public IHttpActionResult GetShowUserListFlag() {
            return Json(new {
                ShowUserList = User.IsInRole("USER") ? 0 : 1
            });
        }

        private void InitAutoMap() {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<User, UserModel>().
                ForMember(dest => dest.Password, opt => opt.Ignore()).
                ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore()).
                ForMember(dest => dest.RolesName, opt => opt.MapFrom(src => src.UserRole.RolesName));
                cfg.CreateMap<UserModel, User>();
            });

        }

        private List<string> getExcludeRole() {
            var rolesList = new List<string>();
            if (User.IsInRole("MANAGER")) {
                rolesList.AddRange(new string[] { "ADMINISTRATOR", "MANAGER" });

            } 
            return rolesList;
        }
    }
}
