using FMSBay.BAL.IService;
using FMSBay.DAL.IRepo;
using FMSBay.DBContext;
using FMSBay.Models.DTOs;
using FMSBay.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace FMSBay.BAL.Service
{
    public class UserMGMTService : IUserMGMTService
    {

        private readonly IUserMGMT uRepo;
        private readonly MyDbContext _context;
        public UserMGMTService(IUserMGMT userMgmtRepo, MyDbContext context)
        {
            this.uRepo = userMgmtRepo;
            _context = context;
        }

        public async Task<LoginResponse> getUserLoginCheck(string? email, string? pwd)
        {
            return await uRepo.getUserLoginCheck(email, pwd);
        }

        public async Task<LoginResponse> getUserTokenbyClaim(string? uname)
        {
            return await uRepo.getUserTokenbyClaim(uname);
        }

        public async Task<LoginResponse> getUserTokenbyId(int id)
        {
            return await uRepo.getUserTokenbyId(id);
        }

        public async Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate)
        {
            return await uRepo.updateRefreshToken(refreshToken, userId, refTokenexpDate);
        }









        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            return await _context.UserEntity
                .Where(u => u.IsDeleted != true)
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    PWord = u.PWord,
                    EmailId = u.EmailId,
                    MobileNumber = u.MobileNumber,
                    ProfileUrl = u.ProfileUrl,
                    UserType = u.UserType,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted,
                    CreatedOn = u.CreatedOn,
                    CreatedBy = u.CreatedBy
                }).ToListAsync();
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _context.UserEntity.FirstOrDefaultAsync(x => x.UserId == id && x.IsDeleted != true);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PWord = user.PWord,
                EmailId = user.EmailId,
                MobileNumber = user.MobileNumber,
                ProfileUrl = user.ProfileUrl,
                UserType = user.UserType,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy
            };
        }

        public async Task<UserDTO> CreateAsync(UserDTO dto)
        {
            var user = new UserEntity
            {
                UserName = dto.UserName,
                PWord = dto.PWord,
                EmailId = dto.EmailId,
                MobileNumber = dto.MobileNumber,
                ProfileUrl = dto.ProfileUrl,
                UserType = dto.UserType,
                IsActive = dto.IsActive ?? true,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = dto.CreatedBy
            };

            _context.UserEntity.Add(user);
            await _context.SaveChangesAsync();

            dto.UserId = user.UserId;
            dto.CreatedOn = user.CreatedOn;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, UserDTO dto)
        {
            var user = await _context.UserEntity.FindAsync(id);
            if (user == null || user.IsDeleted == true) return false;

            user.UserName = dto.UserName;
            user.PWord = dto.PWord;
            user.EmailId = dto.EmailId;
            user.MobileNumber = dto.MobileNumber;
            user.ProfileUrl = dto.ProfileUrl;
            user.UserType = dto.UserType;
            user.IsActive = dto.IsActive;

            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.UserEntity.FindAsync(id);
            if (user == null) return false;

            user.IsDeleted = true;
            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }








    }
}
