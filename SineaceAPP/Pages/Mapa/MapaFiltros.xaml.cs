using Mopups.Services;
using SineaceAPP.Models;

namespace SineaceAPP.Pages.Mapa;

public partial class MapaFiltros 
{
    TaskCompletionSource<MapaFiltroModel> _taskCompletionSource;
    public Task<MapaFiltroModel> PopupDismissedTask => _taskCompletionSource.Task;
    public MapaFiltroModel ReturnValue { get; set; }
    MapaFiltroModel MapaFiltroModel = new MapaFiltroModel();
    public MapaFiltros(MapaFiltroModel model)
    {
        InitializeComponent();
        if (model.TipoMapa == 2)
            btipoInstitucion3.IsVisible = false;
        slDistancia.Value = 10;
        MostarFiltros(model);

    }
    private void MostarFiltros(MapaFiltroModel model)
    {
        MapaFiltroModel=model;
        if (MapaFiltroModel.TipoIns1)
        {
            MapaFiltroModel.TipoIns1 = true;
            btnChkIns1.Source = "check1png.png";
        }
        if (MapaFiltroModel.TipoIns2)
        {
            MapaFiltroModel.TipoIns2 = true;
            btnChkIns2.Source = "check1png.png";
        }
        if (MapaFiltroModel.TipoIns3)
        {
            MapaFiltroModel.TipoIns3 = true;
            btnChkIns3.Source = "check1png.png";
        }
        if (MapaFiltroModel.TipoGes1)
        {
            MapaFiltroModel.TipoGes1 = true;
            btnChkGes1.Source = "check1png.png";
        }
        if (MapaFiltroModel.TipoGes2)
        {
            MapaFiltroModel.TipoGes2 = true;
            btnChkGes2.Source = "check1png.png";
        }
        if (MapaFiltroModel.SinLimite)
        {
            chSinLimites.IsChecked = true;
        } else
        {
            slDistancia.Value = Convert.ToDouble(MapaFiltroModel.Distancia);
        }

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _taskCompletionSource = new TaskCompletionSource<MapaFiltroModel>();
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        _taskCompletionSource.SetResult(ReturnValue);
    }

    private void btnAtras_Clicked(object sender, EventArgs e)
    {
        ReturnValue = new MapaFiltroModel();
        ReturnValue.IB_Atras = true;
        MopupService.Instance.PopAsync();
    }


    private void Slider_ValueChanged_1(object sender, ValueChangedEventArgs e)
    {
        double distancia=Math.Round(slDistancia.Value,2);
        String value= distancia.ToString();
        lblDistancia.Text = value+" Km";
    }
    private void btnChkIns1_Clicked(object sender, EventArgs e)
    {
        if (MapaFiltroModel.TipoIns1)
        {
            MapaFiltroModel.TipoIns1 = false;
            btnChkIns1.Source = "check2png.png";
        } else
        {
            MapaFiltroModel.TipoIns1 = true;
            btnChkIns1.Source = "check1png.png";

        }
    }

    private void btnChkIns2_Clicked(object sender, EventArgs e)
    {
        if (MapaFiltroModel.TipoIns2)
        {
            MapaFiltroModel.TipoIns2 = false;
            btnChkIns2.Source = "check2png.png";
        } else
        {
            MapaFiltroModel.TipoIns2 = true;
            btnChkIns2.Source = "check1png.png";

        }
    }

    private void btnChkIns3_Clicked(object sender, EventArgs e)
    {
        if (MapaFiltroModel.TipoIns3)
        {
            MapaFiltroModel.TipoIns3 = false;
            btnChkIns3.Source = "check2png.png";
        } else
        {
            MapaFiltroModel.TipoIns3 = true;
            btnChkIns3.Source = "check1png.png";

        }
    }

    private void btnChkGes1_Clicked(object sender, EventArgs e)
    {
        if (MapaFiltroModel.TipoGes1)
        {
            MapaFiltroModel.TipoGes1 = false;
            btnChkGes1.Source = "check2png.png";
        } else
        {
            MapaFiltroModel.TipoGes1 = true;
            btnChkGes1.Source = "check1png.png";

        }
    }

    private void btnChkGes2_Clicked(object sender, EventArgs e)
    {
        if (MapaFiltroModel.TipoGes2)
        {
            MapaFiltroModel.TipoGes2 = false;
            btnChkGes2.Source = "check2png.png";
        } else
        {
            MapaFiltroModel.TipoGes2 = true;
            btnChkGes2.Source = "check1png.png";

        }
    }

    private void btnBuscar_Clicked(object sender, EventArgs e)
    {
        ReturnValue = MapaFiltroModel;
        ReturnValue.SinLimite = chSinLimites.IsChecked;
        ReturnValue.IB_Atras = false;
        ReturnValue.Distancia = Math.Round(slDistancia.Value, 2).ToString();
        MopupService.Instance.PopAsync();
    }
}
