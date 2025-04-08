using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernaderoInteligenteMovil.Clases
{
    public static class AuthService
    {
        private const string Token = "token"; // Constante para evitar strings duplicados

        /// <summary>
        /// Guarda el token de autenticación en SecureStorage
        /// </summary>
        public static async Task GuardarToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync(Token, token);
                Console.WriteLine("Token guardado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al almacenar el token: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el token de autenticación desde SecureStorage
        /// </summary>
        public static async Task<string?> ObtenerToken()
        {
            try
            {
                var token = await SecureStorage.GetAsync(Token);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("Token recuperado correctamente.");
                    return token;
                }
                else
                {
                    Console.WriteLine("No se encontró un token almacenado.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el token: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Elimina el token de SecureStorage (Para logout)
        /// </summary>
        public static void EliminarToken()
        {
            try
            {
                SecureStorage.Remove(Token);
                Console.WriteLine("Token eliminado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el token: {ex.Message}");
            }
        }
    }
}
