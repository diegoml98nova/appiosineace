
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

using Mopups.Services;

namespace SineaceAPP.Pages.Instituciones;

public partial class Instituciones : ContentPage
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
    public Instituciones()
    {
        InitializeComponent();
        //GetUniversidades();

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
            ObtenerInstituciones();
            OrdenarCalidad();
            collection1.ItemsSource = listView;

            collection1.RemainingItemsThreshold = 5;
            collection1.RemainingItemsThresholdReached += Collection1_RemainingItemsThresholdReached;
        }
    }
    private void ObtenerInstituciones()
    {
        List<Universidad> list = listOriginal.ToList();
        SuneduService suneduService = new SuneduService();
        programasAcreditados = suneduService.GetProgramas("acreditado","");
        UniversidadModel Items = suneduService.GetUniversidades();
        foreach (Universidad item in Items.data)
        {
            int Exite = (list.Where(x => x.entidadCodigo.Equals(item.entidadCodigo))).ToList<Universidad>().Count;
            if (Exite == 0)
            {
                item.localDepartamento = item.localDepartamento + ", " + item.localProvincia + ", " + item.localDistrito;
                list.Add(item);
            }
        }
        list.OrderBy(x => x.entidadNombre).ToList();
        listOriginal = new ObservableCollection<Universidad>(list);
    }
    //private void OrdenarCalidad2()
    //{
    //    int Numbers = 0;
    //    int Count = 0;
    //    List<Universidad> list = listOriginal.ToList();
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        int ProgramasAcreditados = programasAcreditados.data.Where(x => x.entidadNombre.ToLower().Equals(list[i].entidadNombre.ToLower())).ToList().Count;
    //        if (ProgramasAcreditados != 0)
    //        {
    //            int existe = listView.Where(x => x.Institucion.ToLower().Equals(list[i].entidadNombre.ToLower())).ToList().Count;
    //            if (FiltrarAux(i))
    //            {
    //                Count++;
    //                if (existe == 0)
    //                {
    //                    LlenarListView(list[i], ProgramasAcreditados);
    //                    Numbers++;
    //                }
    //            }
    //        }
    //    }
    //}

    private void Filtrar()
    {
        List<Universidad> list = listOriginal.ToList();
        list = list.OrderBy(x => x.entidadNombre).ToList();
        int Numbers = 0;
        int Count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            int ProgramasAcreditados = programasAcreditados.data.Where(x => x.entidadNombre.Equals(list[i].entidadNombre)).ToList().Count;
            int existe = listView.Where(x => x.Institucion.Equals(list[i].entidadNombre)).ToList().Count;
            if (FiltrarAux(i))
            {
                Count++;

                if (existe == 0)
                {
                    if (Numbers < IndexView)
                    {
                        LlenarListView(list[i], ProgramasAcreditados);
                        Numbers++;
                    }
                }
            }
        }
        //lblResultados.Text = "Resultados: " + Count.ToString();
    }
    private bool FiltrarAux(int i)
    {
        List<Universidad> list = listOriginal.ToList();
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
        if (modelFiltrar.TipoCal1)
            if (!list[i].esLocalPrincipal.Equals("True"))
                validar = false;
        if (modelFiltrar.TipoCal2)
            if (!list[i].esLocalPrincipal.Equals("False"))
                validar = false;
        //if(txtBuscar.Text.Length > 0)
        //    if(!list[i].entidadNombre.Contains(txtBuscar.Text))
        //        validar = false;
        return validar;
    }
    private void LlenarListView(Universidad list, int ProgramasAcreditados)
    {
        InstitucionesViewModel institucionesViewModel = new InstitucionesViewModel
        {
            Institucion = list.entidadNombre,
            Tipo = "Tipo: " + list.tipoInstitucion,
            Gestion = "Gestión: " + list.entidadTipoGestion,
            Estado = list.esLocalPrincipal.Equals("True") ? "Acreditado" : "Licenciado",
            ProgramasAcreditados = "Programas acreditados: " + ProgramasAcreditados.ToString(),
            Direccion = list.localDireccion,
            Distancia = "Distancia: " + list.Distancia + "KM",
            Direccion2 = list.localDepartamento,
            Longitud = list.longitudUbicacion,
            Latitud = list.latitudUbicacion,
            AcreditadosVisible = ProgramasAcreditados == 0 ? false : true
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
        //Shell.Current.FlyoutIsPresented = true;
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
            case 3:
                OrdenarCercania();
                break;
        }
    }
    private async void btnFiltro_Clicked(object sender, EventArgs e)
    {
        var popup = new InstitucionesFiltros();
        await MopupService.Instance.PushAsync(popup);
        var rvalue = await popup.PopupDismissedTask;
        modelFiltrar = (InsititucionesModel)rvalue;
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
                case 3:
                    OrdenarCercania();
                    break;
            }
        }
    }


    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        List<Universidad> list = listOriginal.ToList();
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
            case 3:
                OrdenarCercania();
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
        OrdenarAZ();

    }

    private void btnCercania_Clicked(object sender, EventArgs e)
    {
        CambiarColorBotones();
        Orden = 3;
        lblResultados.Text = "Resultados:0";
        bCercania.BackgroundColor = Color.FromArgb("04acec");
        btnCercania.TextColor = Colors.White;
        listView.Clear();
        for (int i = 0; i < listOriginal.Count; i++)
        {
            if (!listOriginal[i].longitudUbicacion.Equals("") && !listOriginal[i].latitudUbicacion.Equals(""))
            {
                double Latitud = double.Parse(listOriginal[i].latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                double Logitud = double.Parse(listOriginal[i].longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                Location locationEntidad = new Location(Latitud, Logitud);
                listOriginal[i].Distancia = ObtenerDistancia(locationActual, locationEntidad);
            }
        }
        OrdenarCercania();
    }
    private void CambiarColorBotones()
    {
        bAZ.BackgroundColor = Colors.White;
        bCalidad.BackgroundColor = Colors.White;
        bCercania.BackgroundColor = Colors.White;
        btnCercania.TextColor = Color.FromArgb("04acec");
        btnCalidad.TextColor = Color.FromArgb("04acec");
        btnAZ.TextColor = Color.FromArgb("04acec");
    }
    private void OrdenarCalidad()
    {
        int Numbers = 0;
        int Count = 0;
        int Count2 = Convert.ToInt32(lblResultados.Text.Split(":")[1]);
        List<Universidad> list = listOriginal.ToList();
        if (Count2 != (listView.Count == 0 ? -1 : listView.Count))
        {
            if (txtBuscar.Text.Trim().Length > 0)
            {
                listTemporal = list.ToList();
                list = list.Where(x => x.entidadNombre.ToLower().Contains(txtBuscar.Text.ToLower())).ToList();
            }
            for (int i = 0; i < list.Count; i++)
            {
                List<Sunedu> programas = new List<Sunedu>();
                programas = programasAcreditados.data.Where(x => x.entidadNombre.ToLower().Equals(list[i].entidadNombre.ToLower()) && x.etapaAcre.Equals("ACREDITACIÓN")).ToList();
                List<string> codigos = new List<string>();
                foreach (Sunedu sunedu in programas)
                    if (!codigos.Exists(x => x.Equals(sunedu.programaCodigoEntidadPrograma)))
                        codigos.Add(sunedu.programaCodigoEntidadPrograma);
                int ProgramasAcreditados = codigos.Count;
                if (ProgramasAcreditados != 0)
                {
                    int existe = listView.Where(x => x.Institucion.ToLower().Equals(list[i].entidadNombre.ToLower())).ToList().Count;
                    if (FiltrarAux(i))
                    {

                        Count++;
                        if (existe == 0)
                        {
                            if (Numbers < IndexView)
                            {
                                if (!list[i].longitudUbicacion.Equals("") && !list[i].latitudUbicacion.Equals(""))
                                {
                                    double Latitud = double.Parse(list[i].latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                                    double Logitud = double.Parse(list[i].longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                                    Location locationEntidad = new Location(Latitud, Logitud);
                                    list[i].Distancia = ObtenerDistancia(locationActual, locationEntidad);
                                    LlenarListView(list[i], ProgramasAcreditados);
                                    Numbers++;
                                }
                            }
                        }
                    }
                }
            }
            lblResultados.Text = "Resultados:" + Count.ToString();
        }
    }
    private void OrdenarAZ()
    {
        List<Universidad> list = listOriginal.ToList();
        list = list.OrderBy(x => x.entidadNombre).ToList();
        int Numbers = 0;
        int Count = 0;
        int Count2 = Convert.ToInt32(lblResultados.Text.Split(":")[1]);
        if (Count2 != (listView.Count == 0 ? -1 : listView.Count))
        {
            if (txtBuscar.Text.Trim().Length > 0)
            {
                listTemporal = list.ToList();
                list = list.Where(x => x.entidadNombre.ToLower().Contains(txtBuscar.Text.ToLower())).ToList();
            }
            for (int i = 0; i < list.Count; i++)
            {
                List<Sunedu> programas = new List<Sunedu>();
                programas = programasAcreditados.data.Where(x => x.entidadNombre.ToLower().Equals(list[i].entidadNombre.ToLower()) && x.etapaAcre.Equals("ACREDITACIÓN")).ToList();
                List<string> codigos = new List<string>();
                foreach (Sunedu sunedu in programas)
                    if (!codigos.Exists(x => x.Equals(sunedu.programaCodigoEntidadPrograma)))
                        codigos.Add(sunedu.programaCodigoEntidadPrograma);
                int ProgramasAcreditados = codigos.Count;
                int existe = listView.Where(x => x.Institucion.Equals(list[i].entidadNombre)).ToList().Count;
                if (FiltrarAux(i))
                {
                    Count++;
                    if (existe == 0)
                    {
                        if (Numbers < IndexView)
                        {
                            if (!list[i].longitudUbicacion.Equals("") && !list[i].latitudUbicacion.Equals(""))
                            {
                                double Latitud = double.Parse(list[i].latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                                double Logitud = double.Parse(list[i].longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                                Location locationEntidad = new Location(Latitud, Logitud);
                                list[i].Distancia = ObtenerDistancia(locationActual, locationEntidad);
                                LlenarListView(list[i], ProgramasAcreditados);
                                Numbers++;
                            }
                        }
                    }
                }
            }
            lblResultados.Text = "Resultados: " + Count.ToString();
        }
    }
    private void OrdenarCercania()
    {

        List<Universidad> list = listOriginal.ToList();
       
        list = list.OrderBy(x => x.Distancia).ToList();
        int Numbers = 0;
        int Count = 0;
        int Count2 = Convert.ToInt32(lblResultados.Text.Split(":")[1]);
        if (Count2 != (listView.Count == 0 ? -1 : listView.Count))
        {
            if (txtBuscar.Text.Trim().Length > 0)
            {
                listTemporal = list.ToList();
                list = list.Where(x => x.entidadNombre.ToLower().Contains(txtBuscar.Text.ToLower())).ToList();
            }
            for (int i = 0; i < list.Count; i++)
            {
                List<Sunedu> programas = new List<Sunedu>();
                programas = programasAcreditados.data.Where(x => x.entidadNombre.ToLower().Equals(list[i].entidadNombre.ToLower()) && x.etapaAcre.Equals("ACREDITACIÓN")).ToList();
                List<string> codigos = new List<string>();
                foreach (Sunedu sunedu in programas)
                    if (!codigos.Exists(x => x.Equals(sunedu.programaCodigoEntidadPrograma)))
                        codigos.Add(sunedu.programaCodigoEntidadPrograma);
                int ProgramasAcreditados = codigos.Count;
                int existe = listView.Where(x => x.Institucion.Equals(list[i].entidadNombre)).ToList().Count;
                if (FiltrarAux(i))
                {
                    Count++;
                    if (existe == 0)
                    {
                        if (Numbers < IndexView)
                        {
                            LlenarListView(list[i], ProgramasAcreditados);
                            Numbers++;
                        }
                    }
                }
            }
            lblResultados.Text = "Resultados: " + Count.ToString();
        }
    }

    private void collection1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var param = ((TappedEventArgs)e).Parameter;
        string institucion=param.ToString();
        await Navigation.PushModalAsync(new InstitucionesProgramas(institucion, locationActual));
    }
}