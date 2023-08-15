namespace Domain.DTO.DtosFuertes.EstadoCuentaDto
{
    public class EstadoCuentaCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreEdoCuenta { get; set; }
    }
}
