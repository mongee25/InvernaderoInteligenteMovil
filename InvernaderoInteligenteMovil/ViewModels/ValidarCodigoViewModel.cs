using InvernaderoInteligenteMovil.Clases;
using InvernaderoInteligenteMovil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using InvernaderoInteligenteMovil.Views;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class ValidarCodigoViewModel : BaseViewModel
    {
        private readonly HttpClient _httpclient;
        private const string ApiUrl = "https://3j8hk6ww-5148.usw3.devtunnels.ms//api/Usuario/ValidarCodigo";
        private const string ApiUrlEnviarCodigo = "https://3j8hk6ww-5148.usw3.devtunnels.ms/api/Usuario/EnviarCodigo";

        private string _codigo;
        private string _mensajeError;
        private string _mensajeExito;
        private readonly INavigation _navigation;
        private string _token;
        private string _email;



        public string Codigoo
        {
            get { return _codigo; }
            set
            {
                if (_codigo != value)
                {
                    _codigo = value;
                    OnPropertyChanged(nameof(_codigo));
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


        public ICommand ValidarCodigoCommand { get; }
        public ICommand ReenviarCodigoCommand { get; }
        public ICommand IrLoginCommand { get; }


        public ValidarCodigoViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _httpclient = new HttpClient();
            ValidarCodigoCommand = new Command(async () => await ValidarCodigo());
            ReenviarCodigoCommand = new Command(async () => await ReenviarCodigo());
            IrLoginCommand = new Command(async () => await IrALogin());
            CargarToken();
        }

        public async void CargarToken()
        {
            _token = await RecoveryService.ObtenerRecoveryToken();

            if (!string.IsNullOrEmpty(_token))
            {
                // Extraer el email del token
                _email = RecoveryService.ObtenerEmailDesdeToken(_token);
                if (!string.IsNullOrEmpty(_email))
                {
                    Console.WriteLine($"Email extraído: {_email}");
                }
                else
                {
                    Console.WriteLine("No se encontró el email en el token.");
                }
            }
            else
            {
                Console.WriteLine("No se encontró el token.");
            }
        }


        private async Task ValidarCodigo()
        {
            if (string.IsNullOrEmpty(_token))
            {
                MensajeError = "No se encontró un token de recuperación. Intente nuevamente.";
                return;
            }


            var CodigoDto = new ValidarCodigoDTO
            {
                Email = _email,
                Codigo = Codigoo
            };


            var json = JsonSerializer.Serialize(CodigoDto);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpclient.PostAsync($"{ApiUrl}?token={_token}", contenido);

                if (response.IsSuccessStatusCode)
                {

                    string NuevoToken = await response.Content.ReadAsStringAsync();
                    await RecoveryService.GuardarRecoveryToken(NuevoToken);

                    await _navigation.PushAsync(new NuevaContrasena());
                }
                else
                {
                    MensajeError = "Código inválido. Intenta de nuevo.";
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"{ex.Message}", "Ok");
            }

        }


        private async Task ReenviarCodigo()
        {
            if (string.IsNullOrEmpty(_email))
            {
                MensajeError = "No se encontró el email. Intente nuevamente";
            }

            var dto = new RecuperarContrasenaEmailDTO { Email = _email };
            var json = JsonSerializer.Serialize(dto);
            var Contenido = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var Response = await _httpclient.PostAsync(ApiUrlEnviarCodigo, Contenido);

                if (Response.IsSuccessStatusCode)
                {
                    string NuevoToken = await Response.Content.ReadAsStringAsync();
                    await RecoveryService.GuardarRecoveryToken(NuevoToken);
                    MensajeExito = "Nuevo código enviado con éxito. Revisa tu correo.";
                }
                else
                {
                    MensajeError = "Error al reenviar el código. Intenta de nuevo.";
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error: {ex.Message}";
            }

        }

        private async Task IrALogin()
        {
            Application.Current.MainPage = new NavigationPage(new InicioSesion());
        }
    }
}
