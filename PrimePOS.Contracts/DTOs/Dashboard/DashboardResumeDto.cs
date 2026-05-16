namespace PrimePOS.Contracts.DTOs.Dashboard
{
    public class DashboardResumeDto
    {
        public decimal VentasDelDia { get; set; }
        public int VentasCount { get; set; }
        public int ClienteCount { get; set; }
        public int ProductoCount { get; set; }
    }
}
