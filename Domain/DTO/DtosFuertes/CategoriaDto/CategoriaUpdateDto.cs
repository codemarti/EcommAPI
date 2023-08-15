namespace Domain.DTO.DtosFuertes.CategoriaDto
{
    public class CategoriaUpdateDto
    {
        [Required]
        public int IdCategoria { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreCategoria { get; set; }
    }
}
