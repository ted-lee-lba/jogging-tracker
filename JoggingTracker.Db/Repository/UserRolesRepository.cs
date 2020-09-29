using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository {
    public class UserRolesRepository : BaseRepository<UserRole>, IUserRolesRepository {
        public UserRolesRepository(JT_DBEntities dbContext) : base(dbContext) { }

        public IQueryable<UserRole> getAllRoles(params string[] ExcludeRolesName) {
            return _dbContext.UserRoles.Where(c => !ExcludeRolesName.Contains(c.RolesName));
        }
    }
}
