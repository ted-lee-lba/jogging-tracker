using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository {
    public abstract class BaseRepository<T> : IRepository<T> where T : class {
        internal JT_DBEntities _dbContext;
        public BaseRepository(JT_DBEntities dbContext) {
            if (dbContext == null) {
                throw new ArgumentNullException("dbContext should not be null");
            }
            this._dbContext = dbContext;
        }

        public void Delete(T obj) {
            var dbSet = this._dbContext.Set<T>();
            dbSet.Remove(obj);
        }

        public void Delete(int Id) {
            var dbSet = this._dbContext.Set<T>();
            var entity = dbSet.Find(Id);
            if (entity == null)
                return;

            dbSet.Remove(entity);
        }

        public T Get(int Id) {
            return this._dbContext.Set<T>().Find(Id);
        }

        public virtual void Save(T obj) {
            var dbSet = this._dbContext.Set<T>();
            dbSet.Add(obj);
        }

        public virtual void Update(T obj) {
            this._dbContext.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
