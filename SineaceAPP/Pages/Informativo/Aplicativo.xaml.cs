namespace SineaceAPP.Pages.Informativo;

public partial class Aplicativo : ContentPage
{
	public Aplicativo()
	{
		InitializeComponent();
	}

    private async void btnAtras_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}