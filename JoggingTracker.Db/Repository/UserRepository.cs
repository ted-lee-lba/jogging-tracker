using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository {
    public class UserRepository : BaseRepository<User>, IUserRepository {
        public UserRepository(JT_DBEntities dbContext) : base(dbContext) { }

        public User getById(int Id) {
            return this._dbContext.Users.Where(c => c.Users_Id == Id).FirstOrDefault();
        }

        public User getByUserName(string UserName) {
            return this._dbContext.Users.Where(c => c.UserName == UserName).FirstOrDefault();
        }

        public IQueryable<User> getAllUser(string ExcludeUserName) {
            return this._dbContext.Users.Where(c => c.UserName != ExcludeUserName);
        }

        public IQueryable<User> getAllUser(string ExcludeUserName, string[] ExcludeRoles) {
            return (from a in this._dbContext.Users
                    join b in this._dbContext.UserRoles on a.UserRoles_Id equals b.UserRoles_Id
                    where a.UserName != ExcludeUserName && !ExcludeRoles.Contains(b.RolesName)
                    select a);
        }

        public IQueryable<User> getAllUser(string[] ExcludeRoles) {
            return (from a in this._dbContext.Users
                    join b in this._dbContext.UserRoles on a.UserRoles_Id equals b.UserRoles_Id
                    where !ExcludeRoles.Contains(b.RolesName)
                    select a);
        }

        public User getByUserName(string UserName, int NotUsers_Id) {
            return this._dbContext.Users.Where(c => c.UserName == UserName && c.Users_Id != NotUsers_Id).FirstOrDefault();

        }
    }
}
