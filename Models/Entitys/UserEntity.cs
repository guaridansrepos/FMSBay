using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FMSBay.Models.Entitys
    {
        public class UserEntity
        {
            [Key]
            public int UserId { get; set; }

            [StringLength(100)]
            public string? UserName { get; set; }

            [StringLength(100)]
            public string? PWord { get; set; }

            [StringLength(150)]
            public string? EmailId { get; set; }

            [StringLength(15)]
            public string? MobileNumber { get; set; }

            [StringLength(300)]
            public string? ProfileUrl { get; set; }

            public int? UserType { get; set; }
 
          //  public UserTypeMasterEntity? UserTypeMaster { get; set; }

            public DateTime? CreatedOn { get; set; }

            public int? CreatedBy { get; set; }

            public DateTime? ActivatedOn { get; set; }

            public int? ActivatedBy { get; set; }

            public bool? IsActive { get; set; }

            public bool? IsDeleted { get; set; }

            public string? RefreshToken { get; set; }

            public DateTime? RefTokenExpDate { get; set; }
        }
    }



