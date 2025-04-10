using System;
using InvernaderoInteligenteMovil.Clases;
using InvernaderoInteligenteMovil.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using InvernaderoInteligenteMovil.Views;
using System.Text.RegularExpressions;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class CrearCuentaViewModel : BaseViewModel
    {
        private readonly HttpClient _httpclient;
        private const string ApiurlRegistro = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/Usuario/RegistrarUsuario";
        private const string ApiUrlLogin = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/Usuario/Login";
        private string _nombreCompleto;
        private string _email;
        private string _contrasena;
        private string _confirmarContrasena;
        private string _errorMessage;
        private bool _isVisibleErrorMessage;
        private INavigation _navigation;

        public string NombreCompleto
        {
            get { return _nombreCompleto; }
            set
            {
                if (_nombreCompleto != value)
                {
                    _nombreCompleto = value;
                    OnPropertyChanged(nameof(NombreCompleto));
                }
            }
        }

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
            get { return _contrasena; }
            set
            {
                if (_contrasena != value)
                {
                    _contrasena = value;
                    OnPropertyChanged(nameof(Contrasena));
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
                    OnPropertyChanged(nameof(ConfirmarContrasena));
                }
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public bool IsVisibleErrorMessage
        {
            get { return _isVisibleErrorMessage; }
            set
            {
                if (_isVisibleErrorMessage != value)
                {
                    _isVisibleErrorMessage = value;
                    OnPropertyChanged(nameof(IsVisibleErrorMessage));
                }
            }
        }

        public ICommand CrearCuentaCommand { get; }
        public ICommand IrLoginCommand { get; }

        public CrearCuentaViewModel(INavigation navigation)
        {
            _httpclient = new HttpClient();
            _navigation = navigation;
            CrearCuentaCommand = new Command(async () => await CrearCuenta());
            IrLoginCommand = new Command(async () => await IrLogin());
        }

        public async Task CrearCuenta()
        {
            // Validar que todos los campos estén completos
            if (string.IsNullOrWhiteSpace(NombreCompleto) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Contrasena) || string.IsNullOrWhiteSpace(ConfirmarContrasena))
            {
                ErrorMessage = "Por favor, completa todos los campos.";
                IsVisibleErrorMessage = true;
                return;
            }

            // Validar formato de email
            if (!IsValidEmail(Email))
            {
                ErrorMessage = "El email no es válido.";
                IsVisibleErrorMessage = true;
                return;
            }

            // Verificar si el email ya está registrado
            if (await IsEmailRegistered(Email))
            {
                ErrorMessage = "El correo electrónico ya está registrado.";
                IsVisibleErrorMessage = true;
                return;
            }

            // Validar que las contraseñas coincidan
            if (Contrasena != ConfirmarContrasena)
            {
                ErrorMessage = "Las contraseñas no coinciden.";
                IsVisibleErrorMessage = true;
                return;
            }

            // Validar la contraseña (mínimo 6 caracteres, una mayúscula, una minúscula y un número)
            if (!IsValidPassword(Contrasena))
            {
                ErrorMessage = "La contraseña debe tener al menos 6 caracteres, incluir una letra mayúscula, una minúscula y un número.";
                IsVisibleErrorMessage = true;
                return;
            }

            var Usuario = new CrearCuentaDTO
            {
                NombreCompleto = _nombreCompleto,
                Email = _email,
                Contrasena = _contrasena,
            };

            var Json = JsonSerializer.Serialize(Usuario);
            var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
            var Respuesta = await _httpclient.PostAsync(ApiurlRegistro, Contenido);

            if (Respuesta.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Exito", "Usuario registrado correctamente", "Ok");
                await Loguearse();
            }
            else
            {
                var error = await Respuesta.Content.ReadAsStringAsync();
                ErrorMessage = $"No se pudo registrar el usuario: {error}";
                IsVisibleErrorMessage = true;
            }
        }

        private async Task Loguearse()
        {
            var Login = new LoginDTO
            {
                Email = this._email,
                Contrasena = this._contrasena
            };

            var Json = JsonSerializer.Serialize(Login);
            var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
            var Response = await _httpclient.PostAsync(ApiUrlLogin, Contenido);

            if (Response.IsSuccessStatusCode)
            {
                var ResponseContent = await Response.Content.ReadAsStringAsync();
                var JsonDoc = JsonDocument.Parse(ResponseContent);
                string Token = JsonDoc.RootElement.GetProperty("token").GetString();

                await AuthService.GuardarToken(Token);

                Application.Current.MainPage = new NavigationPage(new PantallaPrincipal());
            }
            else
            {
                var error = await Response.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert("Login Fallido", $"Error al iniciar sesión: {error}", "OK");
            }
        }

        private async Task IrLogin()
        {
            await _navigation.PushAsync(new InicioSesion());
        }

        // Método para validar el formato del email
        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Método para validar la contraseña
        private bool IsValidPassword(string password)
        {
            // Al menos 6 caracteres, una mayúscula, una minúscula y un número
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{6,}$";
            return Regex.IsMatch(password, passwordPattern);
        }

        // Método para verificar si el email ya está registrado
        private async Task<bool> IsEmailRegistered(string email)
        {
            var response = await _httpclient.GetAsync($"https://z7zsd20t-5148.usw3.devtunnels.ms/api/Usuario/CheckEmail?email={email}");

            // Si la respuesta es exitosa y el email está registrado, devolver true
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result == "true"; // Asumimos que la API devuelve "true" si el email está registrado
            }

            return false;
        }
    }
}
