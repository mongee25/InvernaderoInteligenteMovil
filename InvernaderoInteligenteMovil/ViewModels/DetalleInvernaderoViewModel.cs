using InvernaderoInteligenteMovil.Clases;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using InvernaderoInteligenteMovil.Models;

namespace InvernaderoInteligenteMovil.ViewModels
{
    public class DetalleInvernaderoViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private const string ApiURL = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/Sensor/estado";
        private const string LecturasApiURL = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/LecturaSensor/ObtenerLecturasConInvernadero";
        private bool _estadoSensorTemperatura; // El estado del sensor (true o false)
        private bool _estadoSensorHumedad;
        private string _mensajeError;
        private string _eyeIcon = "https://imgfz.com/i/Hp3T2fO.png"; // Icono de ojo cerrado
        private readonly INavigation _navigation;
        private string _sensorIdTemp;
        private string _sensorIdHum;
        private double _temperatura;
        private double _humedad;


        public bool EstadoSensorTemperatura
        {
            get { return _estadoSensorTemperatura; }
            set
            {
                if (_estadoSensorTemperatura != value)
                {
                    _estadoSensorTemperatura = value;
                    OnPropertyChanged(nameof(EstadoSensorTemperatura));
                    _ = CambiarEstadoTemperatura(); // Llama al método cuando cambia
                }
            }
        }



        public bool EstadoSensorHumedad
        {
            get { return _estadoSensorHumedad; }
            set
            {
                if (_estadoSensorHumedad != value)
                {
                    _estadoSensorHumedad = value;
                    OnPropertyChanged(nameof(EstadoSensorHumedad));
                    _ = CambiarEstadoHumedad(); // Llama al método cuando cambia
                }
            }
        }


        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }



        public string SensorIdTemp
        {
            get { return _sensorIdTemp; }
            set
            {
                if (_sensorIdTemp != value)
                {
                    _sensorIdTemp = value;
                    OnPropertyChanged(nameof(SensorIdTemp));
                   
                }
            }
        }

        public string SensorIdHum
        {
            get { return _sensorIdHum; }
            set
            {
                if (_sensorIdHum != value)
                {
                    _sensorIdHum = value;
                    OnPropertyChanged(nameof(SensorIdHum));
                   
                }
            }
        }


        public double Temperatura
        {
            get => _temperatura;
            set => SetProperty(ref _temperatura, value);
        }



        public double Humedad
        {
            get => _humedad;
            set => SetProperty(ref _humedad, value);
        }




        // Propiedades para el invernadero
        private InvernaderoModel _invernadero;
        public InvernaderoModel Invernadero
        {
            get => _invernadero;
            set => SetProperty(ref _invernadero, value);
        }

        public ICommand CambiarEstadoSensorTempCommand { get; }

        public ICommand CambiarEstadoSensorHumCommand { get; }



        public DetalleInvernaderoViewModel(InvernaderoModel invernadero, PantallaPrincipalViewModel pantalla, INavigation navigation)
        {
            _httpClient = new HttpClient();
            CambiarEstadoSensorTempCommand = new Command(async () => await CambiarEstadoTemperatura());
            CambiarEstadoSensorHumCommand = new Command(async () => await CambiarEstadoHumedad());
            _navigation = navigation;
            SensorIdTemp = "67f45bceaa2788f794bb2105";
            SensorIdHum = "67f45be3aa2788f794bb2106";
            Invernadero = invernadero;

        }


        private async Task CambiarEstadoTemperatura()
        {
            MensajeError = string.Empty;


            if (string.IsNullOrEmpty(SensorIdTemp))
            {
                MensajeError = "El ID del sensor no puede estar vacío.";
                return;
            }


            var dto = new CambiarEstadoSensorDTO
            {
                SensorId = SensorIdTemp,
                Estado = EstadoSensorTemperatura
            };

            var json = JsonSerializer.Serialize(dto);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {

                var response = await _httpClient.PutAsync(ApiURL, contenido);

                if (response.IsSuccessStatusCode)
                {

                    await Application.Current.MainPage.DisplayAlert("Éxito", "Estado del sensor actualizado correctamente.", "OK");
                }
                else
                {

                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cambiar el estado del sensor.", "OK");
                }
            }
            catch (Exception ex)
            {

                MensajeError = "Error al realizar la solicitud: " + ex.Message;
            }
        }




        private async Task CambiarEstadoHumedad()
        {
            MensajeError = string.Empty;

            if (string.IsNullOrEmpty(SensorIdHum))
            {
                MensajeError = "El ID del sensor de humedad no puede estar vacío.";
                return;
            }

            var dto = new CambiarEstadoSensorDTO
            {
                SensorId = SensorIdHum,
                Estado = EstadoSensorHumedad // Estado de humedad en lugar de temperatura
            };

            var json = JsonSerializer.Serialize(dto);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync(ApiURL, contenido);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Estado del sensor de humedad actualizado correctamente.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cambiar el estado del sensor de humedad.", "OK");
                }
            }
            catch (Exception ex)
            {
                MensajeError = "Error al realizar la solicitud: " + ex.Message;
            }
        }







    }
}


