using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10330209PROG7311POE1.Models;
using ST10330209PROG7311POE1.Services;

namespace ST10330209PROG7311POE1.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly IApiClientService _apiClient;

        public ServiceRequestsController(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var requests = await _apiClient.GetServiceRequestsAsync();
            return View(requests);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var request = await _apiClient.GetServiceRequestByIdAsync(id.Value);
            if (request == null) return NotFound();
            return View(request);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            // Get all contracts with client names for dropdown
            var contracts = await _apiClient.GetContractsAsync(null, null, null);
            var contractList = contracts.Select(c => new {
                Id = c.Id,
                Display = $"{c.Id} - {(c.Client?.Name ?? "Unknown")}"
            });
            ViewBag.Contracts = new SelectList(contractList, "Id", "Display");
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.CreateServiceRequestAsync(request);
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown on error
            var contracts = await _apiClient.GetContractsAsync(null, null, null);
            var contractList = contracts.Select(c => new {
                Id = c.Id,
                Display = $"{c.Id} - {(c.Client?.Name ?? "Unknown")}"
            });
            ViewBag.Contracts = new SelectList(contractList, "Id", "Display", request.ContractId);
            return View(request);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _apiClient.GetServiceRequestByIdAsync(id.Value);
            if (request == null) return NotFound();

            var contracts = await _apiClient.GetContractsAsync(null, null, null);
            var contractList = contracts.Select(c => new {
                Id = c.Id,
                Display = $"{c.Id} - {(c.Client?.Name ?? "Unknown")}"
            });
            ViewBag.Contracts = new SelectList(contractList, "Id", "Display", request.ContractId);
            return View(request);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest request)
        {
            if (id != request.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _apiClient.UpdateServiceRequestAsync(request);
                return RedirectToAction(nameof(Index));
            }

            var contracts = await _apiClient.GetContractsAsync(null, null, null);
            var contractList = contracts.Select(c => new {
                Id = c.Id,
                Display = $"{c.Id} - {(c.Client?.Name ?? "Unknown")}"
            });
            ViewBag.Contracts = new SelectList(contractList, "Id", "Display", request.ContractId);
            return View(request);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var request = await _apiClient.GetServiceRequestByIdAsync(id.Value);
            if (request == null) return NotFound();
            return View(request);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiClient.DeleteServiceRequestAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}