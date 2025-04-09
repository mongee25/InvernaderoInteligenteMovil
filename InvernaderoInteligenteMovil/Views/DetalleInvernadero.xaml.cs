using InvernaderoInteligenteMovil.ViewModels;
using InvernaderoInteligenteMovil.Views;
namespace InvernaderoInteligenteMovil.Views;

public partial class DetalleInvernadero : ContentPage
{
	public DetalleInvernadero () {
		InitializeComponent ();
		DetalleInvernaderoViewModel viewmodel = new DetalleInvernaderoViewModel (Navigation);
		BindingContext = viewmodel;
	}
}