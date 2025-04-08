using System.ComponentModel.DataAnnotations;

namespace FMSBay.Models.Entitys
{
    public class ContactFormEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? LoanTypeId { get; set; }  // Foreign key
        public string? Message { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string?  UpdatedBy { get; set; }


        public LoanTypeEntity  LoanType { get; set; }
    }
}
