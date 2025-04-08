using FMSBay.Models.DTOs;

namespace FMSBay.BAL.IService
{
    public interface IUserMGMTService
    {
        Task<LoginResponse> getUserLoginCheck(string? email, string? pwd);
        Task<LoginResponse> getUserTokenbyClaim(string? uname);
        Task<LoginResponse> getUserTokenbyId(int id);
        Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate);



        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(UserDTO dto);
        Task<bool> UpdateAsync(int id, UserDTO dto);
        Task<bool> DeleteAsync(int id);


    }
}
