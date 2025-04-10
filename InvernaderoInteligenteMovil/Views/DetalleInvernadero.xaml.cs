using InvernaderoInteligenteMovil.ViewModels;
using InvernaderoInteligenteMovil.Views;
using InvernaderoInteligenteMovil.Clases;
using InvernaderoInteligenteMovil.Models;

namespace InvernaderoInteligenteMovil.Views;

public partial class DetalleInvernadero : ContentPage
{
	public DetalleInvernadero(InvernaderoModel invernadero, PantallaPrincipalViewModel pantalla)
	{
		InitializeComponent();
		BindingContext = new DetalleInvernaderoViewModel(invernadero, pantalla, Navigation);
	}
}