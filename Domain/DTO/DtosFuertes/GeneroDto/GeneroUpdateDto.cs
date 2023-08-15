namespace Domain.DTO.DtosFuertes.GeneroDto
{
    public class GeneroUpdateDto
    {
        [Required]
        public int IdGenero { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreGenero { get; set; }
    }
}
