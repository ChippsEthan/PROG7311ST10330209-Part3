using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.API.Repositories
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? status);
        Task<Contract?> GetByIdAsync(int id);
        Task<Contract> AddAsync(Contract contract);
        Task<Contract> UpdateAsync(Contract contract);
        Task<bool> DeleteAsync(int id);
    }
}