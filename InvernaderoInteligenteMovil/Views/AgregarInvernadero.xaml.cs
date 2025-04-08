using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class AgregarInvernadero : ContentPage
{
	public AgregarInvernadero(PantallaPrincipalViewModel pantallaPrincipalViewModel)
	{
		InitializeComponent();
        AgregarInvernaderoViewModel viewModel = new AgregarInvernaderoViewModel(pantallaPrincipalViewModel);
        BindingContext = viewModel;
    }
}