
using SineaceAPP.Models;

namespace SineaceAPP.Pages.Informativo;

public partial class Sineace_1 : ContentPage
{
    public Sineace_1()
    {
        InitializeComponent();
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
                //Posicion=LayoutOptions.Start
            },
            new CarrucelModel {
                Titulo2Visible=true,
                Titulo3Visible=true,
                Titulo2="Objetivos",
                Titulo3="estratégicos",
                Descripcion3v1="OEI 01: Contribuir a la mejora de la calidad del servicio educativo en las instituciones educativas de Educación Superior y Técnico – Productiva en beneficio de la comunidad educativa.",
                Descripcion3v2="OEI 02: Generar mayor valor en las condiciones laborales y educativas de las personas certificadas.",
                Descripcion3v3="OEI 03: Fortalecer la gestión del aseguramiento de la calidad a través de la generación de información, conocimientos y recomendaciones de políticas para los grupos de interés.",
                Descripcion3v4="OEI 04: Mejorar la evaluación de la acreditación y certificación a través de la supervisión de los administrados.",
                Descripcion3v5="OEI 05: Fortalecer la gestión institucional.",
                Descripcion3v6="OEI 06: Implementar la gestión de riesgo de desastres.",
                Descripcion3v1Visible=true,
                Descripcion3v2Visible=true,
                Descripcion3v3Visible=true,
                Descripcion3v4Visible=true,
                Descripcion3v5Visible=true,
                Descripcion3v6Visible=true,
                SubTituloVisible=false,
                TituloVisible=false,
                Descripcion1Visible=false,
                Descripcion2Visible=false,
                Descripcion3Visible=false,
                Descripcion4Visible=false,
                Descripcion5Visible=false,
                Descripcion6Visible=false,
                Descripcion7Visible=false,
                //Posicion=LayoutOptions.Start
            },

        };
        carrucleView.ItemsSource = items;

    }

    private async void btnAtras_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void carrucleView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {

    }
}