using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using InvernaderoInteligenteMovil.Models;
using InvernaderoInteligenteMovil.Views;
using InvernaderoInteligenteMovil.Clases;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class NuevaContasenaViewModel : BaseViewModel
    {
        private readonly HttpClient _httpclient;
        private const string ApiUrl = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/Usuario/CambiarContrasena-Email";


        private string _nuevaContrasena;
        private string _confirmarContrasena;
        private string _mensajeExito;
        private string _mensajeError;
        private string _email;





        public string NuevaContrasena
        {
            get { return _nuevaContrasena; }
            set
            {
                if (_nuevaContrasena != value)
                {
                    _nuevaContrasena = value;
                    OnPropertyChanged(nameof(_nuevaContrasena));
                }
            }
        }


        public string ConfirmarContrasena
        {
            get { return _confirmarContrasena; }
            set
            {
                if (_confirmarContrasena != value)
                {
                    _confirmarContrasena = value;
                    OnPropertyChanged(nameof(_confirmarContrasena));
                }
            }
        }


        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        public string MensajeExito
        {
            get => _mensajeExito;
            set => SetProperty(ref _mensajeExito, value);
        }





        public ICommand CambiarContrasenaCommand { get; }
        public ICommand IrLoginCommand { get; }

        public NuevaContasenaViewModel()
        {
            _httpclient = new HttpClient();
            CambiarContrasenaCommand = new Command(async () => await CambiarContrasena());
            IrLoginCommand = new Command(async () => await IrAlLogin());
            CargarEmailToken();
        }


        private async void CargarEmailToken()
        {
            var Token = await RecoveryService.ObtenerRecoveryToken();

            if (!string.IsNullOrEmpty(Token))
            {
                _email = RecoveryService.ObtenerEmailDesdeToken(Token);

                if (!string.IsNullOrEmpty(_email))
                {
                    _mensajeError = "No se pudo extraer el correo del token";
                }
            }
            else
            {
                _mensajeError = "No se encontro token almacenado";
            }
        }

        private async Task CambiarContrasena()
        {
            if (string.IsNullOrEmpty(NuevaContrasena))
            {
                MensajeError = "Por favor ingresa una nueva contraseña";
            }

            if (NuevaContrasena != ConfirmarContrasena)
            {
                MensajeError = "Las credenciales no coinciden";
                return;
            }


            var datos = new CambiarContrasenaDTO
            {
                Email = _email,
                Contrasena = NuevaContrasena
            };


            var Json = JsonSerializer.Serialize(datos);
            var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");

            try
            {
                var Response = await _httpclient.PostAsync(ApiUrl, Contenido);

                if (Response.IsSuccessStatusCode)
                {
                    MensajeExito = "Contrasena cambiada exitosamente";
                    await Task.Delay(2000);
                    Application.Current.MainPage = new NavigationPage(new InicioSesion());
                }
                else
                {
                    MensajeError = "No se pudo cambiar. Intente de nuevo";
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error: {ex.Message}";
            }
        }

        private async Task IrAlLogin()
        {
            Application.Current.MainPage = new NavigationPage(new InicioSesion());
        }
    }
}
