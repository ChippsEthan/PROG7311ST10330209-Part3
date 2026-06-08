using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10330209PROG7311POE1.Models;
using ST10330209PROG7311POE1.Services;

namespace ST10330209PROG7311POE1.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IApiClientService _apiClient;
        private readonly IWebHostEnvironment _env;

        public ContractsController(IApiClientService apiClient, IWebHostEnvironment env)
        {
            _apiClient = apiClient;
            _env = env;
        }

        
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? status)
        {
            var contracts = await _apiClient.GetContractsAsync(startDate, endDate, status);
            return View(contracts);
        }

        
        public async Task<IActionResult> Create()
        {
            var clients = await _apiClient.GetClientsAsync();
            ViewBag.Clients = new SelectList(clients, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile pdfFile)
        {
            
            if (pdfFile != null && pdfFile.Length > 0)
            {
                var extension = Path.GetExtension(pdfFile.FileName).ToLower();
                if (extension != ".pdf")
                {
                    ModelState.AddModelError("pdfFile", "Only PDF files are allowed.");
                    var clients = await _apiClient.GetClientsAsync();
                    ViewBag.Clients = new SelectList(clients, "Id", "Name", contract.ClientId);
                    return View(contract);
                }
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + ".pdf";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }
                contract.SignedAgreementPath = "/uploads/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                await _apiClient.CreateContractAsync(contract);
                return RedirectToAction(nameof(Index));
            }

            var clientsList = await _apiClient.GetClientsAsync();
            ViewBag.Clients = new SelectList(clientsList, "Id", "Name", contract.ClientId);
            return View(contract);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _apiClient.GetContractByIdAsync(id.Value);
            if (contract == null) return NotFound();
            var clients = await _apiClient.GetClientsAsync();
            ViewBag.Clients = new SelectList(clients, "Id", "Name", contract.ClientId);
            return View(contract);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract, IFormFile pdfFile, string newStatus)
        {
            if (id != contract.Id) return NotFound();

           
            if (pdfFile != null && pdfFile.Length > 0)
            {
                var extension = Path.GetExtension(pdfFile.FileName).ToLower();
                if (extension != ".pdf")
                {
                    ModelState.AddModelError("pdfFile", "Only PDF files are allowed.");
                    var clients = await _apiClient.GetClientsAsync();
                    ViewBag.Clients = new SelectList(clients, "Id", "Name", contract.ClientId);
                    return View(contract);
                }
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + ".pdf";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }
                contract.SignedAgreementPath = "/uploads/" + uniqueFileName;
            }

            
            var existing = await _apiClient.GetContractByIdAsync(id);
            if (existing != null && !string.IsNullOrEmpty(newStatus) && existing.Status != newStatus)
            {
                await _apiClient.UpdateContractStatusAsync(id, newStatus);
            }

           
            return RedirectToAction(nameof(Index));

           
        }

        
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus)
        {
            await _apiClient.UpdateContractStatusAsync(id, newStatus);
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _apiClient.GetContractByIdAsync(id.Value);
            if (contract == null) return NotFound();
            return View(contract);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiClient.DeleteContractAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}