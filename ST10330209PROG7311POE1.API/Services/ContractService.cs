using ST10330209PROG7311POE1.Models;
using ST10330209PROG7311POE1.API.Repositories;
using ST10330209PROG7311POE1.Patterns.Observer;

namespace ST10330209PROG7311POE1.API.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;
        private readonly List<IContractObserver> _observers;

        public ContractService(IContractRepository repository)
        {
            _repository = repository;
            _observers = new List<IContractObserver> { new EmailNotifierObserver() };
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync(DateTime? startDate, DateTime? endDate, string? status) =>
            await _repository.GetAllAsync(startDate, endDate, status);

        public async Task<Contract?> GetContractByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task<Contract> CreateContractAsync(Contract contract) =>
            await _repository.AddAsync(contract);

        public async Task<Contract?> UpdateContractStatusAsync(int id, string newStatus)
        {
            var contract = await _repository.GetByIdAsync(id);
            if (contract == null) return null;

            var oldStatus = contract.Status;
            if (oldStatus == newStatus) return contract;

            contract.Status = newStatus;
            var updated = await _repository.UpdateAsync(contract);

            foreach (var observer in _observers)
                observer.Update(contract, oldStatus, newStatus);

            return updated;
        }

        public async Task<bool> DeleteContractAsync(int id) =>
            await _repository.DeleteAsync(id);
    }
}