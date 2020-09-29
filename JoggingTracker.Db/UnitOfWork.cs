using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using JoggingTracker.Db.Repository;

namespace JoggingTracker.Db {
    public class UnitOfWork : IDisposable, IUnitOfWork {
        private JT_DBEntities dbContext = new JT_DBEntities();
        private IList<object> _lstRepositories = new List<object>();

        public IUserRepository UserRepository {
            get {
                bool bIsExist = this._lstRepositories.Where(c => c.GetType() == typeof(UserRepository)).Any();
                if (!bIsExist) {
                    this._lstRepositories.Add(new UserRepository(dbContext));
                }
                return (IUserRepository)this._lstRepositories.Where(c => c.GetType() == typeof(UserRepository)).FirstOrDefault();
            }
        }

        public IUserRolesRepository UserRolesRepository {
            get {
                bool bIsExist = this._lstRepositories.Where(c => c.GetType() == typeof(UserRolesRepository)).Any();
                if (!bIsExist) {
                    this._lstRepositories.Add(new UserRolesRepository(dbContext));
                }
                return (IUserRolesRepository)this._lstRepositories.Where(c => c.GetType() == typeof(UserRolesRepository)).FirstOrDefault();
            }
        }

        public IJoggingTrackRepository JoggingTrackRepository {
            get {
                bool bIsExist = this._lstRepositories.Where(c => c.GetType() == typeof(JoggingTrackRepository)).Any();
                if (!bIsExist) {
                    this._lstRepositories.Add(new JoggingTrackRepository(dbContext));
                }
                return (IJoggingTrackRepository)this._lstRepositories.Where(c => c.GetType() == typeof(JoggingTrackRepository)).FirstOrDefault();
            }
        }

        public void SaveChanges() {
            using (var tsScope = new TransactionScope(TransactionScopeOption.RequiresNew)) {
                this.dbContext.SaveChanges();
                tsScope.Complete();
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    this.dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
