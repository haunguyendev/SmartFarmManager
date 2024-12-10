using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Service.Interfaces;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalSymptomController : ControllerBase
    {
        private readonly IMedicalSymptomService _medicalSymptomService;

        public MedicalSymptomController(IMedicalSymptomService medicalSymptomService)
        {
            _medicalSymptomService = medicalSymptomService;
        }

        // POST: api/medical-symptoms
        [HttpPost]
        public async Task<IActionResult> CreateMedicalSymptom([FromBody] MedicalSymptom medicalSymptom)
        {
            if (medicalSymptom == null)
            {
                return BadRequest("Invalid medical symptom data.");
            }

            var createdSymptom = await _medicalSymptomService.CreateMedicalSymptomAsync(medicalSymptom);

            return CreatedAtAction(nameof(GetMedicalSymptomById), new { id = createdSymptom.Id }, createdSymptom);
        }

        // GET: api/medical-symptoms/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMedicalSymptomById(Guid id)
        {
            var medicalSymptom = await _medicalSymptomService.GetMedicalSymptomByIdAsync(id);

            if (medicalSymptom == null)
            {
                return NotFound();
            }

            return Ok(medicalSymptom);
        }

        // GET: api/medical-symptoms
        [HttpGet]
        public async Task<IActionResult> GetMedicalSymptoms([FromQuery] string? status)
        {
            var medicalSymptoms = await _medicalSymptomService.GetMedicalSymptomsAsync(status);

            return Ok(medicalSymptoms);
        }

        // PUT: api/medical-symptoms/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMedicalSymptom(Guid id, [FromBody] MedicalSymptom updatedSymptom)
        {
            if (updatedSymptom == null || id != updatedSymptom.Id)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _medicalSymptomService.UpdateMedicalSymptomAsync(updatedSymptom);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

