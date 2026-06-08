using Microsoft.EntityFrameworkCore;
using ST10330209PROG7311POE1.Data;
using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.API.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _context;

        public ContractRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contract>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? status)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();
            if (startDate.HasValue) query = query.Where(c => c.StartDate >= startDate.Value);
            if (endDate.HasValue) query = query.Where(c => c.EndDate <= endDate.Value);
            if (!string.IsNullOrEmpty(status)) query = query.Where(c => c.Status == status);
            return await query.ToListAsync();
        }

        public async Task<Contract?> GetByIdAsync(int id) =>
            await _context.Contracts.Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Contract> AddAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<Contract> UpdateAsync(Contract contract)
        {
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return false;
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}