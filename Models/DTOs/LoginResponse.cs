namespace FMSBay.Models.DTOs
{
    public class LoginResponse
    {
        public int statusCode { get; set; }
        public string? statusMessage { get; set; }
        public string? userTypeName { get; set; }
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefTokenExpDate { get; set; }
        public string? EmailId { get; set; }
        public int? UserType { get; set; }
        public int? RegisterType { get; set; }
        public string? ImageUrl { get; set; }
        public string? MobileNumber { get; set; }
    }


    public class UserTockenModel
    {
        public int statusCode { get; set; }
        public string? statusMessage { get; set; }
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? ImageUrl { get; set; }
        public int? UserType { get; set; }
    }




}
