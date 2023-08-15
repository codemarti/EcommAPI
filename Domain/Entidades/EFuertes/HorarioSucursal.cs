namespace Domain.Entidades.EFuertes
{
    public class HorarioSucursal
    {
        [Key]
        public int IdHorario { get; set; }
        [Required]
        public string Lunes { get; set; } = string.Empty;
        [Required]
        public string Martes { get; set; } = string.Empty;
        [Required]
        public string Miercoles { get; set; } = string.Empty;
        [Required]
        public string Jueves { get; set; } = string.Empty;
        [Required]
        public string Viernes { get; set; } = string.Empty;
        [Required]
        public string Sabado { get; set; } = string.Empty;
        [Required]
        public string Domingo { get; set; } = string.Empty;
    }
}
