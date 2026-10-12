//using CommunityToolkit.Maui.Views;
using Mopups.Services;
using SineaceAPP.Models;
using SineaceAPP.Models.ViewModels;
using SineaceAPP.Pages.Instituciones;
using SineaceAPP.Pages.Otros;
using SineaceAPP.Services;
using System.Collections.ObjectModel;

namespace SineaceAPP.Pages.Mapa;

public partial class MapaProgramas : ContentPage
{
    private ObservableCollection<ProgramasViewModel> listView = new ObservableCollection<ProgramasViewModel>();
    private SuneduModel programasAcreditados = new SuneduModel();
    private int Orden = 1;
    Location locationActual = new Location();
    Location locationEntidad = new Location();
    private MapaViewModel _mapaViewModel;
    string Universidad2 = "";
    public MapaProgramas(string uni, Location location,Location entidad,int tipo)
    {
        InitializeComponent();
        Universidad2 = uni;
        locationActual = location;
        locationEntidad = entidad;
        Orden = tipo;
        CambiarColorBotones();
        if (tipo == 1)
        {
            lblResultados.Text = "Resultados:0";
            bCalidad.BackgroundColor = Color.FromArgb("04acec");
            btnCalidad.TextColor = Colors.White;
        } else
        {
            lblResultados.Text = "Resultados:0";
            bAZ.BackgroundColor = Color.FromArgb("04acec");
            btnAZ.TextColor = Colors.White;
        }
    }
    protected override void OnAppearing()
    {
        ObtenerProgramas();
    }
    private void ObtenerProgramas()
    {
        listView = new ObservableCollection<ProgramasViewModel>();
        List<Sunedu> list = new List<Sunedu>();
        SuneduService suneduService = new SuneduService();
        programasAcreditados = suneduService.GetProgramas(Universidad2, "");
        int Exite = 0;
        foreach (Sunedu item in programasAcreditados.data)
        {
            switch (Orden)
            {
                case 1:
                    if (item.etapaAcre.Equals("ACREDITACIÓN"))
                    {
                        Exite = (list.Where(x => x.programaNombreEstandar.Equals(item.programaNombreEstandar))).ToList<Sunedu>().Count;
                        if (Exite == 0)
                        {
                            int length1 = item.latitudUbicacion.Length - 15;
                            int length2 = item.longitudUbicacion.Length - 15;
                            int length3 = locationEntidad.Latitude.ToString().Length - 15;
                            int length4 = locationEntidad.Longitude.ToString().Length - 15;
                            string Latitud1 = length1 > 0 ? item.latitudUbicacion.Substring(0, item.latitudUbicacion.Length - length1) : item.latitudUbicacion;
                            string Logitud1 = length2 > 0 ? item.longitudUbicacion.Substring(0, item.longitudUbicacion.Length - length2) : item.longitudUbicacion;
                            string Latitud2 = length3 > 0 ? locationEntidad.Latitude.ToString().Replace(",", ".").Substring(0, locationEntidad.Latitude.ToString().Replace(",", ".").Length - length3) : locationEntidad.Latitude.ToString().Replace(",", ".");
                            string Logitud2 = length4 > 0 ? locationEntidad.Longitude.ToString().Replace(",", ".").Substring(0, locationEntidad.Longitude.ToString().Replace(",", ".").Length - length4) : locationEntidad.Longitude.ToString().Replace(",", ".");
                            if (Latitud1.Equals(Latitud2) && Logitud1.Equals(Logitud2))
                            {
                                list.Add(item);
                                LlenarListView(item);
                            }
                        }
                    }
                    break;
                case 2:
                    Exite = (list.Where(x => x.programaNombreEstandar.Equals(item.programaNombreEstandar) && x.etapaAcre.Equals(item.etapaAcre))).ToList<Sunedu>().Count;
                    if (Exite == 0)
                    {
                        list.Add(item);
                        LlenarListView(item);
                    }
                    break;
            }
        }
        list.OrderBy(x => x.programaNombreEstandar).ToList();
        lblResultados.Text = "Resultados:" + list.Count.ToString();
        collection1.ItemsSource = listView;

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
                            Cordenadas = item.latitudUbicacion + ";" + item.longitudUbicacion + ";" + item.entidadNombre,
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
    private async void btnMenu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
    {
        //List<Sunedu> list = listOriginal.ToList();
        //listView.Clear();
        //switch (Orden)
        //{
        //    case 1:
        //        //list =listTemporal.ToList();
        //        OrdenarCalidad();
        //        break;
        //    case 2:
        //        OrdenarAZ();
        //        break;
        //}
    }

    private void btnCalidad_Clicked(object sender, EventArgs e)
    {

        CambiarColorBotones();
        Orden = 1;
        lblResultados.Text = "Resultados:0";
        bCalidad.BackgroundColor = Color.FromArgb("04acec");
        btnCalidad.TextColor = Colors.White;
        listView.Clear();
        ObtenerProgramas();
    }

    private void btnAZ_Clicked(object sender, EventArgs e)
    {
        CambiarColorBotones();
        Orden = 2;
        lblResultados.Text = "Resultados:0";
        bAZ.BackgroundColor = Color.FromArgb("04acec");
        btnAZ.TextColor = Colors.White;
        listView.Clear();
        ObtenerProgramas();
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
        _mapaViewModel.Local.Cordenadas = Cordenadas;
        await Navigation.PopModalAsync();

    }
}