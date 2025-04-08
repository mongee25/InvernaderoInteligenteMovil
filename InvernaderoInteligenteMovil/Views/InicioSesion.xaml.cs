using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class InicioSesion : ContentPage
{
	public InicioSesion()
	{
		InitializeComponent();
        InicioSesionViewModel viewModel = new InicioSesionViewModel(Navigation);
        BindingContext = viewModel;
    }
}