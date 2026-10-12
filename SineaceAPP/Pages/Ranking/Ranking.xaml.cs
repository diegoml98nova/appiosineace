using Microsoft.Maui.Controls;
using SineaceAPP.Models;
using SineaceAPP.Models.ViewModels;
using System.Collections.ObjectModel;
using System.Text.Json;

using System.Text;
using SineaceAPP.Services;
using Color = Microsoft.Maui.Graphics.Color;
using CommunityToolkit.Maui.Views;
using SineaceAPP.Pages.Otros;
using SineaceAPP.Services.Dispositivo;
using System.Collections.Generic;
using SineaceAPP.Pages.Instituciones;

using Mopups.Services;

namespace SineaceAPP.Pages.Ranking;

public partial class Ranking : ContentPage
{
    private readonly CargaPopup _carga = new();

    private readonly ObservableCollection<InstitucionesViewModel> listView = new ObservableCollection<InstitucionesViewModel>();
    private ObservableCollection<Universidad> listOriginal = new ObservableCollection<Universidad>();
    List<Universidad> listTemporal = new List<Universidad>();
    private SuneduModel programasAcreditados = new SuneduModel();
    private InsititucionesModel modelFiltrar = new InsititucionesModel();
    private int IndexView = 10;
    private int Orden = 1;
    Location locationActual = new Location();
    public Ranking()
    {
        InitializeComponent();
    }
    protected async override void OnAppearing()
    {
        int Count2 = Convert.ToInt32(lblResultados.Text.Split(":")[1]);
        if (Count2 == 0)
        {
            MostrarPantallaCargar();
            await Task.Delay(1000);
            await CheckLocation();
            ObtenerInstituciones();
            collection1.ItemsSource = listView;
        }
    }
    private void ObtenerInstituciones()
    {
        List<Universidad> list = new List<Universidad>();
       
        SuneduService suneduService = new SuneduService();
        programasAcreditados = suneduService.GetProgramas("acreditado", "");
        UniversidadModel Items = suneduService.GetUniversidades();
        foreach (Universidad item in Items.data)
        {
            
            int Exite = (list.Where(x => x.entidadCodigo.Equals(item.entidadCodigo))).ToList<Universidad>().Count;
            List<Sunedu> programas = new List<Sunedu>();
            programas = programasAcreditados.data.Where(x => x.entidadNombre.ToLower().Equals(item.entidadNombre.ToLower()) && x.etapaAcre.Equals("ACREDITACIÓN")).ToList();
            List<string> codigos = new List<string>();
            foreach (Sunedu sunedu in programas)
                if (!codigos.Exists(x => x.Equals(sunedu.programaCodigoEntidadPrograma)))
                    codigos.Add(sunedu.programaCodigoEntidadPrograma);
            int ProgramasAcreditados = codigos.Count;
            if (Exite == 0)
            {
                if (!item.longitudUbicacion.Equals("") && !item.latitudUbicacion.Equals(""))
                {
                    double Latitud = double.Parse(item.latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                    double Logitud = double.Parse(item.longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                    Location locationEntidad = new Location(Latitud, Logitud);
                    item.Distancia = ObtenerDistancia(locationActual, locationEntidad);
                    item.ProgramasAcreditados = ProgramasAcreditados;
                    list.Add(item);
                }
            }
        }
        int Count = 0;
        list = list.OrderByDescending(x => x.ProgramasAcreditados).ToList();
        if (txtBuscar.Text.Trim().Length > 0)
        {
            listTemporal = list.ToList();
            list = list.Where(x => x.entidadNombre.ToLower().Contains(txtBuscar.Text.ToLower())).ToList();
        }
        for (int i = 0; i < 10; i++)
        {
            if (list.Count > i)
            {
                LlenarListView(list[i]);
                Count++;
            }
        }
        lblResultados.Text = "Resultados:" + Count.ToString();
    }



    private void LlenarListView(Universidad list)
    {
        InstitucionesViewModel institucionesViewModel = new InstitucionesViewModel
        {
            Institucion = list.entidadNombre,
            Tipo = "Tipo: " + list.tipoInstitucion,
            Gestion = "Gestión: " + list.entidadTipoGestion,
            Estado = list.esLocalPrincipal.Equals("True") ? "Acreditado" : "Licenciado",
            ProgramasAcreditados = "Programas acreditados: " + list.ProgramasAcreditados.ToString(),
            Direccion = list.localDireccion,
            Distancia = "Distancia: " + list.Distancia + "KM",
            Longitud = list.longitudUbicacion,
            Latitud = list.latitudUbicacion,
            AcreditadosVisible = list.ProgramasAcreditados == 0 ? false : true
        };
        listView.Add(institucionesViewModel);
    }
    private async void MostrarPantallaCargar()
    {
        await _carga.MostrarAsync(this);
        await Task.Delay(2000);
        await _carga.CerrarAsync();
    }
    public async Task CheckLocation()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("hola", "ads", "Ok");
            return;
        }
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("hola", "ads", "Ok");
            return;
        }
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location == null)
        {
            location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(30),
                RequestFullAccuracy = true
            });
        }
        if (location == null)
        {
            await Shell.Current.DisplayAlert("hola", "ads", "Ok");
        }
        locationActual = location as Location;
        //locationActual = new Location(-12.117880, -77.033043);
    }

    private double ObtenerDistancia(Location location1, Location location2)
    {
        double distance = Location.CalculateDistance(location2, location1, DistanceUnits.Kilometers);
        return Math.Round(distance, 2);
    }




    private void collection1_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        //var t = (Universidad)sender;
        int index = e.LastVisibleItemIndex;
    }

    private async void btnMenu_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
        await Navigation.PopModalAsync();
    }




    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        List<Universidad> list = listOriginal.ToList();
        listView.Clear();
        ObtenerInstituciones();

    }



    private void collection1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var param = ((TappedEventArgs)e).Parameter;
        string institucion = param.ToString();
        await Navigation.PushModalAsync(new InstitucionesProgramas(institucion, locationActual));
    }
}