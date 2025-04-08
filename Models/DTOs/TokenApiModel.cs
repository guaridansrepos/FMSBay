namespace FMSBay.Models.DTOs
{
    public class TokenApiModel
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public UserTockenModel? UserData { get; set; }
    }
}
