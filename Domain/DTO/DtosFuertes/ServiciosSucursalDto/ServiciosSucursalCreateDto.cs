namespace Domain.DTO.DtosFuertes.ServiciosSucursalDto
{
    public class ServiciosSucursalCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreServicio { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Decripcion { get; set; }
    }
}
