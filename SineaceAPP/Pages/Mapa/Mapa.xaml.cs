using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Mopups.Services;
using System.Text.Json;
using The49.Maui.BottomSheet;

using System.Diagnostics;
using System.Reflection;
using System.Text;
using SineaceAPP.Models.ViewModels;
using SineaceAPP.Custom.Mapa;
using SineaceAPP.Models;
using SineaceAPP.Pages.Otros;
using SineaceAPP.Services.Dispositivo;
using SineaceAPP.Services;
using System.Collections.ObjectModel;
using static SineaceAPP.Models.ViewModels.AutoCompleteViewModel;
using System.Collections.Generic;
using Microsoft.Maui.Devices.Sensors;
using SineaceAPP.Help;

namespace SineaceAPP.Pages.Mapa;

public partial class Mapa : ContentPage
{
    private readonly CargaPopup _carga = new();
    int count = 0;
    AutoCompleteViewModel mainPageViewModel = new AutoCompleteViewModel();
    private SuneduModel programasAcreditados = new SuneduModel();
    Location location1;
    private ObservableCollection<Sunedu> list = new ObservableCollection<Sunedu>();
    private ObservableCollection<Universidad> listEntidades = new ObservableCollection<Universidad>();
    private int IndexView = 3;
    MapaBottomSheet page = null;
    private MapaFiltroModel modelFiltrar = new MapaFiltroModel();
    private MapaViewModel _mapaViewModel;
    private int TipoInicio = 1;
    public Mapa()
    {
        InitializeComponent();
        _mapaViewModel = MapaViewModel.Instance;
    }

