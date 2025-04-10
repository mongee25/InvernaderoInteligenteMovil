using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using InvernaderoInteligenteMovil.Clases;
using InvernaderoInteligenteMovil.Models;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class EditarInvernaderoViewModel
    {

        private readonly HttpClient _httpClient;
        private readonly PantallaPrincipalViewModel _pantallaPrincipalViewModel;

        public ICommand GuardarCambiosCommand { get; }

        public InvernaderoModel Invernadero { get; set; }

        public EditarInvernaderoViewModel(InvernaderoModel invernadero, PantallaPrincipalViewModel pantallaPrincipalViewModel)
        {
            _httpClient = new HttpClient();
            Invernadero = invernadero;
            _pantallaPrincipalViewModel = pantallaPrincipalViewModel;
            GuardarCambiosCommand = new Command(async () => await GuardarCambios());
        }

        private async Task GuardarCambios()
        {
            try
            {
                var json = JsonSerializer.Serialize(Invernadero);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("https://z7zsd20t-5148.usw3.devtunnels.ms/api/Invernadero/ActualizarInvernadero", content);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Invernadero actualizado correctamente.", "OK");

                    // Opcional: recargar los datos en la pantalla principal
                    await _pantallaPrincipalViewModel.CargarInvernaderos();

                    // Regresar a la pantalla anterior
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al actualizar el invernadero.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }
}
