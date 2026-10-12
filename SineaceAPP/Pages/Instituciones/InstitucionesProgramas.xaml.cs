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
using System.Collections.Generic;
using Mopups.Services;

namespace SineaceAPP.Pages.Instituciones;

public partial class InstitucionesProgramas : ContentPage
{
    private  ObservableCollection<ProgramasViewModel> listView = new ObservableCollection<ProgramasViewModel>();
    List<Universidad> listTemporal = new List<Universidad>();
    private SuneduModel programasAcreditados = new SuneduModel();
    private int IndexView = 10;
    private int Orden = 1;
    Location locationActual = new Location();
    string Universidad = "";
    public InstitucionesProgramas(string uni, Location location)
    {
        InitializeComponent();
        Universidad = uni;
        locationActual = location;
    }
    protected override void OnAppearing()
    {
        ObtenerProgramas();
       
        //collection1.RemainingItemsThreshold = 5;
        //collection1.RemainingItemsThresholdReached += Collection1_RemainingItemsThresholdReached;
    }
    private void ObtenerProgramas()
    {
        listView= new ObservableCollection<ProgramasViewModel>();
        List<Sunedu> list = new List<Sunedu>();
        SuneduService suneduService = new SuneduService();
        programasAcreditados = suneduService.GetProgramas(Universidad,"");
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
                            list.Add(item);
                            LlenarListView(item);
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
    private List<LocalesViewModel> ObtenerLocales(string programa,string etapa, out List<ResolucionesViewModel> listResoluciones)
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
    private double ObtenerDistancia(Location location1, Location location2)
    {
        double distance = Location.CalculateDistance(location2, location1, DistanceUnits.Kilometers);
        return Math.Round(distance, 2);
    }
    private void LlenarListView(Sunedu dto)
    {
        List<ResolucionesViewModel> listresoluciones = new List<ResolucionesViewModel>();
        List<LocalesViewModel> listlocales = ObtenerLocales(dto.programaNombreEstandar,dto.etapaAcre, out listresoluciones);
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
    private void CambiarColorBotones()
    {
        bAZ.BackgroundColor = Colors.White;
        bCalidad.BackgroundColor = Colors.White;
        btnCalidad.TextColor = Color.FromArgb("04acec");
        btnAZ.TextColor = Color.FromArgb("04acec");
    }
    private async void btnMenu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
    {

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

    private void collection1_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}