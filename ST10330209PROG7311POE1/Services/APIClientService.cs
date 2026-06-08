using System.Text;
using System.Text.Json;
using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.Services
{
    public interface IApiClientService
    {
        Task<List<Contract>> GetContractsAsync(DateTime? startDate, DateTime? endDate, string? status);
        Task<Contract?> GetContractByIdAsync(int id);
        Task<Contract> CreateContractAsync(Contract contract);
        Task<Contract?> UpdateContractStatusAsync(int id, string newStatus);
        Task<bool> DeleteContractAsync(int id);

        Task<List<Client>> GetClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client> CreateClientAsync(Client client);
        Task<bool> UpdateClientAsync(Client client);
        Task<bool> DeleteClientAsync(int id);

        Task<List<ServiceRequest>> GetServiceRequestsAsync();
        Task<ServiceRequest?> GetServiceRequestByIdAsync(int id);
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request);
        Task<bool> UpdateServiceRequestAsync(ServiceRequest request);
        Task<bool> DeleteServiceRequestAsync(int id);
    }

    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClientService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["ApiBaseUrl"] ?? "http://localhost:5292";
        }

        // Contracts
        public async Task<List<Contract>> GetContractsAsync(DateTime? startDate, DateTime? endDate, string? status)
        {
            var query = $"?startDate={startDate}&endDate={endDate}&status={status}";
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/contracts{query}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Contract>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Contract>();
        }

        public async Task<Contract?> GetContractByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/contracts/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Contract>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Contract> CreateContractAsync(Contract contract)
        {
            var content = new StringContent(JsonSerializer.Serialize(contract), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/contracts", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Contract>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<Contract?> UpdateContractStatusAsync(int id, string newStatus)
        {
            var content = new StringContent($"\"{newStatus}\"", Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{_baseUrl}/api/contracts/{id}/status", content);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Contract>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> DeleteContractAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/contracts/{id}");
            return response.IsSuccessStatusCode;
        }

        // Clients
        public async Task<List<Client>> GetClientsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/clients");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Client>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Client>();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/clients/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Client>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            var content = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/clients", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Client>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            var content = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/api/clients/{client.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/clients/{id}");
            return response.IsSuccessStatusCode;
        }

        // ServiceRequests
        public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/servicerequests");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ServiceRequest>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ServiceRequest>();
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/servicerequests/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ServiceRequest>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/servicerequests", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ServiceRequest>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<bool> UpdateServiceRequestAsync(ServiceRequest request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/api/servicerequests/{request.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteServiceRequestAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/servicerequests/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}