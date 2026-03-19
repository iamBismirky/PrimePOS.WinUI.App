using Microsoft.EntityFrameworkCore;
using PrimePOS.BLL.Services;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;

namespace PrimePOS.WinUI.Infrastructure
{

    public static class Servicios
    {
        public static AppDbContext _context = null!;
        public static UsuarioService UsuarioService { get; private set; } = null!;
        public static RolService RolService { get; private set; } = null!;
        public static CategoriaService CategoriaService { get; private set; } = null!;
        public static ClienteService ClienteService { get; private set; } = null!;
        public static ProductoService ProductoService { get; private set; } = null!;
        public static VentaService VentaService { get; private set; } = null!;
        public static MetodoPagoService MetodoPagoService { get; private set; } = null!;
        public static CajaService CajaService { get; private set; } = null!;
        public static TurnoService TurnoService { get; private set; } = null!;


        public static void Inicializar()
        {
            //Cadena de conexión
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=PrimePOS_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;



            _context = new AppDbContext(options);



            // Crear repositorio
            var usuarioRepository = new UsuarioRepository(_context);
            var rolRepository = new RolRepository(_context);
            var categoriaRepository = new CategoriaRepository(_context);
            var clienteRepository = new ClienteRepository(_context);
            var productoRepository = new ProductoRepository(_context);
            var VentaRepository = new VentaRepository(_context);
            var MetodoPagoRepository = new MetodoPagoRepository(_context);
            var CajaRepository = new CajaRepository(_context);
            var TurnoRepository = new TurnoRepository(_context);

            // Crear service
            UsuarioService = new UsuarioService(usuarioRepository);
            RolService = new RolService(rolRepository);
            CategoriaService = new CategoriaService(categoriaRepository);
            ClienteService = new ClienteService(clienteRepository);
            ProductoService = new ProductoService(productoRepository);
            VentaService = new VentaService(VentaRepository);
            MetodoPagoService = new MetodoPagoService(MetodoPagoRepository);
            CajaService = new CajaService(CajaRepository);
            TurnoService = new TurnoService(TurnoRepository, CajaRepository, _context);
        }
    }
}

