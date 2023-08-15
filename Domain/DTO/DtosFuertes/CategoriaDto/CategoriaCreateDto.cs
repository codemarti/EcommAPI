namespace Domain.DTO.DtosFuertes.CategoriaDto
{
    public class CategoriaCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreCategoria { get; set; }
    }
}
