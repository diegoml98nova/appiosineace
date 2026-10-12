using CommunityToolkit.Maui.Views;
using SineaceAPP.Models;
using SineaceAPP.Pages.Otros;
using SineaceAPP.Services.Dispositivo;

namespace SineaceAPP.Pages.Inicio;

public partial class InicioCarrucel : ContentPage
{
    private readonly CargaPopup _carga = new();
    string PrimeraVez = "";
    public InicioCarrucel()
    {

        InitializeComponent();
        string storedValue = Preferences.Get("PrimeraVez", "");
        PrimeraVez=storedValue;
        if (PrimeraVez.Equals("1"))
        {
            btnOmitir.IsVisible = true;
        }
        var items = new List<CarrucelModel> {
            new CarrucelModel {
                Descripcion1 = "Somos un organismo público descentralizado, adscrito al Ministerio de Educación.",
                Descripcion2 = "Garantizamos a la sociedad que las instituciones educativas ofrezcan un servicio educativo de calidad:",
                Descripcion3 = "•Acreditando la calidad de las instituciones y programas de estudios y",
                Descripcion4 = "•Certificando competencias laborales y profesionales",
                ImgVisible=true,
                TituloVisible=false,
                Descripcion5Visible=false,
                Descripcion6Visible=false,
                Descripcion7Visible=false,
                ButtomVisible=0,
                //Posicion=LayoutOptions.Center
            },
            new CarrucelModel {
                Titulo="Acreditación",
                SubTitulo="Beneficios",
                Descripcion1 = "Es el reconocimiento público y temporal otorgado por el Estado peruano, exclusivamente mediante el Sineace,  a programas de estudios o instituciones educativas con altos niveles de calidad.",
                Descripcion3 = "•Promueve mejora continua",
                Descripcion4 = "•Aumenta la producción académica",
                Descripcion5 = "•Mejora la reputación de la institución ",
                Descripcion6 = "•Facilita la empleabilidad de sus egresados ",
                Descripcion7 = "•Inclusión en redes internacionales",
                Descripcion2Visible=false,
                SubTituloVisible=true,
                ButtomVisible=PrimeraVez.Equals("1")? 0:1,
                //Posicion=LayoutOptions.Start
            },

        };
        carrucleView.ItemsSource = items;
        Label l = new Label();

    }
    private HtmlWebViewSource Parrafo(string texto)
    {
        var source = new HtmlWebViewSource();
        var text = "<html>" +
            "     <style>@font-face {font-family: myFirstFont;src: url('poppinslight.otf');}body {font-family: myFirstFont; font-size:14px; font-color:black;}</style>" +
                "<body  style=\"text-align: justify;\">" + texto + "</body>" +
                "</html>";
        source.Html = text;
        return source;
    }
    private HtmlWebViewSource Parrafo2(string texto)
    {
        var source = new HtmlWebViewSource();
        var text = "<html>" +
            "     <style>@font-face {font-family: myFirstFont;src: url('poppinslight.otf');}body {font-family: myFirstFont; font-size:14px; font-color:black; line-height:2em;}</style>" +
                "<body  style=\"text-align: justify;\">" + texto + "</body>" +
                "</html>";
        source.Html = text;
        return source;
    }
    private void pruba()
    {
        var browser = new WebView();
        browser.HorizontalOptions = LayoutOptions.FillAndExpand;
        browser.VerticalOptions = LayoutOptions.FillAndExpand;

        var source = new HtmlWebViewSource();
        var text = "<html>" +
                "<body  style=\"text-align: justify;\">Garantizamos a la sociedad que las instituciones educativas ofrezcan un servicio educativo de calidad:</body>" +
                "</html>";
        source.Html = text;
        browser.Source = source;

    }

    private async void carrucleView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {

        CarrucelModel item = e.CurrentItem as CarrucelModel;
        int posicion = carrucleView.Position;
        //for(int i=0;i<carrucleView.VisibleViews.Count;i++)
        //{
        //    Label lblTitulo = (carrucleView.VisibleViews[i].FindByName("lblTitulo") as Label);
        //    Button btn = (carrucleView.VisibleViews[i].FindByName("btnComencemos") as Button);
            

        //    if (lblTitulo != null)
        //    {
        //        string text = lblTitulo.Text;
        //        if (text!=null)
        //        {
        //            if (text.Equals("Acreditación"))
        //            {
        //                if (btn != null)
        //                {
        //                    await btn.FadeTo(1, 1000, Easing.Linear);
        //                }
        //            }
        //        } else
        //        {
        //            if (btn != null)
        //            {
        //                await btn.FadeTo(0, 100, Easing.Linear);
        //            }
        //        }
        //    }
        //}
    }
    private async void MostrarPantallaCargar()
    {
        await _carga.MostrarAsync(this);
        await Task.Delay(4000);
        await _carga.CerrarAsync();
    }
    private async void btnComencemos_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("PrimeraVez", "1");
        alPantallaCarga.IsVisible=true;
        ShowStarAnimation();
        await Task.Delay(2000);
       App.Current.MainPage = new AppShell();
        //await Navigation.PushModalAsync(new AppShell());
    }
    public async Task ShowStarAnimation()
    {
        //await Task.Delay(500);
        //imgLoader.IsAnimationPlaying = false;
        //imgLoader.IsAnimationPlaying = true;
        //base.OnAppearing();
        //await Task.Delay(500);
        img1.IsAnimationPlaying = false;
        img1.IsAnimationPlaying = true;
        img2.IsAnimationPlaying = false;
        img2.IsAnimationPlaying = true;
        AnimarLogo();
        AnimarLogo2();


    }

    private async void AnimarLogo()
    {
        ocultar();
        img1.IsVisible = true;
        await Task.Delay(100);
        ocultar();
        img2.IsVisible = true;
        await Task.Delay(100);
        ocultar();
        img3.IsVisible = true;
        await Task.Delay(100);
        AnimarLogo();
    }
    private async void AnimarLogo2()
    {
        ocultar2();
        imgd1.IsVisible = true;
        await Task.Delay(100);
        ocultar2();
        imgd2.IsVisible = true;
        await Task.Delay(100);
        ocultar2();
        imgd3.IsVisible = true;
        await Task.Delay(100);
        AnimarLogo2();
    }
    private void ocultar()
    {
        img1.IsVisible = false;
        img2.IsVisible = false;
        img3.IsVisible = false;
    }
    private void ocultar2()
    {
        imgd1.IsVisible = false;
        imgd2.IsVisible = false;
        imgd3.IsVisible = false;
    }

    private async void btnOmitir_Clicked(object sender, EventArgs e)
    {
        alPantallaCarga.IsVisible = true;
        ShowStarAnimation();
        await Task.Delay(2000);
        App.Current.MainPage = new AppShell();
    }
}