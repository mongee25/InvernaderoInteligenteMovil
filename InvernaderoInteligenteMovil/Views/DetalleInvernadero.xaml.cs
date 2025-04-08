using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class DetalleInvernadero : ContentPage
{
	public DetalleInvernadero(string nombreInvernadero)
	{
		InitializeComponent();
        BindingContext = new DetalleInvernaderoViewModel(nombreInvernadero);
    }
}