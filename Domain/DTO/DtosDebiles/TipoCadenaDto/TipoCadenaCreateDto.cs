﻿namespace Domain.DTO.DtosDebiles.TipoCadenaDto
{
    public class TipoCadenaCreateDto
    {
        [Required]
        public string? NombreTipoCadena { get; set; }
        public string? Descripcion { get; set; }

        // Llave foranea
        [Required]
        public int ModeloId { get; set; }
    }
}
