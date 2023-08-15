namespace Domain.Entidades.EFuertes
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
    }
}
