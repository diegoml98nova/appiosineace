using SineaceAPP.Models;

namespace SineaceAPP.Pages.Informativo;

public partial class Sineace_2 : ContentPage
{
	public Sineace_2()
	{
		InitializeComponent();
        var items = new List<Carrucel2Model> {
            new Carrucel2Model {
                Titulo="Acreditación",
                TituloVisible=true,
                SubTitulo1Visible=false,
                SubTitulo11Visible=false,
                Descripcion1 = "Es un reconocimiento público que entrega el estado Peruano, a través del Sineace, a las instituciones o programas de estudios que cumplen con estándares de calidad y, por tanto, aseguran excelencia educativa.",
                Descripcion1Visible=true,
                SubTitulo2="Pasos para obtener la acreditación",
                SubTitulo2Visible=true,
                SubTitulo22="Paso 01: Autoevaluación",
                SubTitulo22Visible=true,
                Descripcion2 = "La institución educativa o programa de estudios, se organiza y reflexiona sistemáticamente sobre sus fines, procesos, estrategias, prácticas y resultados, contrastándolos con un referente de calidad (modelo para la acreditación) que le ayude a tomar decisiones para gestionar cambios y mejorar con base en evidencias. ",
                Descripcion2Visible=true,
            },
            new Carrucel2Model {
                TituloVisible=false,
                SubTitulo1="Pasos para obtener la acreditación",
                SubTitulo1Visible=true,
                SubTitulo11="Paso 02: Evaluación externa",
                SubTitulo11Visible=true,
                Descripcion1 = "La institución educativa o programa de estudios recibe la visita de una Entidad Evaluadora Externa quien verificará el logro de los estándares de calidad establecidos en el modelo para la acreditación, aprobado por Sineace.",
                Descripcion1Visible=true,
                SubTitulo2Visible=false,
                SubTitulo22="Paso 03: Acreditación",
                SubTitulo22Visible=true,
                Descripcion2 = "El Sineace otorga la acreditación a la institución educativa o programa de estudios, tras conocer los resultados de la evaluación externa. La acreditación es temporal, por lo que su vigencia dependerá del nivel de logro de los estándares de calidad.",
                Descripcion2Visible=true,
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