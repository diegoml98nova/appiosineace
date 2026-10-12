using Microsoft.Maui.Controls;
using SineaceAPP.Models;

namespace SineaceAPP.Pages.Guia;

public partial class Guia : ContentPage
{
    public Guia()
    {
        InitializeComponent();

        var items = new List<CarrucelGuiaModel> {
            new CarrucelGuiaModel {
                Titulo="¡Hola!",
                Descripcion1 = "Bienvenido/a a la aplicación del Sineace donde podrás encontrar información de las instituciones educativas que cuentan con programas de estudio acreditados y vigentes.",
                ImgVisible=true,
                Descripcion2Visible=false,
                Descripcion3Visible=true,
                Descripcion4Visible=false,
                imgRuta="bienvenido.PNG",
            },
            new CarrucelGuiaModel {
                Titulo="Íconos",
                Descripcion1 = "La información se presentará de la siguiente manera:",
                ImgVisible=true,
                Descripcion2Visible=true,
                Descripcion3Visible=true,
                Descripcion4Visible=false,
                imgRuta="bienvenido_2_4.jpg",
            },
             new CarrucelGuiaModel {
                Titulo="Ficha",
                Descripcion1 = "Al seleccionar un ícono, este mostrará información complementaria acerca de la institución educativa: dirección, distancia en la que se encuentra y cantidad de programas acreditados que tiene esta institución.",
                ImgVisible=true,
                Descripcion2Visible=false,
                Descripcion3Visible=true,
                Descripcion4Visible=false,
                imgRuta="bienvenido_3_3.jpg",
            },
              new CarrucelGuiaModel {
                Titulo="Lista",
                Descripcion1 = "Al seleccionar una ficha, esta mostrará los programas de estudio que se brindan en este local de la institución educativa.",
                ImgVisible=true,
                Descripcion2Visible=false,
                Descripcion3Visible=false,
                Descripcion4Visible=true,
                imgRuta="bienvenido_4_4.jpg",
            },

        };
        carrucleView.ItemsSource = items;

    }

    private void carrucleView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        CarrucelModel item = e.CurrentItem as CarrucelModel;

        int posicion = carrucleView.Position;
        if (posicion == 3)
        {
            Button btn1 = (carrucleView.VisibleViews[0].FindByName("btnSiguiente") as Button);
            Button btn2 = (carrucleView.VisibleViews[0].FindByName("btnComenzar") as Button);
            btn1.IsVisible = false;
            btn2.IsVisible = true;
        }
    }



    private async void btnSiguiente_Clicked(object sender, EventArgs e)
    {
        if (carrucleView.Position == 3)
            await Navigation.PopModalAsync();
        else
            carrucleView.Position += 1;
    }

    private async void btnComenzar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}