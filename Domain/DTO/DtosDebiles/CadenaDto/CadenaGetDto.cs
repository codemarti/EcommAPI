namespace Domain.DTO.DtosDebiles.CadenaDto
{
    public class CadenaGetDto
    {
        [Required]
        public int IdCadena { get; set; }
        // Atributo: RFC de la cadena
        [Required]
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
        public Direccion? Direccion { get; set; }
        [Required]
        public TipoCadena? TipoCadena { get; set; }
    }
}
