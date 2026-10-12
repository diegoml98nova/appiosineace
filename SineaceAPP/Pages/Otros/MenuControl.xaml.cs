using SineaceAPP.Pages.Informativo;
using SineaceAPP.Pages.Inicio;
using SineaceAPP.Pages.Instituciones;
namespace SineaceAPP.Pages.Otros;

public partial class MenuControl : ContentView
{
	public MenuControl()
	{
		InitializeComponent();
	}
    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.DisplayAlert("Opción en desarrollo", "Proximamente estará habilitada", "Aceptar");
    }
    private void MapaAcreditados(object sender, EventArgs e)
    {
        App.Current.MainPage = new AppShell();
    }
    private void MapaLicenciados(object sender, EventArgs e)
    {
        App.Current.MainPage = new AppShell2();
        //string path = Shell.Current?.CurrentState.Location.ToString();
        //string root = path.Split("/", StringSplitOptions.RemoveEmptyEntries)[0];
        //await Shell.Current.GoToAsync($"//{nameof(Mapa)}");
        //await Shell.Current.GoToAsync("///" + root);
        //await Shell.Current.GoToAsync($"mapa?parameter1=1");
        //await Shell.Current.DisplayAlert("Opción en desarrollo", "Proximamente estará habilitada", "Aceptar");
	}
    private async void Instituciones(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SineaceAPP.Pages.Instituciones.Instituciones());
    }  
    private async void Guia(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SineaceAPP.Pages.Guia.Guia());
    }
    private async void Programas(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SineaceAPP.Pages.Programas.Programas());
    }
    private async void Ranking(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SineaceAPP.Pages.Ranking.Ranking());
    } 
    private async void SineaceEnLinea(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://enlinea.sineace.gob.pe/");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }


    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {

    }

    private async void InformacionApp(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Aplicativo());
    }

    private async void Contactanos(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Contactanos());
    }
    private async void Sineace(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Sineace());
    }
}