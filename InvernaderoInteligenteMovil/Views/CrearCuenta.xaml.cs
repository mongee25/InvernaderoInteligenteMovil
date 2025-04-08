using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class CrearCuenta : ContentPage
{
	public CrearCuenta()
	{
		InitializeComponent();
        CrearCuentaViewModel viewModel = new CrearCuentaViewModel(Navigation);
        BindingContext = viewModel;
    }
}