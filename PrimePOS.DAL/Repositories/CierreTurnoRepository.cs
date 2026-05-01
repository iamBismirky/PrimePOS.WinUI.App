using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories
{
    public class CierreTurnoRepository : ICierreTurnoRepository
    {
        private readonly AppDbContext _context;

        public CierreTurnoRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Crear(CierreTurno cierreTurno)
        {
            _context.CierresTurno.Add(cierreTurno);
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
