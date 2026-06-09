using SchoolManagement.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    // A simple generic CRUD service for entities with IRepository<T>
    public class GenericCrudService<T> where T : class
    {
        private readonly IRepository<T> _repo;

        public GenericCrudService(IRepository<T> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _repo.ListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(T entity) => await _repo.AddAsync(entity);

        public void Update(T entity) => _repo.Update(entity);

        public void Delete(T entity) => _repo.Delete(entity);
    }
}
