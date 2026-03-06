using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Infrastructure
{
    public static class SesionUsuario
    {
        public static int UsuarioId { get; set; } = 0;
        public static string? NombreUsuario { get; set; } =string.Empty;
        public static int RoldId {  get; set; } 
        public static string RolNombre { get; set; } = string.Empty;
        public static bool Activa { get; set; } 

        public static void Iniciar(int usuarioId, string nombreUsuario, int rolId, string rolNombre)
        {
            UsuarioId = usuarioId;
            NombreUsuario = nombreUsuario;
            RoldId = rolId;
            RolNombre = rolNombre;
            Activa = true;
        }
        public static void Cerrar()
        {
            UsuarioId = 0;
            NombreUsuario = string.Empty;
            RoldId = 0;   
            Activa = false;
        }
    }
}
