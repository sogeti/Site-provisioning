using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface IService<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includes);
        TEntity Get(int id);
        TEntity Get(string id);
        TEntity Get(Func<TEntity, bool> predicate);
        TEntity Get(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includes);
        void Insert(TEntity entity);
        void Delete(TEntity entity);
        void Delete(Func<TEntity, Boolean> predicate);
        void Update(TEntity entity);
        void Save();
    }
}
