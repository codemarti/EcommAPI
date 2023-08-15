namespace Domain.DTO.DtosDebiles.CadenaDto
{
    public class CadenaCreateDto
    {
        // Atributo: RFC de la cadena
        [Required]
        [MaxLength(13)]
        public string? RFCC { get; set; }
        [Required]
        public string? NombreCadena { get; set; }
        public string? Imagen { get; set; }
        // Atributo: Politicas de privacidad de la cadena pueden no ser requeridas o en otras palabras ser nulas
        public string? PPC { get; set; }
        // Atributo: Terminos y condiciones de la cadena pueden no ser requeridas o en otras palabras ser nulas
        public string? TCC { get; set; }

        // Llaves foraneas
        [Required]
        public int? DireccionId { get; set; }
        [Required]
        public int? TipoCadenaId { get; set; }
    }
}
