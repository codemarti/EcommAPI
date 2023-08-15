namespace Domain.DTO.DtosFuertes.PaisDto
{
    public class PaisUpdateDto
    {
        [Required]
        public int IdPais { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombrePais { get; set; }
        [Required]
        [MaxLength(3)]
        public string? ISO { get; set; }
    }
}
