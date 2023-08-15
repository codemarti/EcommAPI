namespace EcommWebApi.Datos
{
    public class DatosDbContext : DbContext
    {
        public DatosDbContext(DbContextOptions<DatosDbContext> options) : base(options) { }

        #region " CREACION DE MODELOS FUERTES EN LA BASE DE DATOS "
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EstadoCarrito> EstadoCarritos { get; set; }
        public DbSet<EstadoCuenta> EstadoCuentas { get; set; }
        public DbSet<EstadoEmpleado> EstadoEmpleados { get; set; }
        public DbSet<EstadoPago> EstadoPagos { get; set; }
        public DbSet<EstadoSucursal> EstadoSucursales { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<HorarioSucursal> HorarioSucursales { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<MetodoEnvio> MetodoEnvios { get; set; }
        public DbSet<MetodoPago> MetodoPagos { get; set; }
        public DbSet<ModeloNegocio> ModeloNegocios { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<ServiciosSucursal> ServiciosSucursales { get; set; }
        #endregion
        #region " CREACION DE MODELOS DEBILES EN LA BASE DE DATOS "
        public DbSet<Cadena> Cadenas { get; set; }
        public DbSet<CarritoCompra> CarritoCompras { get; set; }
        public DbSet<CarritoProducto> CarritoProductos { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<ContactoEmpleado> ContactoEmpleados { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Subproducto> Subproductos { get; set; }
        public DbSet<SubproductoSerie> SubproductosSerie { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<SucursalEmpleado> SucursalesEmpleados { get; set; }
        public DbSet<TipoCadena> TipoCadenas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        #endregion
        #region " CREACION DE HISTORIAL DE MODELOS EN LA BASE DE DATOS "
        public DbSet<HistorialCategoria> HistorialCategorias { get; set; }
        public DbSet<HistorialEmpleado> HistorialEmpleados { get; set; }
        public DbSet<HistorialProducto> HistorialProductos { get; set; }
        public DbSet<HistorialSubproducto> HistorialSubproductos { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region " ALIMENTACION DE TABLAS FUERTES "
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria()
                {
                    IdCategoria = 1,
                    NombreCategoria = "Electrodomesticos"
                }
            );
            modelBuilder.Entity<EstadoCarrito>().HasData(
                new EstadoCarrito()
                {
                    IdEdoCarrito = 1,
                    NombreEdoCarrito = "Activo"
                }
            );
            modelBuilder.Entity<EstadoCuenta>().HasData(
                new EstadoCuenta()
                {
                    IdEdoCuenta = 1,
                    NombreEdoCuenta = "Activo"
                }
            );
            modelBuilder.Entity<EstadoEmpleado>().HasData(
                new EstadoEmpleado()
                {
                    IdEdoEmpleado = 1,
                    NombreEdoEmpleado = "Activo"
                }
            );
            modelBuilder.Entity<EstadoPago>().HasData(
                new EstadoPago()
                {
                    IdEdoPago = 1,
                    NombreEdoPago = "No pagado"
                }
            );
            modelBuilder.Entity<EstadoSucursal>().HasData(
                new EstadoSucursal()
                {
                    IdEdoSucursal = 1,
                    NombreEdoSucursal = "Activa"
                }
            );
            modelBuilder.Entity<Genero>().HasData(
                new Genero()
                {
                    IdGenero = 1,
                    NombreGenero = "Masculino"
                }
            );
            modelBuilder.Entity<HorarioSucursal>().HasData(
                new HorarioSucursal()
                {
                    IdHorario = 1,
                    Lunes = "06:00 - 15:00",
                    Martes = "06:00 - 15:00",
                    Miercoles = "06:00 - 15:00",
                    Jueves = "06:00 - 15:00",
                    Viernes = "06:00 - 15:00",
                    Sabado = "09:00 - 14:00",
                    Domingo = "No disponible"
                }
            );
            modelBuilder.Entity<Marca>().HasData(
                new Marca()
                {
                    IdMarca = 1,
                    NombreMarca = "Dell"
                }
            );
            modelBuilder.Entity<MetodoEnvio>().HasData(
                new MetodoEnvio()
                {
                    IdMetodoEnvio = 1,
                    NombreMetodoEnvio = "A domicilio"
                }
            );
            modelBuilder.Entity<MetodoPago>().HasData(
                new MetodoPago()
                {
                    IdMetodoPago = 1,
                    NombreMetodoPago = "Tarjeta debito/credito"
                }
            );
            modelBuilder.Entity<ModeloNegocio>().HasData(
                new ModeloNegocio()
                {
                    IdModelo = 1,
                    NombreModelo = "Comercio electronico"
                }
            );
            modelBuilder.Entity<Pais>().HasData(
                new Pais()
                {
                    IdPais = 1,
                    NombrePais = "Mexico",
                    ISO = "MX"
                }
            );
            modelBuilder.Entity<Rol>().HasData(
                new Rol()
                {
                    IdRol = 1,
                    NombreRol = "Administrador"
                }
            );
            modelBuilder.Entity<ServiciosSucursal>().HasData(
                new ServiciosSucursal()
                {
                    IdServicio = 1,
                    NombreServicio = "Devoluciones y cambios"
                }
            );
            #endregion
            #region " CONFIGURACION DE FORANEAS Y ESTRUCTURA DE ENTIDADES DEBILES "
            modelBuilder.Entity<CarritoCompra>(entity =>
            {
                entity.HasOne(cc => cc.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Envio>(entity =>
            {
                entity.HasOne(e => e.Venta)
                .WithMany()
                .HasForeignKey(e => e.VentaId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.HasOne(s => s.Direccion)
                    .WithMany()
                    .HasForeignKey(s => s.DireccionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(s => s.HorarioSucursal)
                    .WithMany()
                    .HasForeignKey(s => s.HorarioId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<SucursalEmpleado>(entity =>
            {
                entity.HasOne(se => se.Sucursal)
                    .WithMany()
                    .HasForeignKey(se => se.SucursalId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(se => se.Empleado)
                    .WithMany()
                    .HasForeignKey(se => se.EmpleadoId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasOne(v => v.Empleado)
                .WithMany()
                .HasForeignKey(v => v.EmpleadoId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            #endregion
            #region " ALIMENTACION DE ENTIDADES DEBILES "
            modelBuilder.Entity<Cadena>().HasData(
                new Cadena()
                {
                    IdCadena = 1,
                    RFCC = "LDKAUEIOPD25A",
                    NombreCadena = "Barcel",
                    PPC = "Hay muchas variaciones de los pasajes de Lorem Ipsum disponibles, pero la mayoría sufrió alteraciones en alguna manera, " +
                    "ya sea porque se le agregó humor, o palabras aleatorias que no parecen ni un poco creíbles. Si vas a utilizar un pasaje de Lorem" +
                    " Ipsum, necesitás estar seguro de que no hay nada avergonzante escondido en el medio del texto. ",
                    TCC = "Hay muchas variaciones de los pasajes de Lorem Ipsum disponibles, pero la mayoría sufrió alteraciones en alguna manera, ya " +
                    "sea porque se le agregó humor, o palabras aleatorias que no parecen ni un poco creíbles. Si vas a utilizar un pasaje de Lorem Ipsum," +
                    " necesitás estar seguro de que no hay nada avergonzante escondido en el medio del texto. ",
                    DireccionId = 1,
                    TipoCadenaId = 1,
                }
            );

            modelBuilder.Entity<CarritoCompra>().HasData(
                new CarritoCompra()
                {
                    IdCarrito = 1,
                    CodigoCarrito = "VNTRZD01",
                    Descuento = 0,
                    Impuestos = 0,
                    Total = 0,
                    CarritoProductoId = 1,
                    EdoCarritoId = 1,
                    PagoId = 1,
                    UsuarioId = 1
                }
             );

            modelBuilder.Entity<CarritoProducto>().HasData(
                new CarritoProducto()
                {
                    IdCarritoProducto = 1,
                    Subtotal = 705.50M,
                    Cantidad = 3,
                    FechaCreacionCarrito = DateTime.UtcNow,
                    FechaActualizacionCarrito = DateTime.UtcNow,
                    SubproductoId = 1
                }
             );

            modelBuilder.Entity<Ciudad>().HasData(
                new Ciudad()
                {
                    IdCiudad = 1,
                    NombreCiudad = "Cancún",
                    EstadoId = 1
                }
            );
            modelBuilder.Entity<ContactoEmpleado>().HasData(
                new ContactoEmpleado()
                {
                    IdContacto = 1,
                    NombreContacto = "Jafet Sanguino",
                    Telefono = "9991924567",
                    Parentesco = "Hermano",
                    EmpleadoId = 1
                }
            );
            modelBuilder.Entity<Direccion>().HasData(
                new Direccion()
                {
                    IdDireccion = 1,
                    CP = "77900",
                    Calle1 = "Av.Tules",
                    Calle2 = "Av.Xcaret",
                    NumExt = "18",
                    Detalles = "Edicio verde de 3 pisos con logo del SEATI",
                    CiudadId = 1
                }
             );

            modelBuilder.Entity<Envio>().HasData(
                new Envio()
                {
                    IdEnvio = 1,
                    ReferenciaEnvio = "BITCHIMNOTASTALKER",
                    DireccionId = 1,
                    MetodoEnvioId = 1,
                    VentaId = 1
                }
            );

            modelBuilder.Entity<Empleado>().HasData(
                new Empleado()
                {
                    IdEmpleado = 1,
                    RFCE = "JHOAMOLSTF15B",
                    NumEmpleado = "775555",
                    NombreEmpleado = "Jhoandi Abraham",
                    ApellidoEmpleado = "Ciau Cetz",
                    TelEmpleado = "9985656563",
                    FechaNacEmpleado = new DateTime(2000, 10, 5),
                    FechaIngreso = new DateTime(2021, 10, 9),
                    FechaBaja = null,
                    NickEmpleado = "Fury",
                    EmailEmpleado = "Jhoandi@gmail.com",
                    PassEmpleado = "JHON5555",
                    DireccionId = 1,
                    EdoEmpleadoId = 1,
                    GeneroId = 1,
                    RolId = 1
                }
             );

            modelBuilder.Entity<Estado>().HasData(
                new Estado()
                {
                    IdEstado = 1,
                    NombreEstado = "Quintana Roo",
                    CodigoEstado = "77539",
                    PaisId = 1
                }
            );

            modelBuilder.Entity<Pago>().HasData(
                new Pago()
                {
                    IdPago = 1,
                    NombreTitular = "Martin Martinez Arias",
                    NumeroTarjeta = "9562865396539852",
                    CVV = "102",
                    FechaExpiracion = new DateTime(2024, 12, 3),
                    FechaCreacion = DateTime.UtcNow,
                    MetodoPagoId = 1
                }
            );

            modelBuilder.Entity<Producto>().HasData(
                new Producto()
                {
                    IdProducto = 1,
                    NombreProducto = "Lumina X-Cell",
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    CategoriaId = 1,
                    MarcaId = 1
                }
            );

            modelBuilder.Entity<Subproducto>().HasData(
                new Subproducto()
                {
                    IdSubproducto = 1,
                    CodigoBarras = "8712345678901",
                    ImagenSub = "",
                    Descripcion = "!Todo tipo de herramientas de trabajo!",
                    PrecioSub = 100M,
                    Stock = 5,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    ProductoId = 1,
                    SucursalId = 1
                }
            );

            modelBuilder.Entity<SubproductoSerie>().HasData(
                new SubproductoSerie()
                {
                    IdSerie = 1,
                    NumeroSerie = "ABC12345",
                    SubproductoId = 1
                }
            );

            modelBuilder.Entity<Sucursal>().HasData(
                new Sucursal()
                {
                    IdSucursal = 1,
                    RFCS = "THDAPKSPLK45F",
                    NombreSucursal = "Walmart",
                    TelSucursal = "9985632455",
                    FechaApertura = DateTime.UtcNow,
                    FechaCierre = new DateTime(),
                    ImagenSucursal = "",
                    PPS = "Hay muchas variaciones de los pasajes de Lorem Ipsum disponibles, pero la mayoría sufrió alteraciones en alguna manera, " +
                    "ya sea porque se le agregó humor, o palabras aleatorias que no parecen ni un poco creíbles. Si vas a utilizar un pasaje de Lorem" +
                    " Ipsum, necesitás estar seguro de que no hay nada avergonzante escondido en el medio del texto. ",
                    TCS = "Hay muchas variaciones de los pasajes de Lorem Ipsum disponibles, pero la mayoría sufrió alteraciones en alguna manera, " +
                    "ya sea porque se le agregó humor, o palabras aleatorias que no parecen ni un poco creíbles. Si vas a utilizar un pasaje de Lorem" +
                    " Ipsum, necesitás estar seguro de que no hay nada avergonzante escondido en el medio del texto. ",
                    Detalles = "Tienda de venta de productos al mayoreo",
                    CadenaId = 1,
                    DireccionId = 1,
                    EdoSucursalId = 1,
                    HorarioId = 1,
                    ServicioId = 1
                }
            );

            modelBuilder.Entity<SucursalEmpleado>().HasData(
                new SucursalEmpleado()
                {
                    IdSucursalEmpleado = 1,
                    FechaIngreso = DateTime.UtcNow,
                    EmpleadoId = 1,
                    SucursalId = 1
                }
            );

            modelBuilder.Entity<TipoCadena>().HasData(
                new TipoCadena()
                {
                    IdTipoCadena = 1,
                    NombreTipoCadena = "Servicios",
                    Descripcion = "Cadena especializada en venta de servcios",
                    ModeloId = 1
                }
            );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario()
                {
                    IdUsuario = 1,
                    NombreUsuario = "Joahan Emmanuel",
                    ApellidosUsuario = "Rosario Novelo",
                    TelUsuario = "9986453213",
                    FechaNacUsuario = new DateTime(2002, 5, 3),
                    Nickname = "Akabane",
                    EmailUsuario = "Joahan@gmail.com",
                    PasswordUsuario = "JOAHAN5555",
                    FechaRegistro = DateTime.UtcNow,
                    DireccionId = 1,
                    EdoCuentaId = 1,
                    GeneroId = 1

                }
            );

            modelBuilder.Entity<Venta>().HasData(
                new Venta()
                {
                    IdVenta = 1,
                    FechaRealizada = DateTime.UtcNow,
                    FechaDevolucion = new DateTime(),
                    Comentarios = "Muy buen producto, ¡Me encanto el producto!",
                    CarritoId = 1,
                    EmpleadoId = 1,
                    EdoPagoId = 1
                }
            );
            #endregion
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}