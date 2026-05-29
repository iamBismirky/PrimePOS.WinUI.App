using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;

namespace PrimePOS.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context) => _context = context;


        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
    }
}
