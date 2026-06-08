using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ST10330209PROG7311POE1.API.Services;
using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractsController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContracts([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? status)
        {
            var contracts = await _contractService.GetContractsAsync(startDate, endDate, status);
            return Ok(contracts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContract(int id)
        {
            var contract = await _contractService.GetContractByIdAsync(id);
            if (contract == null) return NotFound();
            return Ok(contract);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody] Contract contract)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _contractService.CreateContractAsync(contract);
            return CreatedAtAction(nameof(GetContract), new { id = created.Id }, created);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateContractStatus(int id, [FromBody] string newStatus)
        {
            var updated = await _contractService.UpdateContractStatusAsync(id, newStatus);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var deleted = await _contractService.DeleteContractAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}