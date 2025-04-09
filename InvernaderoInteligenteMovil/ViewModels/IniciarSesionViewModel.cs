using Microsoft.Maui.ApplicationModel.Communication;
using InvernaderoInteligenteMovil.Models;
using InvernaderoInteligenteMovil.Views;
using InvernaderoInteligenteMovil.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class InicioSesionViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private const string ApiURL = "https://3j8hk6ww-5148.usw3.devtunnels.ms/api/Usuario/Login";

        private string _email;
        private string _contrasena;
        private bool _isPasswordVisible = true;
        private string _mensajeError;
        private string _eyeIcon = "https://imgfz.com/i/Hp3T2fO.png"; // Icono de ojo cerrado
        private readonly INavigation _navigation;
        private string _Msgemail;
        private string _Msgcontrasena;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Contrasena
        {
            get => _contrasena;
            set => SetProperty(ref _contrasena, value);
        }

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged(nameof(IsPasswordVisible));
                EyeIcon = _isPasswordVisible ? "https://imgfz.com/i/iDCskH9.png" : "https://imgfz.com/i/Hp3T2fO.png";
            }
        }

        public string EyeIcon
        {
            get => _eyeIcon;
            set
            {
                _eyeIcon = value;
                OnPropertyChanged(nameof(EyeIcon));
            }
        }

        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordCommand { get; }
        public ICommand RecuperarContrasenaCommand { get; }
        public ICommand CrearCuentaCommand { get; }

        public ICommand ValidacionCommand { get; }


        public InicioSesionViewModel(INavigation Navigation)
        {
            _httpClient = new HttpClient();
            LoginCommand = new Command(async () => await Login());
            TogglePasswordCommand = new Command(TogglePasswordVisibility);
            RecuperarContrasenaCommand = new Command(async () => await NavegarARecuperarContrasena());
            CrearCuentaCommand = new Command(async () => await NavegarACrearCuenta());
            ValidacionCommand = new Command(async () => await Validaciones());
            _navigation = Navigation;

        }

        private async Task Login()
        {
            MensajeError = string.Empty;

            await Validaciones();

            if (!string.IsNullOrEmpty(MensajeError))
            {
                return; // Si hay error, no se ejecuta la llamada a la API
            }


            var datosLogin = new LoginDTO
            {
                Email = Email,
                Contrasena = Contrasena
            };

            var json = JsonSerializer.Serialize(datosLogin);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(ApiURL, contenido);


                if (response.IsSuccessStatusCode)
                {
                    string Token = await response.Content.ReadAsStringAsync();
                    await AuthService.GuardarToken(Token);

                    // Insertamos la PantallaPrincipal antes de la página de inicio de sesión
                    var currentPage = Application.Current.MainPage.Navigation.NavigationStack.FirstOrDefault();
                    if (currentPage != null)
                    {
                        _navigation.InsertPageBefore(new PantallaPrincipal(), currentPage);
                    }

                    // Eliminamos todas las páginas anteriores (en este caso la de inicio de sesión)
                    await _navigation.PopToRootAsync();
                }


                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Credenciales Incorrectas, favor de validar", "Ok");
                }
            }
            catch (Exception ex)
            {
                MensajeError += ex.Message;
            }
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
            EyeIcon = IsPasswordVisible
                ? "http://imgfz.com/i/iDCskH9.png"  // Ojo abierto
                : "http://imgfz.com/i/Hp3T2fO.png"; // Ojo cerrado 
        }

        public async Task NavegarARecuperarContrasena()
        {
            await _navigation.PushAsync(new RecuperarContrasena());
        }

        public async Task NavegarACrearCuenta()
        {
            await _navigation.PushAsync(new CrearCuenta());
        }


        public async Task Validaciones()
        {
            MensajeError = string.Empty;
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
            {
                MensajeError = "Todos los campos son obligatorios.";
                return;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, emailPattern))
            {
                MensajeError = "El email debe ser un correo válido.";
                return;
            }
        }
    }
}
