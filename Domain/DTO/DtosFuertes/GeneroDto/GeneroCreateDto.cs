namespace Domain.DTO.DtosFuertes.GeneroDto
{
    public class GeneroCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreGenero { get; set; }
    }
}
