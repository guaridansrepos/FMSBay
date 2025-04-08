 
    namespace FMSBay.Models.DTOs
    {
        public class ContactFormDTO
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Phone { get; set; }
            public int? LoanTypeId { get; set; }
            public string? Message { get; set; }

            public string? LoanTypeName { get; set; } // For display in GET
        }

    public class LoanTypeDTO
    {
        public int Id { get; set; }
        public string? LoanType { get; set; }
    }

}


