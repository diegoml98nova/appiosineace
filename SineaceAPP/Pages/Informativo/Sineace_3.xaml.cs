using SineaceAPP.Models;

namespace SineaceAPP.Pages.Informativo;

public partial class Sineace_3 : ContentPage
{
    public Sineace_3()
    {
        InitializeComponent();
        var items = new List<Carrucel2Model> {
            new Carrucel2Model {
                Titulo="Certificación de",
                TituloVisible=true,
                SubTitulo1="competencias",
                SubTitulo1Visible=true,
                Descripcion1 = "Es el reconocimiento público de las competencias de una persona para su desempeño en su profesión y ocupación. Es otorgada por las entidades certificadoras autorizadas por el Sineace.",
                Descripcion1Visible=true,
                Descripcion2 = "La certificación de competencias profesionales no sustituye a un grado académico o título técnico/profesional emitido por una institución educativa público o privada. Asimismo, se evalúan competencias existentes, no futuras.",
                Descripcion2Visible=true,
                Descripcion3 = "Usted puede acceder a los reportes actualizados sobre normas de competencia, entidades certificadoras autorizadas, evaluadores de competencia y personas certificadas, a través del siguiente enlace: ",
                Descripcion3Visible=true,
            }

        };
        //carrucleView.ItemsSource = items;
        lbl1.Text = "Certificación de";
        lbl2.Text = "competencias";
        lbl3.Text = "Es el reconocimiento público de las competencias de una persona para su desempeño en su profesión y ocupación. Es otorgada por las entidades certificadoras autorizadas por el Sineace.";
        lbl4.Text = "La certificación de competencias profesionales no sustituye a un grado académico o título técnico/profesional emitido por una institución educativa público o privada. Asimismo, se evalúan competencias existentes, no futuras.";
        lbl5.Text = "Usted puede acceder a los reportes actualizados sobre normas de competencia, entidades certificadoras autorizadas, evaluadores de competencia y personas certificadas, a través del siguiente enlace: ";

    }

    private async void btnAtras_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void carrucleView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {

    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://app.sineace.gob.pe/sigice/padrones/frminicio.aspx");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }
}