using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoggingTracker.Models
{
    public class LoginModel {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginReponseModel {
        public bool IsValidUser { get; set; }

        public string UserName { get; set; }
    }
}
