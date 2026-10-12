using SineaceAPP.Models;
using SineaceAPP.Models.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SineaceAPP.Pages.Mapa;

public partial class MapaBottomSheet : The49.Maui.BottomSheet.BottomSheet
{
    private MapaViewModel _mapaViewModel;
    private string Institucion;
    MapaBottomSheet Page = null;
    public MapaBottomSheet(MapaBottomSheetModel mapaBottomSheetModel)
	{
		BindingContext = mapaBottomSheetModel;
        Institucion=mapaBottomSheetModel.Institucion;

        InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Page = sender as MapaBottomSheet;
        _mapaViewModel = MapaViewModel.Instance;
        _mapaViewModel.Local.Institucion = Institucion;
        Page.DismissAsync();
    }
}