namespace FMSBay.Utilites
{
    public class GenericResponse
    {
        public string? Message { get; set; }
        public int statusCode { get; set; }
        public int? CurrentId { get; set; }
        public object? Data { get; set; }
    }
}
