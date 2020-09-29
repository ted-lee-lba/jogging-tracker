using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository {
    public interface IRepository<T> where T: class {
        void Save(T obj);

        T Get(int Id);

        void Delete(int Id);

        void Delete(T obj);

        void Update(T obj);
    }
}
