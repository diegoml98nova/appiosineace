using SineaceAPP.Models.ViewModels;
using System.Collections.ObjectModel;

namespace SineaceAPP.Pages.Informativo;

public partial class Sineace : ContentPage
{
	public Sineace()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        CargarMenuSineace();
    }
    private void CargarMenuSineace()
    {
        ObservableCollection <SineaceViewModel> listView= new ObservableCollection<SineaceViewModel>();
        listView.Add(new SineaceViewModel
        {
            Titulo = "¿Que es el Sineace?",
            RutaImagen = "sineace_op_1.png"
        });
        listView.Add(new SineaceViewModel
        {
            Titulo = "Acreditación",
            RutaImagen = "sineace_op_2.png"
        });
        listView.Add(new SineaceViewModel
        {
            Titulo = "Certificación",
            RutaImagen = "sineace_op_3.png"
        });
        listView.Add(new SineaceViewModel
        {
            Titulo = "Preguntas Frecuentes",
            RutaImagen = "sineace_op_4.png"
        });
        collection1.ItemsSource = listView;
    }

    private async void btnMenu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var param = ((TappedEventArgs)e).Parameter;
        VerticalStackLayout vertical = sender as VerticalStackLayout;
        Label lbltipo=vertical.Children[1] as Label;
        string tipo = lbltipo.Text;
        switch (tipo)
        {
            case "¿Que es el Sineace?":
                await Navigation.PushModalAsync(new Sineace_1());
                break;
            case "Acreditación":
                await Navigation.PushModalAsync(new Sineace_2());
                break;
            case "Certificación":
                await Navigation.PushModalAsync(new Sineace_3());
                break;  
            case "Preguntas Frecuentes":
                await Navigation.PushModalAsync(new Preguntas());
                break;
        }
    }
}