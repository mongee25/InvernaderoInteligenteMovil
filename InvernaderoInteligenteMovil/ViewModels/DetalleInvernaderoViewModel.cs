using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class DetalleInvernaderoViewModel : BaseViewModel
    {

        public string NombreInvernadero { get; } // Propiedad que guarda el nombre
        public ICommand BorrarCommand { get; }

        public DetalleInvernaderoViewModel(string nombreInvernadero)
        {
            NombreInvernadero = nombreInvernadero; // Guarda el nombre recibido

            BorrarCommand = new Command(async () =>
            {
                bool confirmar = await Application.Current.MainPage.DisplayAlert(
                    "Confirmar",
                    $"¿Borrar el invernadero {NombreInvernadero}?",
                    "Sí", "No");

                if (confirmar)
                {
                    await BorrarInvernadero();
                }
            });
        }





        private async Task BorrarInvernadero()
        {
            try
            {
                // Ejemplo de llamada a API para borrar por nombre
                var response = await new HttpClient().DeleteAsync(
                    $"https://tuapi.com/api/Invernadero/BorrarInvernadero?nombre={Uri.EscapeDataString(NombreInvernadero)}");

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Invernadero borrado", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync(); // Regresa a la lista
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
