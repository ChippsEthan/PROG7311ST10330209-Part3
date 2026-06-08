using Microsoft.AspNetCore.Mvc;
using ST10330209PROG7311POE1.Models;
using ST10330209PROG7311POE1.Services;

namespace ST10330209PROG7311POE1.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IApiClientService _apiClient;

        public ClientsController(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _apiClient.GetClientsAsync();
            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.CreateClientAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _apiClient.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _apiClient.UpdateClientAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _apiClient.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiClient.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}