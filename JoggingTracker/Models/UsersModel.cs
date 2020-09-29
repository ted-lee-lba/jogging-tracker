using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoggingTracker.Models {
    public class UsersGetResponseModel {

        public IList<UserModel> DataList { get; set; }
    }

    public class UserModel {
        public int Users_Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FullName {
            get {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public int? UserRoles_Id { get; set; }

        public Nullable<System.DateTime> DOB { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }

        public string RolesName { get; set; }
    }

    public class UserRolesModel {
        public int UserRoles_Id { get; set; }

        public string RolesName { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}