using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ApplicationRoleManager : IApplicationRoleService
    {
        public Task Add(Dt_ApplicationRole t)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Dt_ApplicationRole t)
        {
            throw new NotImplementedException();
        }

        public Task<Dt_ApplicationRole> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Dt_ApplicationRole>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task Update(Dt_ApplicationRole t)
        {
            throw new NotImplementedException();
        }
    }
}
