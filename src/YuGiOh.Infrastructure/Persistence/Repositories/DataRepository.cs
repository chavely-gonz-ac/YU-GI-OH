using YuGiOh.Domain.Repositories;
using YuGiOh.Infrastructure.Persistence;

using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ardalis.Specification.EntityFrameworkCore;

namespace YuGiOh.Infrastructure.Persistence.Repositories
{
    public class DataRepository<T> : RepositoryBase<T>, IRepositoryBase<T>, IReadRepositoryBase<T> where T : class
    {
        public DataRepository(YuGiOhDbContext dbContext) : base(dbContext) { }
    }
}