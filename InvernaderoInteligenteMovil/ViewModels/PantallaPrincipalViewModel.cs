using InvernaderoInteligenteMovil.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InvernaderoInteligenteMovil.Views;
using InvernaderoInteligenteMovil.Models;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Globalization;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class PantallaPrincipalViewModel : BaseViewModel
    {
        private readonly HttpClient _httpclient;
        public const string ApiUrlConsultarInvernaderos = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/Invernadero/ListarInvernaderos";

        private string _nombreCompleto;
        private bool _isRefreshing;
        private ObservableCollection<InvernaderoModel> _invernaderos;
        private INavigation _navigation;


        public string FechaActual
        {
            get
            {
                // 1. Obtener fecha ACTUAL con zona horaria correcta
                DateTime fechaCorrecta;

                // Opción A: Si estás en México (CDMX)
                fechaCorrecta = DateTime.UtcNow.AddHours(-6); // UTC-6 (horario estándar)

                // Opción B: Para cualquier zona (recomendado)
                // var zona = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City");
                // fechaCorrecta = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zona);

                // 2. Configurar cultura en español con nombres de días exactos
                var culture = new CultureInfo("es-ES");
                culture.DateTimeFormat.DayNames = new[]
                {
            "domingo",
            "lunes",
            "martes",
            "miércoles",
            "jueves",
            "viernes",
            "sábado" // <- Aseguramos acento correcto
        };

                // 3. Formatear con día en minúscula y dos dígitos para el día
                return fechaCorrecta.ToString("dddd, dd MMMM yyyy", culture)
                            .ToLower(); // Para asegurar "sábado" en minúscula
            }
        }
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


        public ObservableCollection<InvernaderoModel> Invernaderos
        {
            get { return _invernaderos; }
            set
            {
                if (_invernaderos != value)
                {
                    _invernaderos = value;
                    OnPropertyChanged(nameof(Invernaderos)); // Se usa el nombre de la propiedad, no el campo
                }
            }
        }


        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }



        public ICommand AgregarInvernaderoCommand { get; }
        public ICommand RefreshCommand { get; }

        public ICommand VerDetallesCommand { get; set; }



        public PantallaPrincipalViewModel(INavigation navigation)
        {
            _httpclient = new HttpClient();
            AgregarInvernaderoCommand = new Command(async () => await AgregarInvernadero());
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
            _navigation = navigation;
            CargarNombre();
            VerDetallesCommand = new Command<InvernaderoModel>(async (invernadero) =>
            {
                if (invernadero != null && !string.IsNullOrEmpty(invernadero.Nombre))
                {
                    // Pasa SOLO el nombre a la página de detalles
                    await _navigation.PushAsync(new DetalleInvernadero(invernadero.Nombre));
                }
            });
        }



        private async void CargarNombre()
        {
            try
            {
                var Token = await AuthService.ObtenerToken();
                if (!string.IsNullOrEmpty(Token))
                {
                    var Handler = new JwtSecurityTokenHandler();
                    var JsonToken = Handler.ReadToken(Token) as JwtSecurityToken;
                    NombreCompleto = JsonToken?.Claims.FirstOrDefault(n => n.Type == "NombreCompleto")?.Value ?? "Usuario";
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", $"{ex.Message}", "Ok");
            }
        }


        public async Task ExecuteRefreshCommand()
        {
            IsRefreshing = true;
            try
            {
                await CargarInvernaderos();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public async Task CargarInvernaderos()
        {
            try
            {
                var response = await _httpclient.GetAsync(ApiUrlConsultarInvernaderos);
                if (response.IsSuccessStatusCode)
                {
                    var invernaderos = await response.Content.ReadFromJsonAsync<List<InvernaderoModel>>();
                    Invernaderos = new ObservableCollection<InvernaderoModel>(invernaderos ?? new List<InvernaderoModel>());
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar invernaderos: {ex.Message}", "OK");
            }
        }


        private async Task AgregarInvernadero()
        {
            await _navigation.PushAsync(new AgregarInvernadero(this));
        }
    }
}
