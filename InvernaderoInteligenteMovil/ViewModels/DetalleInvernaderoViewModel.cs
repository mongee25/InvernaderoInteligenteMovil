
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace InvernaderoInteligenteMovil.ViewModels {
  public class DetalleInvernaderoViewModel : BaseViewModel {
    private readonly HttpClient _httpClient;
    private const string ApiURL = "https://3j8hk6ww-5148.usw3.devtunnels.ms/api/Sensor/estado"; // URL para cambiar estado

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

    public bool EstadoSensor {
      get { return _estadoSensor; }
      set {
        if (_estadoSensor != value) {
          _estadoSensor = value;
          OnPropertyChanged (nameof (EstadoSensor));
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
    }

    // Método para cambiar el estado del sensor
    private async Task CambiarEstadoSensor () {
      MensajeError = string.Empty;

      // Validar que SensorId y EstadoSensor no estén vacíos
      if (string.IsNullOrEmpty (SensorId)) {
        MensajeError = "El ID del sensor no puede estar vacío.";
        return;
      }

      // Construir el objeto DTO con los datos necesarios
      var dto = new CambiarEstadoSensorDTO {
        SensorId = "67f45bceaa2788f794bb2105",
        Estado = EstadoSensor
      };

      var json = JsonSerializer.Serialize (dto);
      var contenido = new StringContent (json, Encoding.UTF8, "application/json");

      try {
        // Realizar la solicitud PUT a la API para cambiar el estado del sensor
        var response = await _httpClient.PutAsync (ApiURL, contenido);

        if (response.IsSuccessStatusCode) {
          // Si la solicitud es exitosa, puedes hacer algo más, por ejemplo, navegar o mostrar un mensaje
          await Application.Current.MainPage.DisplayAlert ("Éxito", "Estado del sensor actualizado correctamente.", "OK");
        } else {
          // Si la respuesta no es exitosa, mostrar el mensaje de error
          await Application.Current.MainPage.DisplayAlert ("Error", "No se pudo cambiar el estado del sensor.", "OK");
        }
      } catch (Exception ex) {
        // Manejo de errores en caso de fallos de red o problemas con la API
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

