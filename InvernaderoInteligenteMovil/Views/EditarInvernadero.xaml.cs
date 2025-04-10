namespace InvernaderoInteligenteMovil.Views;
using InvernaderoInteligenteMovil.Models;
using InvernaderoInteligenteMovil.ViewModels;

public partial class EditarInvernadero : ContentPage
{
    public EditarInvernadero(InvernaderoModel invernadero, PantallaPrincipalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = new EditarInvernaderoViewModel(invernadero, viewModel);
    }
}