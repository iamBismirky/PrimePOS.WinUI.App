using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories
{
    public class CierreTurnoRepository
    {
        private readonly AppDbContext _context;

        public CierreTurnoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(CierreTurno cierreTurno)
        {
            await _context.CierresTurno.AddAsync(cierreTurno);
        }

        public void Actualizar(CierreTurno cierreTurn)
        {
            _context.CierresTurno.Update(cierreTurn);
        }







        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
