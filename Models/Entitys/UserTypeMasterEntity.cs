using System;
using System.ComponentModel.DataAnnotations;

namespace FMSBay.Models.Entitys
{
    public class UserTypeMasterEntity
    {
        [Key]
        public int UserTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string? UserTypeName { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }

        public int? DeactivatedBy { get; set; }

        public DateTime? DeactivatedOn { get; set; }

        
    }
}
