using FMSBay.BAL.IService;
using FMSBay.Models.DTOs;
using FMSBay.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMSBay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserMGMTService _service;

        public UserController(IUserMGMTService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            var response = new GenericResponse
            {
                statusCode = 200,
                Message = "Users fetched successfully",
                Data = data
            };

            return Ok(ConvertToAPI.ConvertResultToApiResonse(response));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            var response = new GenericResponse
            {
                statusCode = data != null ? 200 : 404,
                Message = data != null ? "User found" : "User not found",
                Data = data
            };

            return Ok(ConvertToAPI.ConvertResultToApiResonse(response));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserDTO req)
        {
            var result = await _service.CreateAsync(req);

            var response = new GenericResponse
            {
                statusCode = 201,
                Message = "User created successfully",
                Data = result,
                CurrentId = result.UserId // assuming UserId is returned in DTO
            };

            return Ok(ConvertToAPI.ConvertResultToApiResonse(response));
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO req)
        {
            var success = await _service.UpdateAsync(id, req);

            var response = new GenericResponse
            {
                statusCode = success ? 200 : 404,
                Message = success ? "User updated successfully" : "User not found",
                CurrentId = success ? id : null
            };

            return Ok(ConvertToAPI.ConvertResultToApiResonse(response));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            var response = new GenericResponse
            {
                statusCode = success ? 200 : 404,
                Message = success ? "User deleted successfully" : "User not found"
            };

            return Ok(ConvertToAPI.ConvertResultToApiResonse(response));
        }
    }

}


