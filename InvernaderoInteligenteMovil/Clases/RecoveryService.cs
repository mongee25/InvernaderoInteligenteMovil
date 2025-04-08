using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernaderoInteligenteMovil.Clases
{
    public class RecoveryService
    {
        private const string RecoveryTokenKey = "recoveryToken"; // Clave única para el token de recuperación

        /// <summary>
        /// Guarda el token de recuperación en SecureStorage.
        /// </summary>
        public static async Task GuardarRecoveryToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync(RecoveryTokenKey, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al almacenar el recovery token: {ex.Message}");
            }
        }

        /// <summary>
        /// Recupera el token de recuperación desde SecureStorage.
        /// </summary>
        public static async Task<string?> ObtenerRecoveryToken()
        {
            try
            {
                var token = await SecureStorage.GetAsync(RecoveryTokenKey);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    return token;
                }
                else
                {
                    Console.WriteLine("No se encontró un recovery token almacenado.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el recovery token: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Elimina el token de recuperación de SecureStorage.
        /// </summary>
        public static void EliminarRecoveryToken()
        {
            try
            {
                SecureStorage.Remove(RecoveryTokenKey);
                Console.WriteLine("Recovery token eliminado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el recovery token: {ex.Message}");
            }
        }

        /// <summary>
        /// Extrae el email desde el token de recuperación.
        /// </summary>
        public static string ObtenerEmailDesdeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email");
            return emailClaim?.Value; // Retorna el email o null si no existe
        }

    }
}
