using InvernaderoInteligenteMovil.Views;

namespace InvernaderoInteligenteMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new InicioSesion());
        }
    }
}
