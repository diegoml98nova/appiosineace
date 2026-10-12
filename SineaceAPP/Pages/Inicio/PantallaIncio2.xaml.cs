namespace SineaceAPP.Pages.Inicio;

public partial class PantallaIncio2 : ContentPage
{
	public PantallaIncio2()
	{
		InitializeComponent();
	}
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(3000);
        //await Navigation.PushModalAsync(new sineace());
        App.Current.MainPage = new InicioCarrucel();

    }
}