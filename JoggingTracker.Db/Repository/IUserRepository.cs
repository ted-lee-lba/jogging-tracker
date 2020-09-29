namespace JoggingTracker.Db.Repository {
    using System.Linq;

    public interface IUserRepository : IRepository<User> {
        User getByUserName(string UserName);

        User getById(int Id);

        User getByUserName(string UserName, int NotUsers_Id);

        IQueryable<User> getAllUser(string ExcludeUserName);

        IQueryable<User> getAllUser(string ExcludeUserName, string[] ExcludeRoles);

        IQueryable<User> getAllUser(string[] ExcludeRoles);
    }
}