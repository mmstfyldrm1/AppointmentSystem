using BusinessLayer.Abstract;
using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ApplicationUserManager : IApplicationUserService
    {
        private readonly IApplicationUserRepository _repository;

        public ApplicationUserManager(IApplicationUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(Dt_ApplicationUser t)
        {
            await _repository.TAdd(t);  
        }

        public async Task Delete(Dt_ApplicationUser t)
        {
            await _repository.TDelete(t);
        }

        public async Task<Dt_ApplicationUser> GetById(int id)
        {
            return await _repository.TGetById(id);
        }

        public async Task<List<Dt_ApplicationUser>> GetList()
        {
            return await _repository.TGetAll();
        }

        public async Task Update(Dt_ApplicationUser t)
        {
            await _repository.TUpdate(t);
        }
    }
}
