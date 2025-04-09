using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using InvernaderoInteligenteMovil.Clases;
using InvernaderoInteligenteMovil.Models;
using InvernaderoInteligenteMovil.Views;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class RecuperarContrasenaViewModel : BaseViewModel
    {
        private readonly HttpClient _httpclient;
        private const string ApiUrl = "https://3j8hk6ww-5148.usw3.devtunnels.ms/api/Usuario/EnviarCodigo";


        private string _email;
        private string _mensajeError;
        private string _mensajeExito;
        private readonly INavigation _navigation;


        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(_email));
                }
            }
        }

        public string MensajeExito
        {
            get => _mensajeExito;
            set => SetProperty(ref _mensajeExito, value);
        }

        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        public ICommand EnviarCodigoCommand { get; }
        public ICommand EnviarInicioSesion { get; }


        public RecuperarContrasenaViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _httpclient = new HttpClient();
            EnviarCodigoCommand = new Command(async () => await EnviarCodigoDeVerificacion());
            EnviarInicioSesion = new Command(async () => await NavegarAInicioSesion());
        }


        private async Task EnviarCodigoDeVerificacion()
        {

            var EmailUsuario = new RecuperarContrasenaEmailDTO
            {
                Email = _email
            };

            var Json = JsonSerializer.Serialize(EmailUsuario);
            var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");

            try
            {
                var Response = await _httpclient.PostAsync(ApiUrl, Contenido);

                if (Response.IsSuccessStatusCode)
                {
                    string Token = await Response.Content.ReadAsStringAsync();

                    await RecoveryService.GuardarRecoveryToken(Token);

                    MensajeExito = "Se ha enviado el código de verificacion a su correo electrónico";

                    await _navigation.PushAsync(new IngresarCodigo());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Hubo un error al enviar el codigo", "Ok");
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al enviar el código: {ex.Message}";
            }

        }

        public async Task NavegarAInicioSesion()
        {
            await _navigation.PushAsync(new InicioSesion());
        }

    }
}
