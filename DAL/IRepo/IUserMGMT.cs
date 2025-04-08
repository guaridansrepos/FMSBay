using FMSBay.Models.DTOs;
using FMSBay.Models.Entitys;

namespace FMSBay.DAL.IRepo
{
    public interface IUserMGMT
    {

        Task<LoginResponse> getUserLoginCheck(string? email, string? pwd);
        UserEntity GetUserById(int id);

        Task<LoginResponse> getUserTokenbyClaim(string? uname);
        Task<LoginResponse> getUserTokenbyId(int id);
        Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate);

    }
}
