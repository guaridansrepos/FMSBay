using FMSBay.BAL.IService;
using FMSBay.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMSBay.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class HomeController : Controller
    {
        
            private readonly IContactFrmService _service;

            public HomeController(IContactFrmService service)
            {
                _service = service;
            }



            // -------------------- CONTACT FORM --------------------

       [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllContacts()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

      

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactFormDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // -------------------- LOAN TYPE --------------------

        [HttpGet("LoanTypes")]
        public async Task<IActionResult> GetAllLoanTypes()
        {
            var result = await _service.GetAllLAsync();
            return Ok(result);
        }

        [HttpGet("LoanTypes/{id}")]
        public async Task<IActionResult> GetLoanTypeById(int id)
        {
            var result = await _service.GetByIdLAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("LoanTypes")]
        public async Task<IActionResult> CreateLoanType([FromBody] LoanTypeDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetLoanTypeById), new { id = created.Id }, created);
        }

        [HttpPut("LoanTypes/{id}")]
        public async Task<IActionResult> UpdateLoanType(int id, [FromBody] LoanTypeDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("LoanTypes/{id}")]
        public async Task<IActionResult> DeleteLoanType(int id)
        {
            var deleted = await _service.DeleteLAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

    }

}

