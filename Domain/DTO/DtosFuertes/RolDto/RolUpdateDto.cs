namespace Domain.DTO.DtosFuertes.RolDto
{
    public class RolUpdateDto
    {
        [Required]
        public int IdRol { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreRol { get; set; }
    }
}
