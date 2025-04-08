using System.ComponentModel.DataAnnotations;

namespace FMSBay.Models.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string? emailId { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
