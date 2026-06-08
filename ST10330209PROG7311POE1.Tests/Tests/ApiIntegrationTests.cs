using System.Net;
using System.Text.Json;
using Xunit;

namespace ST10330209PROG7311POE1.Tests
{
    public class SimpleApiTests
    {
        private readonly HttpClient _client = new();
        private readonly string _baseUrl = "http://localhost:5292";

        [Fact]
        public async Task GetContracts_ReturnsOk()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/contracts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetClients_ReturnsOk()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/clients");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetServiceRequests_ReturnsOk()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/servicerequests");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetContractById_Invalid_ReturnsNotFound()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/contracts/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}