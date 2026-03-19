using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories
{
    public class AperturaCajaRepository
    {
        private readonly AppDbContext _context;

        public AperturaCajaRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AbrirCaja(AperturaCaja aperturaCaja)
        {
            _context.AperturaCajas.Add(aperturaCaja);
        }
        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
