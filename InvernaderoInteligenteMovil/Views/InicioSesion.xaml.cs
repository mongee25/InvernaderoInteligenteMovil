using InvernaderoInteligenteMovil.ViewModels;
using System.Globalization;

namespace InvernaderoInteligenteMovil.Views;

public partial class InicioSesion : ContentPage
{
	public InicioSesion()
	{
		InitializeComponent();
        InicioSesionViewModel viewModel = new InicioSesionViewModel(Navigation);
        BindingContext = viewModel;
      Resources.Add ("StringToVisibilityConverter", new StringToVisibilityConverter ());
  }


  private class StringToVisibilityConverter : IValueConverter {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is string str && !string.IsNullOrWhiteSpace (str))
        return true; // o Visibility.Visible si usas otro tipo

      return false; // o Visibility.Collapsed
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException ();
    }
  }
}