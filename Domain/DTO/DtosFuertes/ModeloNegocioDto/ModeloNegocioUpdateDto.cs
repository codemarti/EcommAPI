namespace Domain.DTO.DtosFuertes.ModeloNegocioDto
{
    public class ModeloNegocioUpdateDto
    {
        [Required]
        public int IdModelo { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreModelo { get; set; }
        public string? Descripcion { get; set; }
    }
}
