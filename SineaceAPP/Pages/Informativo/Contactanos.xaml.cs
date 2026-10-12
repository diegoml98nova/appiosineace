namespace SineaceAPP.Pages.Informativo;

public partial class Contactanos : ContentPage
{
	public Contactanos()
	{
		InitializeComponent();
	}

    private async void btnAtras_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnCompartir_Clicked(object sender, EventArgs e)
    {
        await ShareText("Te recomiendo esta Aplicación de Sineace: https://play.google.com/store/apps/details?id=app.background.appsineace");
    }
    public async Task ShareUri(string uri, IShare share)
    {
        await share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Share Web Link"
        });
    }
    public async Task ShareText(string text)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Share Text"
        });
    }

    private async void btnFacebook_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://web.facebook.com/SINEACEOFICIAL");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    private async void btnTwiter_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://twitter.com/SineacePeru");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    private async void btnYoutube_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.youtube.com/c/SINEACECalidadEducativa");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    private async void btnLinkedin_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.linkedin.com/company/sineace/");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    private void btnTelefono_Clicked(object sender, EventArgs e)
    {
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open("516371123");
    }

    private async void btnCorreo1_Clicked(object sender, EventArgs e)
    {
        List<string> list = new List<string>();
        list.Add("consultas@sineace.gob.pe");
        var message = new EmailMessage
        {
            Subject = "Titulo",
            Body = "",
            BodyFormat = EmailBodyFormat.PlainText,
            To = list
        };

        await Email.Default.ComposeAsync(message);
    }

    private async void btnCorreo2_Clicked(object sender, EventArgs e)
    {
        List<string> list = new List<string>();
        list.Add("soporte@sineace.gob.pe");
        var message = new EmailMessage
        {
            Subject = "Titulo",
            Body = "",
            BodyFormat = EmailBodyFormat.PlainText,
            To = list
        };

        await Email.Default.ComposeAsync(message);
    }

    private async void btnGobierno_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.gob.pe/sineace");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    private async void btnMapa_Clicked(object sender, EventArgs e)
    {
        var location = new Location(-12.1008908, -77.0186877);
        var options = new MapLaunchOptions { Name = "Sineace - Sistema Nacional de Evaluación, Acreditación y Certificación de la Calidad Educativa" };

        try
        {
            await Map.Default.OpenAsync(location, options);
        } catch (Exception ex)
        {
            // No map application available to open
        }
    }
}