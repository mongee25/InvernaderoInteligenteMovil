using InvernaderoInteligenteMovil.ViewModels;

namespace InvernaderoInteligenteMovil.Views;

public partial class IngresarCodigo : ContentPage
{
	public IngresarCodigo()
	{
		InitializeComponent();
        ValidarCodigoViewModel validarCodigo = new ValidarCodigoViewModel(Navigation);
        BindingContext = validarCodigo;
        AsignarEventos();
    }

    private void AsignarEventos()
    {
        Code1.TextChanged += (s, e) => { ErrorLabel.IsVisible = false; MoverFoco(Code1, Code2); };
        Code2.TextChanged += (s, e) => { ErrorLabel.IsVisible = false; MoverFoco(Code2, Code3); };
        Code3.TextChanged += (s, e) => { ErrorLabel.IsVisible = false; MoverFoco(Code3, Code4); };
        Code4.TextChanged += (s, e) => { ErrorLabel.IsVisible = false; MoverFoco(Code4, Code5); };
        Code5.TextChanged += (s, e) => { ErrorLabel.IsVisible = false; MoverFoco(Code5, Code6); };
        Code6.TextChanged += (s, e) => { ErrorLabel.IsVisible = false; };
    }

    private void MoverFoco(Entry actual, Entry siguiente)
    {
        if (!string.IsNullOrEmpty(actual.Text))
        {
            siguiente.Focus();
        }
    }

    private async void ConfirmarCodigo_Clicked(object sender, EventArgs e)
    {
        // Concatenamos el c�digo
        string codigo = Code1.Text + Code2.Text + Code3.Text + Code4.Text + Code5.Text + Code6.Text;

        if (string.IsNullOrEmpty(codigo) || codigo.Length < 6)
        {
            // Si no se ha completado el c�digo, mostramos un error
            ErrorLabel.Text = "Por favor ingresa un c�digo completo.";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Llamamos al VM para validar el c�digo
        var viewModel = (ValidarCodigoViewModel)BindingContext;
        viewModel.Codigoo = codigo; // Asignamos el c�digo al ViewModel

        // Ejecutamos el comando de validaci�n (sin ExecuteAsync)
        viewModel.ValidarCodigoCommand.Execute(null);  // Usamos Execute() sin Async
    }
}