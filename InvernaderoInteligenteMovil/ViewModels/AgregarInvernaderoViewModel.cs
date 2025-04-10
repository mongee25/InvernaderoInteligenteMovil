using InvernaderoInteligenteMovil.Clases;
using InvernaderoInteligenteMovil.Models;
using InvernaderoInteligenteMovil.Views;
using Microsoft.Maui.Controls;
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
        private const string ApiUrlAgregarInvernadero = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/Invernadero/AgregarInvernadero";
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
                if (string.IsNullOrWhiteSpace(_nombre))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El nombre del invernadero es requerido", "OK");
                    return;
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
                    MaxHumedad = ParseDecimal(_maxHumedad),
                    Usuarios = ConvertirStringALista(_usuarios),
                    Sensores = ConvertirStringALista(_sensores)
                };

                var json = JsonSerializer.Serialize(NuevoInvernadero, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var response = await _httpclient.PostAsync(ApiUrlAgregarInvernadero, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Invernadero agregado", "OK");
                    LimpiarCampos();
                    await _pantallaPrincipalViewModel.CargarInvernaderos();
                    Application.Current.MainPage = new NavigationPage(new PantallaPrincipal());

                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Error", $"Error en la API: {errorContent}", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        private List<string> ConvertirStringALista(string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? new List<string>()
                : input.Split(',')
                      .Select(item => item.Trim())
                      .Where(item => !string.IsNullOrWhiteSpace(item))
                      .Distinct()
                      .ToList();
        }

        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            value = value.Replace(",", ".");
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result)
                ? result
                : 0;
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
            Usuarios = string.Empty;
            Sensores = string.Empty;
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
