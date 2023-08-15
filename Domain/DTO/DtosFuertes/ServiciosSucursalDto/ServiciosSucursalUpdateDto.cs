namespace Domain.DTO.DtosFuertes.ServiciosSucursalDto
{
    public class ServiciosSucursalUpdateDto
    {
        [Required]
        public int IdServicio { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreServicio { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Decripcion { get; set; }
    }
}
