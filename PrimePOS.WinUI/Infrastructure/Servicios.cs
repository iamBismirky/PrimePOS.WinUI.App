using Microsoft.EntityFrameworkCore;
using PrimePOS.BLL.Services;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation.Collections;

namespace PrimePOS.WinUI.Infrastructure
{

    public static class Servicios
    {
        public static AppDbContext _context = null!;

        public static UsuarioService UsuarioService { get; private set; } = null!;
        public static RolService RolService { get; private set; } = null!;
        public static CategoriaService CategoriaService { get; private set; } = null!;

        
        public static void Inicializar()
        {
            //Cadena de conexión
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=PrimePOS.DB.WEB;Trusted_Connection=True;TrustServerCertificate=True;";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;




            var _context = new AppDbContext(options);

            // Crear repositorio
            var usuarioRepository = new UsuarioRepository(_context);
            var rolRepository = new RolRepository(_context);
            var categoriaRepository = new CategoriaRepository(_context);

            // Crear service
            UsuarioService = new UsuarioService(usuarioRepository);
            RolService = new RolService(rolRepository);
            CategoriaService = new CategoriaService(categoriaRepository);
        }
    }
}

