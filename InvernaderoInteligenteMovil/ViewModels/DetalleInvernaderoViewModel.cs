
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace InvernaderoInteligenteMovil.ViewModels {
  public class DetalleInvernaderoViewModel : BaseViewModel {
    private readonly HttpClient _httpClient;
    private const string ApiURL = "https://z7zsd20t-5148.usw3.devtunnels.ms/api/Sensor/estado"; // URL para cambiar estado

    private string _sensorId; // El ID del sensor
    private bool _estadoSensor; // El estado del sensor (true o false)
    private string _mensajeError;
    private string _eyeIcon = "https://imgfz.com/i/Hp3T2fO.png"; // Icono de ojo cerrado
    private readonly INavigation _navigation;

    public string SensorId {
      get { return _sensorId; }
      set {
        if (_sensorId != value) {
          _sensorId = value;
          OnPropertyChanged (nameof (SensorId));
        }
      }
    }

        public bool EstadoSensor
        {
            get { return _estadoSensor; }
            set
            {
                if (_estadoSensor != value)
                {
                    _estadoSensor = value;
                    OnPropertyChanged(nameof(EstadoSensor));
                    _ = CambiarEstadoSensor(); // Llama al método cuando cambia
                }
            }
        }

        public string MensajeError {
      get => _mensajeError;
      set => SetProperty (ref _mensajeError, value);
    }

    public ICommand CambiarEstadoSensorCommand { get; }

    public DetalleInvernaderoViewModel (INavigation navigation) {
      _httpClient = new HttpClient ();
      CambiarEstadoSensorCommand = new Command (async () => await CambiarEstadoSensor ());
      _navigation = navigation;
      SensorId = "67f45bceaa2788f794bb2105";
        }

   
    private async Task CambiarEstadoSensor () {
      MensajeError = string.Empty;

    
      if (string.IsNullOrEmpty (SensorId)) {
        MensajeError = "El ID del sensor no puede estar vacío.";
        return;
      }

    
      var dto = new CambiarEstadoSensorDTO {
        SensorId = SensorId,
        Estado = EstadoSensor
      };
            
      var json = JsonSerializer.Serialize (dto);
      var contenido = new StringContent (json, Encoding.UTF8, "application/json");

      try {
      
        var response = await _httpClient.PutAsync (ApiURL, contenido);

        if (response.IsSuccessStatusCode) {
         
          await Application.Current.MainPage.DisplayAlert ("Éxito", "Estado del sensor actualizado correctamente.", "OK");
        } else {
          
          await Application.Current.MainPage.DisplayAlert ("Error", "No se pudo cambiar el estado del sensor.", "OK");
        }
      } catch (Exception ex) {
      
        MensajeError = "Error al realizar la solicitud: " + ex.Message;
      }
    }
  }

  // DTO para el PUT
  public class CambiarEstadoSensorDTO {
    public string SensorId { get; set; }
    public bool Estado { get; set; }
  }
}

