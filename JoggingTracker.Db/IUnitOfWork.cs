using JoggingTracker.Db.Repository;

namespace JoggingTracker.Db {
    public interface IUnitOfWork {
        IUserRepository UserRepository { get; }

        IUserRolesRepository UserRolesRepository { get; }

        IJoggingTrackRepository JoggingTrackRepository { get; }

        void Dispose();
        void SaveChanges();
    }
}