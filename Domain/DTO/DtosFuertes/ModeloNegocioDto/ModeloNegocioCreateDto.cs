namespace Domain.DTO.DtosFuertes.ModeloNegocioDto
{
    public class ModeloNegocioCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreModelo { get; set; }
        public string? Descripcion { get; set; }
    }
}
