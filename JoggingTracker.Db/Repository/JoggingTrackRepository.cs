using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository {
    public class JoggingTrackRepository : BaseRepository<JoggingTrack>, IJoggingTrackRepository {
        public JoggingTrackRepository(JT_DBEntities dbContext) : base(dbContext) { }

        public IQueryable<JoggingTrack> getAll() {
            return this._dbContext.JoggingTracks;
        }

        public IQueryable<JoggingTrack> getAll(string[] ExcludeRoles) {
            return (from a in this._dbContext.JoggingTracks
                    join b in this._dbContext.Users on a.Users_Id equals b.Users_Id
                    join c in this._dbContext.UserRoles on b.UserRoles_Id equals c.UserRoles_Id
                    where !ExcludeRoles.Contains(c.RolesName)
                    select a);
        }

        public IQueryable<JoggingTrack> getByUserId(int? id) {
            return this._dbContext.JoggingTracks.Where(c => c.Users_Id == id || id == null || id == 0);
        }

        public IQueryable<JoggingTrack> getByFilter(int Id, DateTime FromDate, DateTime ToDate) {
            return this._dbContext.JoggingTracks.Where(c => c.Users_Id == Id && c.FromDateTime >= FromDate && c.ToDateTime <= ToDate);
        }
    }
}
