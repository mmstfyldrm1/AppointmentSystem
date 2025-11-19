using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {

        Task Add(T t);
        Task Delete(T t);
        Task Update(T t);
        Task<List<T>> GetList();
        Task<T> GetById(int id);
    }
}
