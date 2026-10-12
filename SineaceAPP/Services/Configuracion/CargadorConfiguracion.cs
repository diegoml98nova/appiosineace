using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SineaceAPP.Services.Configuracion;

/// <summary>
/// Lee la configuracion del entorno (AD-05). El .csproj incrusta solo el archivo del entorno elegido con
/// la propiedad SineaceEntorno (por defecto Produccion) bajo el nombre logico NombreRecurso.
/// Una URL que no sea https absoluta, un timeout fuera de 1..30 s o un JSON invalido producen una
/// configuracion no valida y se registra el motivo (sin la URL completa del usuario: no hay datos de usuario).
/// </summary>
public static class CargadorConfiguracion
{
    public const string NombreRecurso = "SineaceAPP.appsettings.json";

    public static AppSettings Cargar(Assembly ensamblado, ILogger logger)
    {
        using var flujo = ensamblado.GetManifestResourceStream(NombreRecurso);
        if (flujo is null)
        {
            return Rechazar("No se encontro el recurso de configuracion " + NombreRecurso, logger);
        }

        using var lector = new StreamReader(flujo);
        return Desde(lector.ReadToEnd(), logger);
    }

    public static AppSettings Desde(string json, ILogger logger)
    {
        try
        {
            using var doc = JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
            var raiz = doc.RootElement;

            var urlTexto = Texto(raiz, "Api", "UrlBase");
            if (!Uri.TryCreate(urlTexto, UriKind.Absolute, out var url))
            {
                return Rechazar("Api:UrlBase no es una URL absoluta.", logger);
            }
            if (!string.Equals(url.Scheme, Uri.UriSchemeHttps, StringComparison.Ordinal))
            {
                return Rechazar("Api:UrlBase debe usar https (esquema recibido: " + url.Scheme + ").", logger);
            }
            if (!url.AbsolutePath.EndsWith('/'))
            {
                url = new Uri(url.GetLeftPart(UriPartial.Path) + "/" + url.Query);
            }

            if (!Entero(raiz, "Api", "TimeoutSegundos", out var timeout)
                || timeout < 1 || timeout > AppSettings.TimeoutMaximoSegundos)
            {
                return Rechazar("Api:TimeoutSegundos debe estar entre 1 y " + AppSettings.TimeoutMaximoSegundos + ".", logger);
            }

            return new AppSettings(url, timeout, Texto(raiz, "App", "UrlTiendaApp") ?? string.Empty);
        }
        catch (JsonException)
        {
            return Rechazar("El archivo de configuracion no es un JSON valido.", logger);
        }
    }

    private static AppSettings Rechazar(string motivo, ILogger logger)
    {
        logger?.LogError("Configuracion no valida: {Motivo}", motivo);
        return AppSettings.Invalida(motivo);
    }

    private static string Texto(JsonElement raiz, string seccion, string clave)
    {
        if (raiz.ValueKind == JsonValueKind.Object
            && raiz.TryGetProperty(seccion, out var s) && s.ValueKind == JsonValueKind.Object
            && s.TryGetProperty(clave, out var v) && v.ValueKind == JsonValueKind.String)
        {
            return v.GetString();
        }
        return null;
    }

    private static bool Entero(JsonElement raiz, string seccion, string clave, out int valor)
    {
        valor = 0;
        return raiz.ValueKind == JsonValueKind.Object
            && raiz.TryGetProperty(seccion, out var s) && s.ValueKind == JsonValueKind.Object
            && s.TryGetProperty(clave, out var v) && v.ValueKind == JsonValueKind.Number
            && v.TryGetInt32(out valor);
    }
}
