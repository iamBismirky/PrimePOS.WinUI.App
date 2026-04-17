using PrimePOS.Contracts.DTOs.Turno;

namespace PrimePOS.WinUI.Infrastructure
{
    public static class Sesion
    {
        //Usuario
        public static int UsuarioId { get; set; } = 0;
        public static string UsuarioNombre { get; set; } = string.Empty;
        public static int RolId { get; set; }
        public static string RolNombre { get; set; } = string.Empty;
        public static bool Activa { get; set; }


        //Caja
        public static int CajaId { get; set; } = 1;
        public static int? TurnoId { get; set; } = 0;
        public static TurnoDto? TurnoActual { get; set; }
        public static bool TieneTurno => TurnoActual != null;


        public static void Iniciar(int usuarioId, string nombreUsuario, int rolId, string rolNombre)
        {
            UsuarioId = usuarioId;
            UsuarioNombre = nombreUsuario;
            RolId = rolId;
            RolNombre = rolNombre;
            Activa = true;
        }
        public static void Cerrar()
        {
            UsuarioId = 0;
            UsuarioNombre = string.Empty;
            RolNombre = string.Empty;
            RolId = 0;

            CajaId = 0;
            TurnoId = 0;
        }


    }
}
