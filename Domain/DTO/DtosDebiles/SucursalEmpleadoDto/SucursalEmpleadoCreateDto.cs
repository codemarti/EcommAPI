namespace Domain.DTO.DtosDebiles.SucursalEmpleadoDto
{
    public class SucursalEmpleadoCreateDto
    {
        [Required]
        public DateTime? FechaIngreso { get; set; }
        [Required]
        public int EmpleadoId { get; set; }
        [Required]
        public int SucursalId { get; set; }
    }
}
