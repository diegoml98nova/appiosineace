namespace SineaceAPP.Services.Configuracion;

/// <summary>
/// Configuracion de la app por entorno (C-14, AD-05): URL base de la API, timeout y UrlTiendaApp.
/// Es de solo lectura. Si la configuracion no es valida, EsValida es false y Error dice por que;
/// en ese caso no se envia ninguna peticion (CA-014.3, ConfiguracionInvalida).
/// </summary>
public sealed class AppSettings
{
    public const int TimeoutMaximoSegundos = 30;

    public Uri UrlBaseApi { get; }
    public int TimeoutSegundos { get; }
    public string UrlTiendaApp { get; }
    public string Error { get; }
    public bool EsValida => Error is null;

    public AppSettings(Uri urlBaseApi, int timeoutSegundos, string urlTiendaApp)
    {
        UrlBaseApi = urlBaseApi;
        TimeoutSegundos = timeoutSegundos;
        UrlTiendaApp = urlTiendaApp ?? string.Empty;
    }

    private AppSettings(string error)
    {
        Error = error;
        UrlTiendaApp = string.Empty;
    }

    public static AppSettings Invalida(string error) => new AppSettings(error);
}
