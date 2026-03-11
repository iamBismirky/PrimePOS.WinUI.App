using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.Services
{
    public class CodigoService
    {
        public string GenerarCodigo(string prefijo, int id)
        {
            return $"{prefijo}-{id:D4}";
        }
    }
}
