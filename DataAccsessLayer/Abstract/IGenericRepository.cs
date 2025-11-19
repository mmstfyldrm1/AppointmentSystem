using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Abstract
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> TGetAll();
        Task<T> TGetById(int id);
        Task TAdd(T entity);
        Task TUpdate(T entity);
        Task TDelete(T entity);
    }
}
