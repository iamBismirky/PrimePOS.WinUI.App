namespace PrimePOS.Contracts.DTOs.Empresa
{
    public class CrearEmpresaDto
    {

        public string Nombre { get; set; } = "";
        public string RNC { get; set; } = "";
        public string? LogoUrl { get; set; }
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Direccion { get; set; } = "";
        public bool Activa { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
