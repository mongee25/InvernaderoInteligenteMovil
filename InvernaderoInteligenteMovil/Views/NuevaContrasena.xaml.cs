using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class NuevaContrasena : ContentPage
{
	public NuevaContrasena()
	{
		InitializeComponent();
        NuevaContasenaViewModel viewModel = new NuevaContasenaViewModel();
        BindingContext = viewModel;
    }
}