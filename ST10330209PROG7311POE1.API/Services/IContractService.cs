using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.API.Services
{
    public interface IContractService
    {
        Task<IEnumerable<Contract>> GetContractsAsync(DateTime? startDate, DateTime? endDate, string? status);
        Task<Contract?> GetContractByIdAsync(int id);
        Task<Contract> CreateContractAsync(Contract contract);
        Task<Contract?> UpdateContractStatusAsync(int id, string newStatus);
        Task<bool> DeleteContractAsync(int id);
    }
}