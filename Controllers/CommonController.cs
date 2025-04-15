using FMSBay.BAL.IService;
using FMSBay.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMSBay.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class CommonController : Controller
    {
        private readonly IContactFrmService _service;

        public CommonController(IContactFrmService service)
        {
            _service = service;
        }

        [HttpGet("LoanTypes/Dropdown")]
        public async Task<IActionResult> GetLoanTypeDropdown()
        {
            var result = await _service.GetDropdownListAsync();
            return Ok(result);
        }

         

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactFormDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            return Ok(new { message = "Contact created successfully" });
        }


    }
}
