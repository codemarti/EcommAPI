namespace Domain.DTO.DtosFuertes.MarcaDto
{
    public class MarcaUpdateDto
    {
        [Required]
        public int IdMarca { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreMarca { get; set; }
    }
}
