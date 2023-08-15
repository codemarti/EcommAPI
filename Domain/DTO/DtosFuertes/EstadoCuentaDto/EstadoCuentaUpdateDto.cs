namespace Domain.DTO.DtosFuertes.EstadoCuentaDto
{
    public class EstadoCuentaUpdateDto
    {
        [Required]
        public int IdEdoCuenta { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreEdoCuenta { get; set; }
    }
}
