namespace Domain.Funcionalidades
{
    public class PerfilesMapeo : Profile
    {
        public PerfilesMapeo()
        {
            #region " SECCION DE MAPEO DE ENTIDADES FUERTES "
            #region " Seccion de Categoria "
            CreateMap<Categoria, CategoriaCreateDto>().ReverseMap();
            CreateMap<Categoria, CategoriaGetDto>().ReverseMap();
            CreateMap<Categoria, CategoriaUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Estado carrito compra "
            CreateMap<EstadoCarrito, EstadoCarritoCreateDto>().ReverseMap();
            CreateMap<EstadoCarrito, EstadoCarritoGetDto>().ReverseMap();
            CreateMap<EstadoCarrito, EstadoCarritoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Estado Cuenta "
            CreateMap<EstadoCuenta, EstadoCuentaCreateDto>().ReverseMap();
            CreateMap<EstadoCuenta, EstadoCuentaGetDto>().ReverseMap();
            CreateMap<EstadoCuenta, EstadoCuentaUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Estado Empleado "
            CreateMap<EstadoEmpleado, EstadoEmpleadoCreateDto>().ReverseMap();
            CreateMap<EstadoEmpleado, EstadoEmpleadoGetDto>().ReverseMap();
            CreateMap<EstadoEmpleado, EstadoEmpleadoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Estado Pago "
            CreateMap<EstadoPago, EstadoPagoCreateDto>().ReverseMap();
            CreateMap<EstadoPago, EstadoPagoGetDto>().ReverseMap();
            CreateMap<EstadoPago, EstadoPagoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Estado Sucursal "
            CreateMap<EstadoSucursal, EstadoSucursalCreateDto>().ReverseMap();
            CreateMap<EstadoSucursal, EstadoSucursalGetDto>().ReverseMap();
            CreateMap<EstadoSucursal, EstadoSucursalUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Genero "
            CreateMap<Genero, GeneroCreateDto>().ReverseMap();
            CreateMap<Genero, GeneroGetDto>().ReverseMap();
            CreateMap<Genero, GeneroUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Horario sucursal "
            CreateMap<HorarioSucursal, HorarioSucursalCreateDto>().ReverseMap();
            CreateMap<HorarioSucursal, HorarioSucursalGetDto>().ReverseMap();
            CreateMap<HorarioSucursal, HorarioSucursalUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Marca "
            CreateMap<Marca, MarcaCreateDto>().ReverseMap();
            CreateMap<Marca, MarcaGetDto>().ReverseMap();
            CreateMap<Marca, MarcaUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Metodo Envio "
            CreateMap<MetodoEnvio, MetodoEnvioCreateDto>().ReverseMap();
            CreateMap<MetodoEnvio, MetodoEnvioGetDto>().ReverseMap();
            CreateMap<MetodoEnvio, MetodoEnvioUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Metodo Pago "
            CreateMap<MetodoPago, MetodoPagoCreateDto>().ReverseMap();
            CreateMap<MetodoPago, MetodoPagoGetDto>().ReverseMap();
            CreateMap<MetodoPago, MetodoPagoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Modelo Negocio "
            CreateMap<ModeloNegocio, ModeloNegocioCreateDto>().ReverseMap();
            CreateMap<ModeloNegocio, ModeloNegocioGetDto>().ReverseMap();
            CreateMap<ModeloNegocio, ModeloNegocioUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Pais "
            CreateMap<Pais, PaisCreateDto>().ReverseMap();
            CreateMap<Pais, PaisGetDto>().ReverseMap();
            CreateMap<Pais, PaisUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Rol "
            CreateMap<Rol, RolCreateDto>().ReverseMap();
            CreateMap<Rol, RolGetDto>().ReverseMap();
            CreateMap<Rol, RolUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Servicio Sucursal "
            CreateMap<ServiciosSucursal, ServiciosSucursalCreateDto>().ReverseMap();
            CreateMap<ServiciosSucursal, ServiciosSucursalGetDto>().ReverseMap();
            CreateMap<ServiciosSucursal, ServiciosSucursalUpdateDto>().ReverseMap();
            #endregion
            #endregion
            #region " SECCION DE MAPEO DE ENTIDADES DEBILES "
            #region " Seccion de Cadena "
            CreateMap<Cadena, CadenaCreateDto>().ReverseMap();
            CreateMap<Cadena, CadenaGetDto>().ReverseMap();
            CreateMap<Cadena, CadenaUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Carrito de compras "
            CreateMap<CarritoCompra, CarritoCompraCreateDto>().ReverseMap();
            CreateMap<CarritoCompra, CarritoCompraGetDto>().ReverseMap();
            CreateMap<CarritoCompra, CarritoCompraUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Carrito y producto "
            CreateMap<CarritoProducto, CarritoProductoCreateDto>().ReverseMap();
            CreateMap<CarritoProducto, CarritoProductoGetDto>().ReverseMap();
            CreateMap<CarritoProducto, CarritoProductoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Ciudad "
            CreateMap<Ciudad, CiudadCreateDto>().ReverseMap();
            CreateMap<Ciudad, CiudadGetDto>().ReverseMap();
            CreateMap<Ciudad, CiudadUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Direccion "
            CreateMap<Direccion, DireccionCreateDto>().ReverseMap();
            CreateMap<Direccion, DireccionGetDto>().ReverseMap();
            CreateMap<Direccion, DireccionUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Empleado "
            CreateMap<Empleado, EmpleadoCreateDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoGetDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Envio "
            CreateMap<Envio, EnvioCreateDto>().ReverseMap();
            CreateMap<Envio, EnvioGetDto>().ReverseMap();
            CreateMap<Envio, EnvioUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Estado "
            CreateMap<Estado, EstadoCreateDto>().ReverseMap();
            CreateMap<Estado, EstadoGetDto>().ReverseMap();
            CreateMap<Estado, EstadoUpdateDto>().ReverseMap();
            #endregion
            // EXPLICAR SECCION PAGO
            #region " Seccion de Pago "
            CreateMap<Pago, PagoCreateDto>().ReverseMap();
            CreateMap<Pago, PagoGetDto>().ReverseMap();
            #endregion
            #region " Seccion de Producto "
            CreateMap<Producto, ProductoCreateDto>().ReverseMap();
            CreateMap<Producto, ProductoGetDto>().ReverseMap();
            CreateMap<Producto, ProductoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Subproducto "
            CreateMap<Subproducto, SubproductoCreateDto>().ReverseMap();
            CreateMap<Subproducto, SubproductoGetDto>().ReverseMap();
            CreateMap<Subproducto, SubproductoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Sucursal "
            CreateMap<Sucursal, SucursalCreateDto>().ReverseMap();
            CreateMap<Sucursal, SucursalGetDto>().ReverseMap();
            CreateMap<Sucursal, SucursalUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Sucursal y empleado "
            CreateMap<SucursalEmpleado, SucursalEmpleadoCreateDto>().ReverseMap();
            CreateMap<SucursalEmpleado, SucursalEmpleadoGetDto>().ReverseMap();
            CreateMap<SucursalEmpleado, SucursalEmpleadoUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion del Tipo de cadena "
            CreateMap<TipoCadena, TipoCadenaCreateDto>().ReverseMap();
            CreateMap<TipoCadena, TipoCadenaGetDto>().ReverseMap();
            CreateMap<TipoCadena, TipoCadenaUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Usuario "
            CreateMap<Usuario, UsuarioCreateDto>().ReverseMap();
            CreateMap<Usuario, UsuarioGetDto>().ReverseMap();
            CreateMap<Usuario, UsuarioUpdateDto>().ReverseMap();
            #endregion
            #region " Seccion de Venta "
            CreateMap<Venta, VentaCreateDto>().ReverseMap();
            CreateMap<Venta, VentaGetDto>().ReverseMap();
            CreateMap<Venta, VentaUpdateDto>().ReverseMap();
            #endregion
            #endregion
            #region " SECCION DE MAPEO DE HISTORIAL DE ENTIDADES "
            CreateMap<HistorialCategoria, HistorialCategoriaGetDto>().ReverseMap();
            CreateMap<HistorialEmpleado, HistorialEmpleadoGetDto>().ReverseMap();
            CreateMap<HistorialProducto, HistorialProductoGetDto>().ReverseMap();
            CreateMap<HistorialSubproducto, HistorialSubproductoGetDto>().ReverseMap();
            #endregion
        }
    }
}
