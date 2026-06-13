using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimePOS.ENTITIES.Models.Seguridad
{
    [Table("Empresas")]
    public class Empresa
    {
        [Key]
        public int EmpresaId { get; set; }
        public string Nombre { get; set; } = "";
        public string RNC { get; set; } = "";
        public string? LogoUrl { get; set; }
        public string Telefono { get; set; } = "";
        public string Email { get; set; } = "";
        public string Direccion { get; set; } = "";
        public bool Activa { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
