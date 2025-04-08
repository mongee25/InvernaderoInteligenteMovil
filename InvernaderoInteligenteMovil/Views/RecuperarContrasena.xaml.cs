using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class RecuperarContrasena : ContentPage
{
	public RecuperarContrasena()
	{
		InitializeComponent();
        RecuperarContrasenaViewModel viewModel = new RecuperarContrasenaViewModel(Navigation);
        BindingContext = viewModel;
    }
}