namespace Domain.DTO.DtosFuertes.RolDto
{
    public class RolCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreRol { get; set; }
    }
}
