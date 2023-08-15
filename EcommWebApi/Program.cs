#region " USO DE BIBLIOTECAS GLOBALES "
global using System.Net;
global using System.Text;
global using System.Linq.Expressions;
global using EcommWebApi.Datos;
global using Domain.DTO;
global using Domain.DTO.DtosHistorial;
global using Domain.Entidades.EHistorial;
global using Domain.Funcionalidades;
global using AutoMapper;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using EcommWebApi.Repositorios;
global using EcommWebApi.Repositorios.ReposFuertes.Repos;
global using EcommWebApi.Repositorios.ReposFuertes.IRepos;
global using EcommWebApi.Repositorios.ReposDebiles.Repos;
global using EcommWebApi.Repositorios.ReposDebiles.IRepos;
#endregion
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
global using Domain.Entidades.EFuertes;
#endregion
#region " LLAMANDO A DTOS DEBILES "
global using Domain.DTO.DtosDebiles.CadenaDto;
global using Domain.DTO.DtosDebiles.CarritoCompraDto;
global using Domain.DTO.DtosDebiles.CarritoProductoDto;
global using Domain.DTO.DtosDebiles.CiudadDto;
global using Domain.DTO.DtosDebiles.ContactoEmpleadoDto;
global using Domain.DTO.DtosDebiles.DireccionDto;
global using Domain.DTO.DtosDebiles.EmpleadoDto;
global using Domain.DTO.DtosDebiles.EnvioDto;
global using Domain.DTO.DtosDebiles.EstadoDto;
global using Domain.DTO.DtosDebiles.PagoDto;
global using Domain.DTO.DtosDebiles.ProductoDto;
global using Domain.DTO.DtosDebiles.SubproductoDto;
global using Domain.DTO.DtosDebiles.SubproductoSerieDto;
global using Domain.DTO.DtosDebiles.SucursalDto;
global using Domain.DTO.DtosDebiles.SucursalEmpleadoDto;
global using Domain.DTO.DtosDebiles.TipoCadenaDto;
global using Domain.DTO.DtosDebiles.UsuarioDto;
global using Domain.DTO.DtosDebiles.VentaDto;
global using Domain.Entidades.EDebiles;
#endregion
#region " BIBLIOTECAS AUTENTICACION / AUTORIZACION "
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using EcommWebApi.ConfiguracionJwt.UsuarioJwt;
global using EcommWebApi.ConfiguracionJwt.EmpleadoJwt;
#endregion


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

#region " CONFIGURACION DE AUTENTICACION / AUTORIZACION "
string jwtKey = builder.Configuration["JwtConfiguracion:Key"];
if (jwtKey == null)
{
    throw new InvalidOperationException("La clave de JWT no está configurada correctamente.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JwtConfiguracion:Audience"],
        ValidIssuer = builder.Configuration["JwtConfiguracion:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddScoped<IUsuarioJwt, UsuarioJwt>();
builder.Services.AddScoped<IEmpleadoJwt, EmpleadoJwt>();
#endregion
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region " CONEXION A BASE DE DATOS "
builder.Services.AddDbContext<DatosDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcommDatos"));
});
#endregion
// IMPLEMENTAR LA FUNCIONALIDAD DE MAPEO DE LAS ENTIDADES
builder.Services.AddAutoMapper(typeof(PerfilesMapeo));
#region " INYECCION DE SERVICIOS DE REPOSITORIOS "
#region " REPOSITORIOS FUERTES "
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IEstadoCarritoRepositorio, EstadoCarritoRepositorio>();
builder.Services.AddScoped<IEstadoCuentaRepositorio, EstadoCuentaRepositorio>();
builder.Services.AddScoped<IEstadoEmpleadoRepositorio, EstadoEmpleadoRepositorio>();
builder.Services.AddScoped<IEstadoPagoRepositorio, EstadoPagoRepositorio>();
builder.Services.AddScoped<IEstadoSucursalRepositorio, EstadoSucursalRepositorio>();
builder.Services.AddScoped<IGeneroRepositorio, GeneroRepositorio>();
builder.Services.AddScoped<IHorarioSucursalRepositorio, HorarioSucursalRepositorio>();
builder.Services.AddScoped<IMarcaRepositorio, MarcaRepositorio>();
builder.Services.AddScoped<IMetodoEnvioRepositorio, MetodoEnvioRepositorio>();
builder.Services.AddScoped<IMetodoPagoRepositorio, MetodoPagoRepositorio>();
builder.Services.AddScoped<IModeloNegocioRepositorio, ModeloNegocioRepositorio>();
builder.Services.AddScoped<IPaisRepositorio, PaisRepositorio>();
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
builder.Services.AddScoped<IServiciosSucursalRepositorio, ServiciosSucursalRepositorio>();
#endregion
#region " REPOSITORIOS DEBILES "
builder.Services.AddScoped<ICadenaRepositorio, CadenaRepositorio>();
builder.Services.AddScoped<ICarritoCompraRepositorio, CarritoCompraRepositorio>();
builder.Services.AddScoped<ICarritoProductoRepositorio, CarritoProductoRepositorio>();
builder.Services.AddScoped<ICiudadRepositorio, CiudadRepositorio>();
builder.Services.AddScoped<IContactoEmpleadoRepositorio, ContactoEmpleadoRepositorio>();
builder.Services.AddScoped<IDireccionRepositorio, DireccionRepositorio>();
builder.Services.AddScoped<IEmpleadoRepositorio, EmpleadoRepositorio>();
builder.Services.AddScoped<IEnvioRepositorio, EnvioRepositorio>();
builder.Services.AddScoped<IEstadoRepositorio, EstadoRepositorio>();
builder.Services.AddScoped<IRepositorio<Pago>, Repositorio<Pago>>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<ISubproductoRepositorio, SubproductoRepositorio>();
builder.Services.AddScoped<ISubproductoSerieRepositorio, SubproductoSerieRepositorio>();
builder.Services.AddScoped<ISucursalRepositorio, SucursalRepositorio>();
builder.Services.AddScoped<ISucursalEmpleadoRepositorio, SucursalEmpleadoRepositorio>();
builder.Services.AddScoped<ITipoCadenaRepositorio, TipoCadenaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();
#endregion
#region " REPOSITORIOS (HISTORIALES DE ENTIDADES) "
builder.Services.AddScoped<IRepositorio<HistorialCategoria>, Repositorio<HistorialCategoria>>();
builder.Services.AddScoped<IRepositorio<HistorialEmpleado>, Repositorio<HistorialEmpleado>>();
builder.Services.AddScoped<IRepositorio<HistorialProducto>, Repositorio<HistorialProducto>>();
builder.Services.AddScoped<IRepositorio<HistorialSubproducto>, Repositorio<HistorialSubproducto>>();
#endregion
#endregion
#region " IMPLEMENTACION DEL CORS "
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.SetIsOriginAllowed(origin => true) // Permite cualquier origen
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();