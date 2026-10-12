
using SineaceAPP.Pages;
using SineaceAPP.Pages.Guia;
using SineaceAPP.Pages.Informativo;
using SineaceAPP.Pages.Inicio;
using SineaceAPP.Pages.Instituciones;
using SineaceAPP.Pages.Mapa;

namespace SineaceAPP;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new PantallaIncio1();
		//MainPage = new Contactanos();
	}
}
