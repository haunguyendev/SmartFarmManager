using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Service.Interfaces;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService _medicationService;

        public MedicationController(IMedicationService medicationService)
        {
            _medicationService = medicationService;
        }

        [HttpPost("medications")]
        public async Task<IActionResult> CreateMedication([FromBody] Medication medication)
        {
            if (medication == null) return BadRequest("Invalid medication data.");

            var createdMedication = await _medicationService.CreateMedicationAsync(medication);

            return CreatedAtAction(nameof(GetMedications), null, createdMedication);
        }

        // GET: api/medications
        [HttpGet("medications")]
        public async Task<IActionResult> GetMedications()
        {
            var medications = await _medicationService.GetAllMedicationsAsync();
            return Ok(medications);
        }
    }
}
