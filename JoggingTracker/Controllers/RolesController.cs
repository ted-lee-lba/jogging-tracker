using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using JoggingTracker.Models;
using JoggingTracker.Db;
using JoggingTracker.Db.Repository;

namespace JoggingTracker.Controllers {
    [Authorize]
    public class RolesController : ApiController {
        private IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Roles = "ADMINISTRATOR,MANAGER")]
        public IHttpActionResult Get() {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<UserRole, UserRolesModel>();
            });

            var rolesList = new List<string>();
            if (User.IsInRole("MANAGER")) {
                rolesList.AddRange(new string[] { "ADMINISTRATOR" });

            } 

            var roleList = this._unitOfWork.UserRolesRepository.getAllRoles(rolesList.ToArray()).ProjectTo<UserRolesModel>().ToList();
            return Json(new {
                roles = roleList
            });
        }
    }
}
