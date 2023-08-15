namespace Domain.DTO.DtosFuertes.MarcaDto
{
    public class MarcaCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreMarca { get; set; }
    }
}
