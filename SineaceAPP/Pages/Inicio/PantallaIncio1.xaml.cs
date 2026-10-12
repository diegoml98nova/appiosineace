namespace SineaceAPP.Pages.Inicio;

public partial class PantallaIncio1 : ContentPage
{
	public PantallaIncio1()
	{
		InitializeComponent();
	}
    private async void AnimarLogo()
    {
        MoverDerecha();
        await Task.Delay(1000);
        MoverArriba();
        await Task.Delay(1000);
        MoverAbajo();
        await Task.Delay(1000);
        var a1 = gifImage2.FadeTo(1, 1000, Easing.Linear);
        var a2 = gifImage1.FadeTo(0, 1000, Easing.Linear);
        await Task.WhenAll(a1, a2);
        await Task.Delay(1000);
        App.Current.MainPage = new PantallaIncio2();
      


    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await gifImage2.FadeTo(0, 0, Easing.Linear);

        await Task.Delay(500);
        gifImage1.IsAnimationPlaying = false;
        gifImage1.IsAnimationPlaying = true;
        gifImage2.IsAnimationPlaying = false;
        gifImage2.IsAnimationPlaying = true;
        AnimarLogo();
    }


    private async void CounterBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AppShell());
    }
    private void MoverDerecha()
    {
        gifImage1.TranslateTo(90, 0, 1000);
    }
    private void MoverArriba()
    {
        gifImage1.TranslateTo(0, -80, 1000);
    }
    private void MoverAbajo()
    {
        gifImage1.TranslateTo(-90, 0, 1000);
    }
    private void MoverIzquierda()
    {
        //    gifImage.TranslateTo(-20, 0, 1000);
    }
}