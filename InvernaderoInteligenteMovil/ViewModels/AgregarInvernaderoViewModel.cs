using InvernaderoInteligenteMovil.Clases;
using InvernaderoInteligenteMovil.Models;
using InvernaderoInteligenteMovil.Views;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class AgregarInvernaderoViewModel : BaseViewModel
    {
        private readonly HttpClient _httpclient;
        private const string ApiUrlAgregarInvernadero = "https://3j8hk6ww-5148.usw3.devtunnels.ms/api/Invernadero/CrearInvernadero";
        private readonly PantallaPrincipalViewModel _pantallaPrincipalViewModel;

        private string _nombre;
        private string _nombrePlanta;
        private string _tipoPlanta;
        private string _imagen;
        private string _minTemperatura;
        private string _maxTemperatura;
        private string _minHumedad;
        private string _maxHumedad;
        private string _usuarios;
        private string _sensores;

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        public string NombrePlanta
        {
            get => _nombrePlanta;
            set
            {
                if (_nombrePlanta != value)
                {
                    _nombrePlanta = value;
                    OnPropertyChanged(nameof(NombrePlanta));
                }
            }
        }

        public string TipoPlanta
        {
            get => _tipoPlanta;
            set
            {
                if (_tipoPlanta != value)
                {
                    _tipoPlanta = value;
                    OnPropertyChanged(nameof(TipoPlanta));
                }
            }
        }

        public string Imagen
        {
            get => _imagen;
            set
            {
                if (_imagen != value)
                {
                    _imagen = value;
                    OnPropertyChanged(nameof(Imagen));
                }
            }
        }

        public string MinTemperatura
        {
            get => _minTemperatura;
            set
            {
                if (_minTemperatura != value)
                {
                    _minTemperatura = value;
                    OnPropertyChanged(nameof(MinTemperatura));
                }
            }
        }

        public string MaxTemperatura
        {
            get => _maxTemperatura;
            set
            {
                if (_maxTemperatura != value)
                {
                    _maxTemperatura = value;
                    OnPropertyChanged(nameof(MaxTemperatura));
                }
            }
        }

        public string MinHumedad
        {
            get => _minHumedad;
            set
            {
                if (_minHumedad != value)
                {
                    _minHumedad = value;
                    OnPropertyChanged(nameof(MinHumedad));
                }
            }
        }

        public string MaxHumedad
        {
            get => _maxHumedad;
            set
            {
                if (_maxHumedad != value)
                {
                    _maxHumedad = value;
                    OnPropertyChanged(nameof(MaxHumedad));
                }
            }
        }

        public ICommand AgregarInvernaderoCommand { get; }

        public AgregarInvernaderoViewModel(PantallaPrincipalViewModel pantallaPrincipalViewModel)
        {
            _httpclient = new HttpClient();
            AgregarInvernaderoCommand = new Command(async () => await AgregarInvernadero());
            _pantallaPrincipalViewModel = pantallaPrincipalViewModel;
        }

        private async Task AgregarInvernadero()
        {
            try
            {
                decimal ParseDecimal(string value)
                {
                    if (string.IsNullOrEmpty(value)) return 0;

                    // Reemplaza comas por puntos para un parsing consistente
                    value = value.Replace(",", ".");

                    if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                        return result;

                    return 0; // Valor por defecto si el parsing falla
                }

                var NuevoInvernadero = new InvernaderoModel
                {
                    Nombre = _nombre,
                    NombrePlanta = _nombrePlanta,
                    TipoPlanta = _tipoPlanta,
                    Imagen = _imagen,
                    MinTemperatura = ParseDecimal(_minTemperatura),
                    MaxTemperatura = ParseDecimal(_maxTemperatura),
                    MinHumedad = ParseDecimal(_minHumedad),
                    MaxHumedad = ParseDecimal(_maxHumedad)
                };

                // Configuración para asegurar que el JSON use puntos
                var options = new JsonSerializerOptions
                {
                    NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };


                var Json = JsonSerializer.Serialize(NuevoInvernadero, options);
                Console.WriteLine($"JSON enviado: {Json}"); // Para depuración
                var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");

                var Response = await _httpclient.PostAsync(ApiUrlAgregarInvernadero, Contenido);
                if (Response.IsSuccessStatusCode)
                {
                    Application.Current.MainPage.DisplayAlert("Éxito", "Invernadero agregado", "OK");
                    LimpiarCampos();
                    await _pantallaPrincipalViewModel.CargarInvernaderos();
                    Application.Current.MainPage = new NavigationPage(new PantallaPrincipal());

                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Fallo en la API", "OK");
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void LimpiarCampos()
        {
            Nombre = string.Empty;
            NombrePlanta = string.Empty;
            TipoPlanta = string.Empty;
            Imagen = string.Empty;
            MinTemperatura = string.Empty;
            MaxTemperatura = string.Empty;
            MinHumedad = string.Empty;
            MaxHumedad = string.Empty;
        }

        public string Usuarios
        {
            get => _usuarios;
            set
            {
                if (_usuarios != value)
                {
                    _usuarios = value;
                    OnPropertyChanged(nameof(Usuarios));
                    ValidarUsuarios();
                }
            }
        }

        public string Sensores
        {
            get => _sensores;
            set
            {
                if (_sensores != value)
                {
                    _sensores = value;
                    OnPropertyChanged(nameof(Sensores));
                    ValidarSensores();
                }
            }
        }

        private void ValidarUsuarios()
        {
            // Dividir los usuarios por coma y quitar espacios vacíos
            var usuariosList = _usuarios.Split(',')
                                          .Select(u => u.Trim())
                                          .Where(u => !string.IsNullOrEmpty(u))
                                          .Distinct()
                                          .ToList();

            // Volver a asignar la lista única de usuarios
            _usuarios = string.Join(", ", usuariosList);
        }

        private void ValidarSensores()
        {
            // Dividir los sensores por coma y quitar espacios vacíos
            var sensoresList = _sensores.Split(',')
                                        .Select(s => s.Trim())
                                        .Where(s => !string.IsNullOrEmpty(s))
                                        .Distinct()
                                        .ToList();

            // Volver a asignar la lista única de sensores
            _sensores = string.Join(", ", sensoresList);
        }
    }
}
