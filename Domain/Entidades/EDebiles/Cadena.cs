namespace Domain.Entidades.EDebiles;
public class Cadena
{
    [Key]
    public int IdCadena { get; set; }
    // Atributo: RFC de la cadena
    [Required]
    [MaxLength(13)]
    public string? RFCC { get; set; }
    public string? NombreCadena { get; set; }
    public string? Imagen { get; set; }
    // Atributo: Politicas de privacidad de la cadena
    public string? PPC { get; set; }
    // Atributo: Terminos y condiciones de la cadena
    public string? TCC { get; set;}

    // Llaves foraneas
    public Direccion? Direccion { get; set; }
    [ForeignKey("Direccion")]
    public int? DireccionId { get; set; }
    public TipoCadena? TipoCadena { get; set; }
    [ForeignKey("TipoCadena")]
    public int? TipoCadenaId { get; set; }
}