    protected async override void OnAppearing()
    {

        _mapaViewModel = MapaViewModel.Instance;
        if (_mapaViewModel.Local.Cordenadas != null)
        {
            if (!_mapaViewModel.Local.Cordenadas.Equals(""))
            {
                _mapaViewModel.Local.Cordenadas = "";
                IrALocal(_mapaViewModel.Local.Cordenadas);
            }
        }

        if (map.Pins.Count == 0)
        {
            alPantallaCarga.IsVisible = true;
            ShowStarAnimation();
            await Task.Delay(2000);
            alPantallaCarga.IsVisible = false;
            map.Pins.Clear();
            GetProgramas("acreditado");
            //search.Background = null;
            CargarInsituciones();
            await CheckLocation();

        }


    }
    private void IrALocal(string param)
    {
        string[] cordenadas = param.ToString().Split(";");
        grid1.HeightRequest = 10;
        grid1.IsVisible = false;
        double Latitud = double.Parse(cordenadas[0], System.Globalization.CultureInfo.InvariantCulture);
        double Logitud = double.Parse(cordenadas[1], System.Globalization.CultureInfo.InvariantCulture);
        Location location = new Location(Latitud, Logitud);
        map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(2000)));
        AbrirDetalleBottom(cordenadas[0], cordenadas[1], cordenadas[2], "");
    }

    public void GetProgramas(string Tipo)
    {
        SuneduService suneduService = new SuneduService();
        programasAcreditados = suneduService.GetProgramas(Tipo, "");
        mainPageViewModel.ListAutoCompleteDetalle = new ObservableCollection<AutoCompleteDetalle>();
        foreach (Sunedu item in programasAcreditados.data)
        {
            //int Exite = (list.Where(x => x.entidadCodigo.Equals(item.entidadCodigo))).ToList<Sunedu>().Count;
            //if (Exite == 0)
            list.Add(item);
        }
        for (int i = 0; i < IndexView; i++)
        {
            if (list.Count > i)
            {
                mainPageViewModel.ListAutoCompleteDetalle.Add(new AutoCompleteDetalle
                {
                    Universidad = list[i].entidadNombre,
                    Programa = list[i].programaNombreEstandar,
                    Direccion = list[i].localDireccion,
                    TipoPrograma = list[i].programaNivelAcademico,
                    Estado = Tipo.Equals("Acreditado") ? "Acreditado" : "",
                    Cordenadas = list[i].latitudUbicacion + ";" + list[i].longitudUbicacion + ";" + list[i].entidadNombre + ";" + list[i].entidadCodigo
                });
            }
        }
    }

    public void ColocarMakerUbicacion(Location location)
    {

        ImageSource Icon = ImageSource.FromResource("SineaceAPP.Resources.Images.Mapa.my_location_24.png");
        CustomPin pinKyiv = new CustomPin
        {
            Type = PinType.Place,
            Label = "Mi ubicación",
            Address = "",
            Location = location,
            ImageSource = Icon
        };
        map.Pins.Add(pinKyiv);
        map.MoveToRegion(MapSpan.FromCenterAndRadius(pinKyiv.Location, Distance.FromMeters(2000)));
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
        location1 = location as Location;
        //location1 = new Location(-12.117880, -77.033043);
        ColocarMakerUbicacion(location1);
    }


    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status == PermissionStatus.Granted)
            return status;
        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }
        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            // Prompt the user with additional information as to why the permission is needed
            await Shell.Current.DisplayAlert("Needs permissions", "BECAUSE!!!", "OK");
        }
        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        return status;
    }
    private async void MostrarPantallaCargar()
    {
        await _carga.MostrarAsync(this);
        await Task.Delay(4000);
        await _carga.CerrarAsync();
    }
    public async Task ShowStarAnimation()
    {
        //await Task.Delay(500);
        //imgLoader.IsAnimationPlaying = false;
        //imgLoader.IsAnimationPlaying = true;
        //base.OnAppearing();
        //await Task.Delay(500);
        img1.IsAnimationPlaying = false;
        img1.IsAnimationPlaying = true;
        img2.IsAnimationPlaying = false;
        img2.IsAnimationPlaying = true;
        AnimarLogo();
        AnimarLogo2();


    }

    private async void AnimarLogo()
    {
        ocultar();
        img1.IsVisible = true;
        await Task.Delay(100);
        ocultar();
        img2.IsVisible = true;
        await Task.Delay(100);
        ocultar();
        img3.IsVisible = true;
        await Task.Delay(100);
        AnimarLogo();
    }
    private async void AnimarLogo2()
    {
        ocultar2();
        imgd1.IsVisible = true;
        await Task.Delay(100);
        ocultar2();
        imgd2.IsVisible = true;
        await Task.Delay(100);
        ocultar2();
        imgd3.IsVisible = true;
        await Task.Delay(100);
        AnimarLogo2();
    }
    private void ocultar()
    {
        img1.IsVisible = false;
        img2.IsVisible = false;
        img3.IsVisible = false;
    }
    private void ocultar2()
    {
        imgd1.IsVisible = false;
        imgd2.IsVisible = false;
        imgd3.IsVisible = false;
    }
    //private void CerrarPantallaCargar()
    //{
    //    popup.Close();
    //}

    void Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            MyCollectionViews.ItemsSource = mainPageViewModel.ListAutoCompleteDetalle;
            grid1.HeightRequest = 10;
            grid1.IsVisible = false;
        } else
        {
            List<Sunedu> listTemp = list.Where(i => i.programaNombreEstandar.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            ObservableCollection<AutoCompleteDetalle> listView = new ObservableCollection<AutoCompleteDetalle>();

            for (int i = 0; i < IndexView; i++)
            {
                if (listTemp.Count > i)
                {
                    listView.Add(new AutoCompleteDetalle
                    {
                        Universidad = listTemp[i].entidadNombre,
                        Programa = listTemp[i].programaNombreEstandar,
                        Direccion = listTemp[i].localDireccion,
                        TipoPrograma = listTemp[i].programaNivelAcademico,
                        Estado = "Acreditado",
                        Cordenadas = listTemp[i].latitudUbicacion + ";" + listTemp[i].longitudUbicacion + ";" + listTemp[i].entidadNombre + ";" + list[i].entidadCodigo
                    });
                }
            }
            MyCollectionViews.ItemsSource = listView;
            grid1.HeightRequest = 700;
            grid1.IsVisible = true;
        }
    }


    void MarkerClick(object sender, PinClickedEventArgs e)
    {
        if (page != null)
            page.DismissAsync();
        CustomPin pin = (CustomPin)sender;
        string[] Variable = pin.Address.Split(';');
        double Latitud = double.Parse(Variable[9], System.Globalization.CultureInfo.InvariantCulture);
        double Logitud = double.Parse(Variable[10], System.Globalization.CultureInfo.InvariantCulture);
        Location locationEntidad = new Location(Latitud, Logitud);
        string distancia = ObtenerDistancia(location1, locationEntidad).ToString() + "Km";
        int CountProgramas = 0;
        List<Sunedu> Programas = programasAcreditados.data.Where(x => x.entidadNombre.ToLower().Equals(Variable[0].ToLower())).ToList();
        foreach (Sunedu Programa in Programas)
        {
            int length1 = Programa.latitudUbicacion.Length - 15;
            int length2 = Programa.longitudUbicacion.Length - 15;
            int length3 = Variable[9].Length - 15;
            int length4 = Variable[10].Length - 15;
            string Latitud1 = length1 > 0 ? Programa.latitudUbicacion.Substring(0, Programa.latitudUbicacion.Length - length1) : Programa.latitudUbicacion;
            string Logitud1 = length2 > 0 ? Programa.longitudUbicacion.Substring(0, Programa.longitudUbicacion.Length - length2) : Programa.longitudUbicacion;
            string Latitud2 = length3 > 0 ? Variable[9].Substring(0, Variable[9].Length - length3) : Variable[9];
            string Logitud2 = length4 > 0 ? Variable[10].Substring(0, Variable[10].Length - length4) : Variable[10];
            if (Latitud1.Equals(Latitud2) && Logitud1.Equals(Logitud2))
                CountProgramas++;
        }
        MapaBottomSheetModel mapaBottomSheetModel = new MapaBottomSheetModel
        {
            Institucion = Variable[0],
            Direccion = Variable[1],
            Ubigeo = Variable[4] + ", " + Variable[3] + ", " + Variable[2],
            TipoLocal = Variable[5] + " - " + (Variable[6].Equals("True") ? "LocalPrincipal" : (Variable[6].Equals("Sí") ? "LocalPrincipal" : "Filial")),
            Estado = Variable[7].Equals("1") ? "Acreditado" : Variable[8].Equals("0") ? "Licenciado" : "No Licenciado",
            Distancia = distancia,
            ProgramasAcreditados = CountProgramas.ToString() + " Programas Acredtirados"
        };
        page = new MapaBottomSheet(mapaBottomSheetModel);
        page.Dismissed += (s, e) =>
        {
            if (_mapaViewModel.Local.Institucion != null)
            {
                if (!_mapaViewModel.Local.Institucion.Equals(""))
                {
                    //string aux = _mapaViewModel.Local.Institucion.Replace("\\", "");
                    string text = _mapaViewModel.Local.Institucion;
                    text = text.Replace("\\", "").Replace("\"", "");
                    string Institucion = text;
                    _mapaViewModel.Local.Institucion = "";
                    Navigation.PushModalAsync(new SineaceAPP.Pages.Mapa.MapaProgramas(Institucion, location1, locationEntidad, 1));
                }
            }
        };
        page.ShowAsync(Window);


    }
    private double ObtenerDistancia(Location location, Location location2)
    {
        double distance = Location.CalculateDistance(location2, location, DistanceUnits.Kilometers);
        return Math.Round(distance, 2);
    }
    void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        //Debug.WriteLine($"MapClick: {e.Location.Latitude}, {e.Location.Longitude}");
    }
    public void CargarInsituciones()
    {
        SuneduService suneduService = new SuneduService();
        try
        {
            int count1 = 0;
            UniversidadModel universidadModel = suneduService.GetUniversidades();
            listEntidades = universidadModel.data;
            foreach (Universidad iten in universidadModel.data)
            {
                if (count1 != 100)
                {
                    if (iten.porc02.Equals("1"))
                    {
                        ColocarMarker(iten);
                        count1++;
                    }
                }
            }
            Console.WriteLine("Resultado 1:" + count1);
        } catch (Exception ex)
        {
            throw;
        }
    }
    public async void CargarInsitucionesFiltro()
    {
        SuneduService suneduService = new SuneduService();
        try
        {
            UniversidadModel universidadModel = suneduService.GetUniversidades();
            foreach (Universidad iten in universidadModel.data)
            {
                if (iten.porc02.Equals("1"))
                    if (FiltrarAux(iten))
                    {
                        if (!modelFiltrar.SinLimite)
                        {
                            double Latitud = double.Parse(iten.latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                            double Logitud = double.Parse(iten.longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
                            Location locationEntidad = new Location(Latitud, Logitud);
                            double distancia = ObtenerDistancia(location1, locationEntidad);
                            double distanciaLimite = Convert.ToDouble(modelFiltrar.Distancia);
                            if (distancia <= distanciaLimite)
                            {
                                await Task.Delay(200);
                                ColocarMarker(iten);
                            }
                        } else
                        {
                            await Task.Delay(200);
                            ColocarMarker(iten);
                        }
                    }
            }
            if (modelFiltrar.SinLimite)
                lblDistancia.Text = "-";
            else
                lblDistancia.Text = modelFiltrar.Distancia;
            //CerrarPantallaCargar();

        } catch (Exception ex)
        {
            throw;
        }
    }
    public void ColocarMarker(Universidad sunedu)
    {
        if (sunedu.latitudUbicacion != null && sunedu.longitudUbicacion != null)
        {
            double Latitud = double.Parse(sunedu.latitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
            double Logitud = double.Parse(sunedu.longitudUbicacion, System.Globalization.CultureInfo.InvariantCulture);
            ImageSource Icon = ImageSource.FromResource("SineaceAPP.Resources.Images.iconomapav1.png");
            switch (sunedu.tipoInstitucion)
            {
                case "Universidad":
                    Icon = ImageSource.FromResource("SineaceAPP.Resources.Images.iconomapav3.png");
                    break;
                case "Instituto":
                    Icon = ImageSource.FromResource("SineaceAPP.Resources.Images.iconomapav2.png");
                    break;

                case "Escuelas de las FFAA":
                    Icon = ImageSource.FromResource("SineaceAPP.Resources.Images.iconomapav1.png");
                    break;
            }
            string detalle = sunedu.entidadNombre + ";" + sunedu.localDireccion + ";" + sunedu.localDistrito + ";" + sunedu.localProvincia + ";" + sunedu.localDepartamento + ";" + sunedu.entidadTipoGestion + ";" + sunedu.esLocalPrincipal + ";" + sunedu.porc02 + ";" + sunedu.tipoInstitucion + ";" + sunedu.latitudUbicacion + ";" + sunedu.longitudUbicacion;
            CustomPin pinKyiv = new CustomPin
            {
                Type = PinType.Place,
                Label = string.Empty,
                Address = detalle,
                Location = new Location(Latitud, Logitud),
                ImageSource = Icon
            };
            pinKyiv.MarkerClicked += MarkerClick;


            map.Pins.Add(pinKyiv);
        }
    }

    private async void CerrarLeyenda()
    {
        await FlexLayout.TranslateTo(183, 0, 500);
        await lblLeyendaTitulo.TranslateTo(35, 0, 100);

    }
    private async void AbrirLeyenda()
    {
        await FlexLayout.TranslateTo(10, 0, 500);
        await lblLeyendaTitulo.TranslateTo(0, 0, 100);
    }
    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        AbrirLeyenda();
    }

    private void btnCerrarLeyenda_Clicked(object sender, EventArgs e)
    {
        CerrarLeyenda();
        btnAbrirLeyenda.IsVisible = true;
        btnCerrarLeyenda.IsVisible = false;
    }

    private void btnAbrirLeyenda_Clicked(object sender, EventArgs e)
    {
        AbrirLeyenda();
        btnAbrirLeyenda.IsVisible = false;
        btnCerrarLeyenda.IsVisible = true;
    }

    private async void btnFiltro_Clicked(object sender, EventArgs e)
    {
        var popup = new MapaFiltros(modelFiltrar);
        await MopupService.Instance.PushAsync(popup);
        var rvalue = await popup.PopupDismissedTask;
        modelFiltrar = (MapaFiltroModel)rvalue;
        if (!modelFiltrar.IB_Atras)
        {
            await Task.Delay(100);
            alPantallaCarga.IsVisible = true;
            ShowStarAnimation();
            map.Pins.Clear();
            CargarInsitucionesFiltro();
            await Task.Delay(2000);
            alPantallaCarga.IsVisible = false;

        }
    }
    private bool FiltrarAux(Universidad item)
    {
        bool validar = true;
        bool validarGestion = true;
        if (modelFiltrar.TipoGes1)
            if (item.entidadTipoGestion.Equals("Privado"))
                validarGestion = false;
        if (modelFiltrar.TipoGes2)
            if (!item.entidadTipoGestion.Equals("Privado"))
                validarGestion = false;
        if (validarGestion)
        {
            if (modelFiltrar.TipoIns1)
                if (!item.tipoInstitucion.Equals("Universidad"))
                    validar = false;
                else
                    return true;
            if (modelFiltrar.TipoIns2)
                if (!item.tipoInstitucion.Equals("Instituto"))
                    validar = false;
                else
                    return true;
            if (modelFiltrar.TipoIns3)
                if (!item.tipoInstitucion.Equals("Escuelas de las FFAA"))
                    validar = false;
                else
                    return true;
        }
        if (!validarGestion)
            validar = validarGestion;
        return validar;
    }



    private async void btnMiUbicacion_Clicked(object sender, EventArgs e)
    {
        await CheckLocation();
    }

    private void btnMenu_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var param = ((TappedEventArgs)e).Parameter;
        string[] cordenadas = param.ToString().Split(";");
        grid1.HeightRequest = 10;
        grid1.IsVisible = false;
        double Latitud = double.Parse(cordenadas[0], System.Globalization.CultureInfo.InvariantCulture);
        double Logitud = double.Parse(cordenadas[1], System.Globalization.CultureInfo.InvariantCulture);
        Location location = new Location(Latitud, Logitud);
        map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(2000)));
        AbrirDetalleBottom(cordenadas[0], cordenadas[1], cordenadas[2], cordenadas[3]);
    }
    private void AbrirDetalleBottom(string latitud, string longitud, string entidad, string entidadcodigo)
    {
        if (page != null)
            page.DismissAsync();
        double Latitud = double.Parse(latitud, System.Globalization.CultureInfo.InvariantCulture);
        double Logitud = double.Parse(longitud, System.Globalization.CultureInfo.InvariantCulture);
        Location locationEntidad = new Location(Latitud, Logitud);
        string distancia = ObtenerDistancia(location1, locationEntidad).ToString() + "Km";
        int CountProgramas = 0;
        List<Sunedu> Programas = programasAcreditados.data.Where(x => x.entidadNombre.ToLower().Equals(entidad.ToLower())).ToList();
        List<Universidad> listE = listEntidades.ToList();
        Universidad dtoEntidad = listE.Find(x => x.entidadNombre.Equals(entidad));
        //Universidad dtoEntidad = listE.Find(x=>x.entidadCodigo.Equals(entidadcodigo) && x.entidadNombre.Equals(entidad));
        foreach (Sunedu Programa in Programas)
        {
            int length1 = Programa.latitudUbicacion.Length - 15;
            int length2 = Programa.longitudUbicacion.Length - 15;
            int length3 = latitud.Length - 15;
            int length4 = longitud.Length - 15;
            string Latitud1 = length1 > 0 ? Programa.latitudUbicacion.Substring(0, Programa.latitudUbicacion.Length - length1) : Programa.latitudUbicacion;
            string Logitud1 = length2 > 0 ? Programa.longitudUbicacion.Substring(0, Programa.longitudUbicacion.Length - length2) : Programa.longitudUbicacion;
            string Latitud2 = length3 > 0 ? latitud.Substring(0, latitud.Length - length3) : latitud;
            string Logitud2 = length4 > 0 ? longitud.Substring(0, longitud.Length - length4) : longitud;
            if (Latitud1.Equals(Latitud2) && Logitud1.Equals(Logitud2))
                CountProgramas++;
        }
        MapaBottomSheetModel mapaBottomSheetModel = new MapaBottomSheetModel
        {
            Institucion = dtoEntidad.entidadNombre,
            Direccion = dtoEntidad.localDireccion,
            Ubigeo = dtoEntidad.localDepartamento + ", " + dtoEntidad.localProvincia + ", " + dtoEntidad.localDistrito,
            TipoLocal = dtoEntidad.entidadTipoGestion + " - " + (dtoEntidad.esLocalPrincipal.Equals("True") ? "LocalPrincipal" : (dtoEntidad.esLocalPrincipal.Equals("Sí") ? "LocalPrincipal" : "Filial")),
            Estado = dtoEntidad.porc02.Equals("1") ? "Acreditado" : dtoEntidad.porc02.Equals("0") ? "Licenciado" : "No Licenciado",
            Distancia = distancia,
            ProgramasAcreditados = CountProgramas.ToString() + " Programas Acredtirados"
        };
        page = new MapaBottomSheet(mapaBottomSheetModel);
        page.ShowAsync(Window);
    }
}

