using Gigs.Domain;
using Gigs.Domain.Models.Entities;
using gigu_back_end.Shared.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gigs.Infrastructure.Persistence.EFC.Repositories
{
    public class PullRepository : IPullRepository
    {
        private readonly GigUContext _context;

        public PullRepository(GigUContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pull>> GetAllAsync()
        {
            return await _context.Pulls.ToListAsync();
        }

        public async Task<Pull?> GetByIdAsync(int id) // ✅ Acepta retorno nulo
        {
            return await _context.Pulls.FindAsync(id);
        }

        public async Task CreateAsync(Pull pull)
        {
            await _context.Pulls.AddAsync(pull);
        }

        public Task UpdateAsync(Pull pull) // ✅ No necesita async si no hay await
        {
            _context.Pulls.Update(pull);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
