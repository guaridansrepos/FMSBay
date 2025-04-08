using FMSBay.DAL.IRepo;
using FMSBay.DBContext;
using FMSBay.Models.Entitys;
using FMSBay.Utilites;
using FMSBay.Models.DTOs;
using FMSBay.Models;
using Microsoft.EntityFrameworkCore;

namespace FMSBay.DAL.Repo
{
    public class UserMGMT : IUserMGMT
    {

        private readonly MyDbContext context;
        public UserMGMT(MyDbContext _context)
        {
            context = _context;
        }



        #region User LoginCheck

        public async Task<LoginResponse> getUserLoginCheck(string? emailId, string? pwd)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                var u = await context.UserEntity.Where(a => a.MobileNumber == emailId || a.EmailId.ToLower().Trim() == emailId.ToLower().Trim() && a.IsDeleted == false).FirstOrDefaultAsync();
                if (u == null)
                {
                    response.statusCode = 0;
                    response.statusMessage = "Email/Mobile not registered / Inactive";
                }
                else if (u.IsDeleted == true)
                {
                    response.statusCode = 0;
                    response.statusMessage = "Email/Mobile is Inactive";
                }
                else if (u.IsActive == false)
                {
                    response.statusCode = 0;
                    response.statusMessage = "User is deactivated,reach out to Admin..!";
                }
                else
                {
                    string mPw = EncryptTool.Encrypt(pwd);
                    string oPw = EncryptTool.Decrypt(u.PWord);
                    string uPw = u.PWord;
                    if (!mPw.Equals(uPw))
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Password Mismatch";
                    }
                    else
                    {
                        response.EmailId = u.EmailId;
                        response.UserId = u.UserId;
                        response.ImageUrl = u.ProfileUrl;
                        response.MobileNumber = u.MobileNumber;
                        response.UserName = u.UserName;
                        response.userTypeName = await context.UserTypeMasterEntity.Where(a => a.UserTypeId == u.UserType).Select(b => b.UserTypeName).FirstOrDefaultAsync();
                        response.UserType = u.UserType;
                        response.statusCode = 1;
                        response.statusMessage = "login Sucess";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source = "getUserLoginCheck";
            }
            return response;
        }

        public UserEntity GetUserById(int id)
        {
            UserEntity u = new UserEntity();
            try
            {
                u = context.UserEntity.Where(u => u.UserId == id && u.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return u;
        }

        public async Task<LoginResponse> getUserTokenbyClaim(string? uname)
        {
            try
            {
                LoginResponse response = new LoginResponse();
                var u = await context.UserEntity.Where(a => a.UserName.Trim().Equals(uname.Trim()) && a.IsDeleted == false).
                 Select(b => new LoginResponse
                 {
                     UserId = b.UserId,
                     RefreshToken = b.RefreshToken,
                     RefTokenExpDate = b.RefTokenExpDate
                 }).FirstOrDefaultAsync();
                return u;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<LoginResponse> getUserTokenbyId(int id)
        {
            try
            {
                LoginResponse response = new LoginResponse();
                var u = await context.UserEntity.Where(a => a.UserId == id && a.IsDeleted == false).
                 Select(b => new LoginResponse
                 {
                     UserId = b.UserId,
                     RefreshToken = b.RefreshToken,
                     RefTokenExpDate = b.RefTokenExpDate
                 }).FirstOrDefaultAsync();
                return u;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate)
        {
            try
            {
                var u = await context.UserEntity.Where(a => a.UserId == userId && a.IsDeleted == false).FirstOrDefaultAsync();
                if (u == null) { return (0, "no user found"); }
                else
                {
                    u.RefreshToken = refreshToken;
                    u.RefTokenExpDate = refTokenexpDate;
                }
                int returnValue = context.SaveChanges();
                if (returnValue > 0)
                {
                    return (1, "refresh token updated sucessfully");
                }
                else
                {
                    return (0, "failed to update refresh token");
                }
            }
            catch (Exception ex)
            {
                return (0, ex.InnerException.Message);
            }
        }



        #endregion
    }
}
