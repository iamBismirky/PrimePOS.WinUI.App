using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace PrimePOS.BLL.Security;


    public static class PasswordService
    {
        
        /// Genera el hash seguro de una contraseña
       
        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        
        /// Verifica si la contraseña ingresada coincide con el hash almacenado
        
        public static bool Verify(string passwordIngresada, string passwordGuardada)
        {
            if (string.IsNullOrWhiteSpace(passwordIngresada))
                return false;

            if (string.IsNullOrWhiteSpace(passwordGuardada))
                return false;

            return BCrypt.Net.BCrypt.Verify(passwordIngresada, passwordGuardada);
        }
    }

