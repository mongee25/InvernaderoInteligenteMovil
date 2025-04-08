using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class PantallaPrincipal : ContentPage
{
	public PantallaPrincipal()
	{
		InitializeComponent();
        BindingContext = new PantallaPrincipalViewModel(Navigation);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PantallaPrincipalViewModel viewModel)
        {
            await viewModel.ExecuteRefreshCommand(); // Usa el nuevo método de refresh
        }
    }
}