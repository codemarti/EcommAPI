namespace Domain.DTO
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExistoso { get; set; } = true;
        public List<string>? ErrorMessages { get; set; }
        public object? Resultado { get; set; }
    }
}
