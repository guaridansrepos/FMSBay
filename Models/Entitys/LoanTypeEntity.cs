using System.ComponentModel.DataAnnotations;

namespace FMSBay.Models.Entitys
{
    public class LoanTypeEntity
    {
        [Key]
        public int Id { get; set; }
        public string? LoanType { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
