using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10330209PROG7311POE1.Data;
using ST10330209PROG7311POE1.Models;
using ST10330209PROG7311POE1.Services;

namespace ST10330209PROG7311POE1.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(ApplicationDbContext context, CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceRequests()
        {
            var requests = await _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceRequest(int id)
        {
            var request = await _context.ServiceRequests
                .Include(r => r.Contract)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest([FromBody] ServiceRequest request)
        {
            var contract = await _context.Contracts.FindAsync(request.ContractId);
            if (contract == null) return BadRequest("Invalid contract");
            if (contract.Status == "Expired" || contract.Status == "OnHold")
                return BadRequest("Cannot create request for expired/on-hold contract");

            var rate = await _currencyService.GetUsdToZarRateAsync();
            request.CostInZAR = request.CostInUSD * rate;
            request.Status = "Open";

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServiceRequest), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceRequest(int id, [FromBody] ServiceRequest request)
        {
            if (id != request.Id) return BadRequest();
            var existing = await _context.ServiceRequests.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Description = request.Description;
            existing.CostInUSD = request.CostInUSD;
            existing.Status = request.Status;
            var rate = await _currencyService.GetUsdToZarRateAsync();
            existing.CostInZAR = existing.CostInUSD * rate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return NotFound();
            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}