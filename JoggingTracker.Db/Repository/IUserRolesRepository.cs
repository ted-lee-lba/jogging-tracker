using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository {
    public interface IUserRolesRepository : IRepository<UserRole> {
        IQueryable<UserRole> getAllRoles(params string[] ExcludeRolesName);
    }
}
