global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Net;
global using AutoMapper;
#region " LLAMANDO A DTOS FUERTES "
global using Domain.DTO.DtosFuertes.CategoriaDto;
global using Domain.DTO.DtosFuertes.EstadoCarritoDto;
global using Domain.DTO.DtosFuertes.EstadoCuentaDto;
global using Domain.DTO.DtosFuertes.EstadoEmpleadoDto;
global using Domain.DTO.DtosFuertes.EstadoPagoDto;
global using Domain.DTO.DtosFuertes.EstadoSucursalDto;
global using Domain.DTO.DtosFuertes.GeneroDto;
global using Domain.DTO.DtosFuertes.HorarioSucursalDto;
global using Domain.DTO.DtosFuertes.MarcaDto;
global using Domain.DTO.DtosFuertes.MetodoEnvioDto;
global using Domain.DTO.DtosFuertes.MetodoPagoDto;
global using Domain.DTO.DtosFuertes.ModeloNegocioDto;
global using Domain.DTO.DtosFuertes.PaisDto;
global using Domain.DTO.DtosFuertes.RolDto;
global using Domain.DTO.DtosFuertes.ServiciosSucursalDto;
#endregion
#region " LLAMANDO A DTOS DEBILES "
global using Domain.DTO.DtosDebiles.CadenaDto;
global using Domain.DTO.DtosDebiles.CarritoCompraDto;
global using Domain.DTO.DtosDebiles.CarritoProductoDto;
global using Domain.DTO.DtosDebiles.CiudadDto;
global using Domain.DTO.DtosDebiles.DireccionDto;
global using Domain.DTO.DtosDebiles.EmpleadoDto;
global using Domain.DTO.DtosDebiles.EnvioDto;
global using Domain.DTO.DtosDebiles.EstadoDto;
global using Domain.DTO.DtosDebiles.PagoDto;
global using Domain.DTO.DtosDebiles.ProductoDto;
global using Domain.DTO.DtosDebiles.SubproductoDto;
global using Domain.DTO.DtosDebiles.SucursalDto;
global using Domain.DTO.DtosDebiles.SucursalEmpleadoDto;
global using Domain.DTO.DtosDebiles.TipoCadenaDto;
global using Domain.DTO.DtosDebiles.UsuarioDto;
global using Domain.DTO.DtosDebiles.VentaDto;
#endregion
#region " LLAMADO A ENTIDADES "
global using Domain.Entidades.EFuertes;
global using Domain.Entidades.EDebiles;
#endregion
#region " LLAMADO A HISTORIAL DE ENTIDADES "
global using Domain.Entidades.EHistorial;
global using Domain.DTO.DtosHistorial;
#endregion
// Requisito para validaciones de ModelState, se NECESITA el maximo de caracteres para dar un error en caso de que se pase
// [Required]
// [MaxLength(45)]