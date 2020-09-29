using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Globalization;
using Microsoft.AspNet.Identity;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using JoggingTracker.Models;
using JoggingTracker.Db;
using JoggingTracker.Db.Repository;

namespace JoggingTracker.Controllers {
    [Authorize]
    public class JoggingTrackController : ApiController {
        private IUnitOfWork _unitOfWork;
        private ISimpleUserManager _simpleUserManager;

        public JoggingTrackController(IUnitOfWork unitOfWork, ISimpleUserManager userManager) {
            this._unitOfWork = unitOfWork;
            this._simpleUserManager = userManager;
            InitAutomap();
        }

        [HttpGet]
        public IHttpActionResult Get(DateTime? FromDateTime, DateTime? ToDateTime, int Id = 0) {
            if (Id == 0 && !User.IsInRole("ADMINISTRATOR")) {
                return Json(new {
                    Failed = true,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }

            if (!this.RoleAgainstUserToShowValidation(User.Identity.GetUserId<int>(), Id)) {
                return Json(new {
                    Failed = true,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }
            var fromDate = FromDateTime ?? DateTime.Now.AddDays(-7);
            var toDate = ToDateTime ?? DateTime.Now;
            fromDate = fromDate.Subtract(fromDate.TimeOfDay);
            toDate = toDate.AddDays(1).Subtract(toDate.TimeOfDay).AddMinutes(-1);

            var trackList = this._unitOfWork.JoggingTrackRepository.getByFilter(Id, fromDate, toDate).ProjectTo<JoggingTrackModel>().ToList();
            var weeklyList = (from a in trackList
                              group a by a.YearWeekNumber into grp1
                              select new {
                                  DateRange = string.Format("{0:dd MMM} - {1:dd MMM}", 
                                    grp1.Min(c=>c.FromDateTime.StartOfWeek(DayOfWeek.Sunday)), 
                                    grp1.Max(c=>c.ToDateTime.EndOfWeek(DayOfWeek.Saturday))),
                                  TotalTime = string.Format("{0:dd} days, {0:hh} hours, {0:mm} minutes {0:ss} seconds", TimeSpan.FromTicks(grp1.Sum(c=>c.TimeUsed.Ticks))),
                                  TotalDistance = grp1.Sum(c=>c.Distance),
                                  AverageSpeed = (double)grp1.Sum(c=>c.Distance) / grp1.Sum(c=>c.TimeUsed.TotalMinutes)
                              }).ToList();
            return Json(new {
                DataList = trackList,
                WeeklyReport = weeklyList
            });
        }

        [HttpGet]
        [Route("api/getJoggingTrack")]
        public IHttpActionResult GetJoggingTrack(int id) {
            var joggingTrack = this._unitOfWork.JoggingTrackRepository.Get(id);
            if (!this.RoleAgainstUserToShowValidation(User.Identity.GetUserId<int>(), joggingTrack.Users_Id)) {
                return Json(new {
                    Failed = true,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }

            var joggingTrackModel = Mapper.Map<JoggingTrackModel>(joggingTrack);
            joggingTrack.FromDateTime = DateTime.SpecifyKind(joggingTrack.FromDateTime, DateTimeKind.Utc);
            joggingTrack.ToDateTime = DateTime.SpecifyKind(joggingTrack.ToDateTime, DateTimeKind.Utc);
            return Json(new {
                JoggingTrack = joggingTrackModel
            });
        }

        [HttpPost]
        [Route("api/savejoggingtrack")]
        public IHttpActionResult Update(JoggingTrackModel model) {

            if (model.FromDateTime == DateTime.MinValue || model.ToDateTime == DateTime.MinValue) {
                return Json(new {
                    Failed = true,
                    Message = new string[] {
                        "Invalid date time."
                    }
                });
            }


            if (!this.RoleAgainstUserToShowValidation(User.Identity.GetUserId<int>(), model.Users_Id)) {
                return Json(new {
                    Failed = true,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }

            if (model.FromDateTime > model.ToDateTime) {
                return Json(new {
                    Save = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.INVALID_DATE_RANGE
                    }
                });
            }

            if (model.Distance <= 0) {
                return Json(new {
                    Save = false,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.INVALID_DISTANCE
                    }
                });
            }

            var joggingTrack = Mapper.Map<JoggingTrackModel, JoggingTrack>(model);
            if (joggingTrack.FromDateTime.Kind == DateTimeKind.Utc) {
                joggingTrack.FromDateTime = joggingTrack.FromDateTime.ToLocalTime();
            }
            if (joggingTrack.ToDateTime.Kind == DateTimeKind.Utc) {
                joggingTrack.ToDateTime = joggingTrack.ToDateTime.ToLocalTime();
            }

            if (joggingTrack.JoggingTrack_Id == 0) {
                joggingTrack.CreatedBy = User.Identity.GetUserName();
                joggingTrack.CreatedDate = DateTime.Now;
                joggingTrack.UpdatedBy = User.Identity.GetUserName();
                joggingTrack.UpdatedDate = DateTime.Now;
                this._unitOfWork.JoggingTrackRepository.Save(joggingTrack);

            } else {
                joggingTrack.UpdatedBy = User.Identity.GetUserName();
                joggingTrack.UpdatedDate = DateTime.Now;
                this._unitOfWork.JoggingTrackRepository.Update(joggingTrack);

            }
            this._unitOfWork.SaveChanges();

            return Json(new { Save = true });
        }

        [HttpPost]
        [Route("api/deletejoggingtrack")]
        public IHttpActionResult Delete(JoggingTrackModel model) {

            if (!this.RoleAgainstUserToShowValidation(User.Identity.GetUserId<int>(), model.Users_Id)) {
                return Json(new {
                    Failed = true,
                    Message = new string[] {
                        JoggingTracker.Localization.Message.UNABLE_PROCESS_YOUR_REQUEST
                    }
                });
            }

            var joggingTrack = Mapper.Map<JoggingTrackModel, JoggingTrack>(model);
            this._unitOfWork.JoggingTrackRepository.Delete(joggingTrack.JoggingTrack_Id);
            this._unitOfWork.SaveChanges();

            var trackList = this._unitOfWork.JoggingTrackRepository.getByUserId(model.Users_Id).ProjectTo<JoggingTrackModel>().ToList();
            return Json(new {
                Delete = true,
                DataList = trackList
            });
        }

        private string GetUserRoleNameByUserId(int Users_Id) {
            return this._unitOfWork.UserRepository.getById(Users_Id).UserRole.RolesName;
        }

        private bool RoleAgainstUserToShowValidation(int RequestorId, int RequestToShowId) {
            var requestorRole = this.GetUserRoleNameByUserId(RequestorId);
            var requestToShowRole = this.GetUserRoleNameByUserId(RequestToShowId);

            return RoleAgainstActionToUserShowValidation(RequestorId, RequestToShowId, requestorRole, requestToShowRole);
        }

        private bool RoleAgainstActionToUserShowValidation(int RequestorId, int RequestToShowId, string requestorRole, string requestToShowRole) {
            if (requestorRole.Equals(requestorRole, StringComparison.OrdinalIgnoreCase) && 
                RequestorId.Equals(RequestToShowId)) {
                return true;
            }

            if (requestorRole.Equals("MANAGER", StringComparison.OrdinalIgnoreCase) &&
                requestToShowRole.Equals("USER")) {
                return true;
            }

            if (requestorRole.Equals("ADMINISTRATOR", StringComparison.OrdinalIgnoreCase) &&
                (requestToShowRole.Equals("MANAGER") || requestToShowRole.Equals("USER"))) {
                return true;
            }
            return false;
        }

        private void InitAutomap() {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<JoggingTrack, JoggingTrackModel>().ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(
                        src => src.User.UserName
                    )).ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(
                        src => src.User.FirstName
                    )).ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(
                        src => src.User.LastName
                    )).ForMember(
                    dest => dest.YearWeekNumber,
                    opt => opt.Ignore());
                cfg.CreateMap<JoggingTrackModel, JoggingTrack>();
            });

        }
    }
}