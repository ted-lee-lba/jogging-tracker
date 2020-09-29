using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository {
    public interface IJoggingTrackRepository : IRepository<JoggingTrack> {
        IQueryable<JoggingTrack> getAll();

        IQueryable<JoggingTrack> getByUserId(int? id);

        IQueryable<JoggingTrack> getAll(string[] ExcludeRoles);

        IQueryable<JoggingTrack> getByFilter(int Id, DateTime FromDate, DateTime ToDate);
    }
}