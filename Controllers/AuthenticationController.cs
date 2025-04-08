using FMSBay.BAL.IService;
using FMSBay.Models.DTOs;
using FMSBay.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMSBay.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserMGMTService uService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration config;
        private string? generatedToken = string.Empty;
        public AuthenticationController(IUserMGMTService userMgmtService, IConfiguration configuration, ITokenService tokenService)
        {
            uService = userMgmtService;
            _tokenService = tokenService;
            config = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest userobj)
        {
            GenericResponse results = new GenericResponse();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var res = await uService.getUserLoginCheck(userobj.emailId, userobj.password);
                if (res.statusCode == 1)
                {

                    var claims = GenerateClaims(res);
                    var generatedToken = _tokenService.BuildToken(config["Jwt:key"], config["Jwt:ValidIssuer"], config["Jwt:ValidAudience"], claims);
                    var RefreshToken = _tokenService.GenerateRefreshToken();
                    var RefreshTokenExpTime = DateTime.Now.AddDays(7);
                    var (statusi, messagei) = await uService.updateRefreshToken(RefreshToken, res.UserId, RefreshTokenExpTime);
                    UserTockenModel data = new UserTockenModel()
                    {
                        statusCode = res.statusCode,
                        statusMessage = res.statusMessage,
                        UserName = res.UserName,
                        ImageUrl = res.ImageUrl,
                        UserType = res.UserType,
                        UserId = res.UserId,
                    };
                    var tokendata = new TokenApiModel()
                    {
                        UserData = data,
                        AccessToken = generatedToken,
                        RefreshToken = RefreshToken,
                        RefreshTokenExpiryTime = RefreshTokenExpTime
                    };
                    results.Message = res.statusMessage;
                    results.statusCode = res.statusCode;
                    var finalResponse = ConvertToAPI.ConvertResultToApiResonse(tokendata);
                    finalResponse.Succeded = true;
                    return Ok(finalResponse);
                }
                else
                {
                    results.Message = res.statusMessage;
                    results.statusCode = 0;
                    var finalResponse = ConvertToAPI.ConvertResultToApiResonse(results);
                    finalResponse.Succeded = false;
                    return BadRequest(finalResponse);
                }
            }
            catch (Exception ex)
            {
                results.Message = ex.Message;
                results.statusCode = 0;
                var finalResponse = ConvertToAPI.ConvertResultToApiResonse(results);
                finalResponse.Succeded = false;
                return Unauthorized(finalResponse);
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> refresh(TokenApiModel tokenApiModel)

        {
            GenericResponse results = new GenericResponse();
            try
            {
                if (tokenApiModel is null)
                    return BadRequest("Invalid client request");
                string accessToken = tokenApiModel.AccessToken;
                string refreshToken = tokenApiModel.RefreshToken;
                var principal = _tokenService.GetPrincipalFromExpiredToken(config["Jwt:key"], config["Jwt:ValidIssuer"], config["Jwt:ValidAudience"], accessToken);
                var username = principal.Identity.Name;
                var user = await uService.getUserTokenbyClaim(username);
                //var user = await uService.getUserTokenbyId((int)tokenApiModel.UserData.UserId);
                if (user is null || user.RefreshToken != refreshToken || user.RefTokenExpDate <= DateTime.Now)
                    return BadRequest("Invalid client request");
                var newAccessToken = _tokenService.BuildToken(config["Jwt:key"], config["Jwt:ValidIssuer"], config["Jwt:ValidAudience"], principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                var RefreshTokenExpTime = DateTime.Now.AddDays(7);
                var (status, message) = await uService.updateRefreshToken(newRefreshToken, user.UserId, RefreshTokenExpTime);
                if (status == 1)
                {
                    var tokendata = new TokenApiModel()
                    { RefreshToken = newRefreshToken, AccessToken = newAccessToken, RefreshTokenExpiryTime = RefreshTokenExpTime };
                    results.Message = message;
                    results.statusCode = 1;
                    var finalResponse = ConvertToAPI.ConvertResultToApiResonse(tokendata);
                    finalResponse.Succeded = true;
                    return Ok(finalResponse);
                }
                else
                {
                    results.Message = message;
                    results.statusCode = 0;
                    var finalResponse = ConvertToAPI.ConvertResultToApiResonse(results);
                    finalResponse.Succeded = false;
                    return Ok(finalResponse);
                }
            }
            catch (Exception ex)
            {
                results.Message = ex.Message;
                results.statusCode = 0;
                var finalResponse = ConvertToAPI.ConvertResultToApiResonse(results);
                finalResponse.Succeded = false;
                return Unauthorized(finalResponse);
            }
        }

        private static List<Claim> GenerateClaims(LoginResponse? res)
        {
            var claims = new List<Claim>
              {
                new Claim(ClaimTypes.Name, res?.UserName ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, res?.MobileNumber ?? string.Empty),
                new Claim(ClaimTypes.Email, res?.EmailId ?? string.Empty),
                new Claim("GUID", Guid.NewGuid().ToString()), // Unique GUID
				new Claim("UserTypeID", res?.UserType.ToString() ?? string.Empty),
                new Claim("UserTypeName", res?.userTypeName ?? string.Empty),
                new Claim("UserID", res?.UserId.ToString() ?? string.Empty),
                new Claim("ProfileImage", res?.ImageUrl ?? string.Empty),
                new Claim("UserName", res?.UserName ?? string.Empty)
                 };
            return claims;
        }

    }
}
