using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernaderoInteligenteMovil.Models {
  public class InvernaderoModel {

    public string? InvernaderoId { get; set; }
    public string Nombre { get; set; }
    public string NombrePlanta { get; set; }
    public string TipoPlanta { get; set; }
    public string Imagen { get; set; }
    public decimal MinTemperatura { get; set; }
    public decimal MaxTemperatura { get; set; }
    public decimal MinHumedad { get; set; }
    public decimal MaxHumedad { get; set; }
    public List<string>? Usuarios { get; set; } = new List<string> ();
    public List<string>? Sensores { get; set; } = new List<string> ();

  }
}
