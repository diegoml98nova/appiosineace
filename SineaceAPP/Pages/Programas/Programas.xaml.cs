using CommunityToolkit.Maui.Views;
using Mopups.Services;
using SineaceAPP.Models;
using SineaceAPP.Models.ViewModels;
using SineaceAPP.Pages.Instituciones;
using SineaceAPP.Pages.Otros;
using SineaceAPP.Services.Dispositivo;
using SineaceAPP.Services;
using System.Collections.ObjectModel;

namespace SineaceAPP.Pages.Programas;

public partial class Programas : ContentPage
{
    private readonly CargaPopup _carga = new();
    private readonly ObservableCollection<ProgramasViewModel> listView = new ObservableCollection<ProgramasViewModel>();
    private ObservableCollection<Sunedu> listOriginal = new ObservableCollection<Sunedu>();
    List<Sunedu> listTemporal = new List<Sunedu>();
    private SuneduModel programasAcreditados = new SuneduModel();
    private ProgramaFiltroModel modelFiltrar = new ProgramaFiltroModel();
    private int IndexView = 10;
    private int Orden = 1;
    Location locationActual = new Location();
    private MapaViewModel _mapaViewModel;
    public Programas()
    {
        InitializeComponent();
    }
    protected async override void OnAppearing()
    {
        int Count2 = Convert.ToInt32(lblResultados.Text.Split(":")[1]);
        if (Count2 == 0)
        {
            //await Task.Delay(500);
            MostrarPantallaCargar();
            await Task.Delay(1000);
            await CheckLocation();
            ObtenerProgramasAcreditados();
            OrdenarCalidad();
            collection1.ItemsSource = listView;
            collection1.RemainingItemsThreshold = 5;
            collection1.RemainingItemsThresholdReached += Collection1_RemainingItemsThresholdReached;
        }
    }
    private void ObtenerProgramasAcreditados()
    {
        List<Sunedu> list = listOriginal.ToList();
        SuneduService suneduService = new SuneduService();
        programasAcreditados = suneduService.GetProgramas("acreditado","");
        foreach (Sunedu item in programasAcreditados.data)
        {
            int Exite = (list.Where(x => x.cui.Equals(item.cui))).ToList<Sunedu>().Count;
            if (Exite == 0)
                list.Add(item);
        }
        list.OrderBy(x => x.entidadNombre).ToList();
        listOriginal = new ObservableCollection<Sunedu>(list);
    }
    private void ObtenerProgramasTotales()
    {
        SuneduService suneduService = new SuneduService();
        programasAcreditados = suneduService.GetProgramas("", "filtro");
        programasAcreditados.data.OrderBy(x => x.programaNombreEstandar).ToList();
        listOriginal = new ObservableCollection<Sunedu>(programasAcreditados.data);
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
    private void OrdenarCalidad()
    {
        int Numbers = 0;
        int Count = 0;
        int Count2 = Convert.ToInt32(lblResultados.Text.Split(":")[1]);
        List<Sunedu> list = listOriginal.ToList();
        if (Count2 != (listView.Count == 0 ? -1 : listView.Count))
        {
            if (txtBuscar.Text.Trim().Length > 0)
            {
                listTemporal = list.ToList();
                list = list.Where(x => x.programaNombreEstandar.ToLower().Contains(txtBuscar.Text.ToLower())).ToList();
            }
            for (int i = 0; i < list.Count; i++)
            {
                int existe = listView.Where(x => x.Programa.Equals(list[i].programaNombreEstandar)).ToList().Count;
                if (FiltrarAux(i))
                {
                    Count++;
                    if (existe == 0)
                    {
                        if (Numbers < IndexView)
                        {
                            LlenarListView(list[i]);
                            Numbers++;
                        }
                    }
                }
            }
            lblResultados.Text = "Resultados:" + Count.ToString();
        }
    }
    private bool FiltrarAux(int i)
    {
        List<Sunedu> list = listOriginal.ToList();
        bool validar = true;
        if (modelFiltrar.TipoGes1)
            if (list[i].entidadTipoGestion.Equals("Privado"))
                validar = false;
        if (modelFiltrar.TipoGes2)
            if (!list[i].entidadTipoGestion.Equals("Privado"))
                validar = false;
        if (modelFiltrar.TipoIns1)
            if (!list[i].tipoInstitucion.Equals("Universidad"))
                validar = false;
        if (modelFiltrar.TipoIns2)
            if (!list[i].tipoInstitucion.Equals("Instituto"))
                validar = false;
        if (modelFiltrar.TipoIns3)
            if (!list[i].tipoInstitucion.Equals("Escuelas de las FFAA"))
                validar = false;
        return validar;
    }
    private void OrdenarAZ()
    {
        List<Sunedu> list = listOriginal.ToList();
        list = list.OrderBy(x => x.programaNombreEstandar).ToList();
        int Numbers = 0;
        int Count = 0;
        int Count2 = Convert.ToInt32(lblResultados.Text.Split(":")[1]);
        if (Count2 != (listView.Count == 0 ? -1 : listView.Count))
        {
            if (txtBuscar.Text.Trim().Length > 0)
            {
                listTemporal = list.ToList();
                list = list.Where(x => x.programaNombreEstandar.ToLower().Contains(txtBuscar.Text.ToLower())).ToList();
            }
            for (int i = 0; i < list.Count; i++)
            {

                if (FiltrarAux(i))
                {
                    Count++;

                    if (Numbers < IndexView)
                    {
                        LlenarListView(list[i]);
                        Numbers++;
                    }
                }
            }
            lblResultados.Text = "Resultados: " + Count.ToString();
        }
    }
    private double ObtenerDistancia(Location location1, Location location2)
    {
        double distance = Location.CalculateDistance(location2, location1, DistanceUnits.Kilometers);
        return Math.Round(distance, 2);
    }
    private void LlenarListView(Sunedu dto)
    {
        List<ResolucionesViewModel> listresoluciones = new List<ResolucionesViewModel>();
        List<LocalesViewModel> listlocales = ObtenerLocales(dto.programaNombreEstandar, dto.etapaAcre, out listresoluciones);
        ProgramasViewModel institucionesViewModel = new ProgramasViewModel
        {
            Institucion = dto.entidadNombre,
            Programa = dto.programaNombreEstandar,
            TipoPrograma = "Tipo: " + dto.programaNivelAcademico,
            LocalesAcreditados = (dto.etapaAcre.Equals("ACREDITACIÓN") ? "Locales en los que se imparte el programa acreditado: " : "Locales en los que se imparte el programa: ") + listlocales.Count.ToString(),
            Distancia = "Distancia: " + dto.Distancia + "KM",
            AcreditadosVisible = dto.etapaAcre.Equals("ACREDITACIÓN") ? true : false,
            AcreditadosVisible2 = dto.etapaAcre.Equals("ACREDITACIÓN") ? false : true,
            Locales = listlocales,
            Resoluciones = listresoluciones
        };
        listView.Add(institucionesViewModel);
    }
    private List<LocalesViewModel> ObtenerLocales(string programa, string etapa, out List<ResolucionesViewModel> listResoluciones)
    {
        List<LocalesViewModel> listLocales = new List<LocalesViewModel>();
        listResoluciones = new List<ResolucionesViewModel>();
        string Distancia = "-";
        List<Sunedu> list = programasAcreditados.data.Where(x => x.programaNombreEstandar.Equals(programa) && x.etapaAcre.Equals(etapa)).ToList();
        foreach (Sunedu item in list)
        {
            switch (Orden)
            {
                case 1:
                    if (item.etapaAcre.Equals("ACREDITACIÓN"))
                    {
                        if (!item.longitudUbicacion.Equals("") && !item.latitudUbicacion.Equals(""))
                        {
                            double Latitud = double.Parse(item.latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                            double Logitud = double.Parse(item.longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                            Location locationEntidad = new Location(Latitud, Logitud);
                            Distancia = ObtenerDistancia(locationActual, locationEntidad).ToString();
                        }
                        listLocales.Add(new LocalesViewModel
                        {
                            Direccion = item.localDireccion,
                            Departemento = item.localDepartamento + ", " + item.localProvincia + ", " + item.localDistrito,
                            Provincia = item.localProvincia,
                            Distrito = item.localDistrito,
                            Distancia = "Distancia: " + Distancia + " KM",
                            Cordenadas=item.latitudUbicacion+";"+item.longitudUbicacion+";"+item.entidadNombre,
                        });
                        listResoluciones.Add(new ResolucionesViewModel
                        {
                            Resolucion = item.porc02,
                            Fechas = "[" + item.feciniAcre + " - " + item.fecfinAcre + "]",
                        });
                    }
                    break;
                case 2:
                    if (!item.longitudUbicacion.Equals("") && !item.latitudUbicacion.Equals(""))
                    {
                        double Latitud = double.Parse(item.latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                        double Logitud = double.Parse(item.longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                        Location locationEntidad = new Location(Latitud, Logitud);
                        Distancia = ObtenerDistancia(locationActual, locationEntidad).ToString();
                    }
                    listLocales.Add(new LocalesViewModel
                    {
                        Direccion = item.localDireccion,
                        Departemento = item.localDepartamento + ", " + item.localProvincia + ", " + item.localDistrito,
                        Provincia = item.localProvincia,
                        Distrito = item.localDistrito,
                        Distancia = "Distancia: " + Distancia + " KM",
                    });
                    listResoluciones.Add(new ResolucionesViewModel
                    {
                        Resolucion = item.porc02,
                        Fechas = "[" + item.feciniAcre + " - " + item.fecfinAcre + "]",
                    });
                    break;
            }
        }
        return listLocales;

    }
    private void Collection1_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        switch (Orden)
        {
            case 1:
                OrdenarCalidad();
                break;
            case 2:
                OrdenarAZ();
                break;
        }
    }
    private async void btnMenu_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
        await Navigation.PopModalAsync();
    }

    private async void btnFiltro_Clicked(object sender, EventArgs e)
    {
        var popup = new ProgramasFiltros();
        await MopupService.Instance.PushAsync(popup);
        var rvalue = await popup.PopupDismissedTask;
        modelFiltrar = (ProgramaFiltroModel)rvalue;
        if (!modelFiltrar.IB_Atras)
        {
            listView.Clear();
            switch (Orden)
            {
                case 1:
                    OrdenarCalidad();
                    break;
                case 2:
                    OrdenarAZ();
                    break;

            }
        }
    }

    private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
    {
        List<Sunedu> list = listOriginal.ToList();
        listView.Clear();
        switch (Orden)
        {
            case 1:
                //list =listTemporal.ToList();
                OrdenarCalidad();
                break;
            case 2:
                OrdenarAZ();
                break;
        }
    }

    private void btnCalidad_Clicked(object sender, EventArgs e)
    {
        CambiarColorBotones();
        Orden = 1;
        lblResultados.Text = "Resultados:0";
        bCalidad.BackgroundColor = Color.FromArgb("04acec");
        btnCalidad.TextColor = Colors.White;
        listView.Clear();
        ObtenerProgramasAcreditados();
        OrdenarCalidad();
    }

    private void btnAZ_Clicked(object sender, EventArgs e)
    {
        CambiarColorBotones();
        Orden = 2;
        lblResultados.Text = "Resultados:0";
        bAZ.BackgroundColor = Color.FromArgb("04acec");
        btnAZ.TextColor = Colors.White;
        listView.Clear();
        ObtenerProgramasTotales();
        OrdenarAZ();
    }
    private void CambiarColorBotones()
    {
        bAZ.BackgroundColor = Colors.White;
        bCalidad.BackgroundColor = Colors.White;
        btnCalidad.TextColor = Color.FromArgb("04acec");
        btnAZ.TextColor = Color.FromArgb("04acec");
    }
    private void collection1_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        var param = ((TappedEventArgs)e).Parameter;
        string Cordenadas = param.ToString();
        _mapaViewModel = MapaViewModel.Instance;
        _mapaViewModel.Local.Cordenadas= Cordenadas;
        await Navigation.PopModalAsync();

    }
}