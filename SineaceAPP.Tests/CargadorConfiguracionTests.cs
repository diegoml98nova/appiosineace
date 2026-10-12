using Microsoft.Extensions.Logging;
using SineaceAPP.Services.Configuracion;
using Xunit;

namespace SineaceAPP.Tests;

public class CargadorConfiguracionTests
{
    private static string Archivo(string entorno) =>
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Config", $"appsettings.{entorno}.json"));

    private static string Json(string url, string timeout = "30", string tienda = "\"\"") =>
        "{\"Api\":{\"UrlBase\":\"" + url + "\",\"TimeoutSegundos\":" + timeout + "},\"App\":{\"UrlTiendaApp\":" + tienda + "}}";

    // CA-014.2 / CP-S1-35: la URL sale del archivo del entorno
    [Fact]
    public void Produccion_carga_la_url_de_produccion_con_timeout_30()
    {
        var logger = new DobleLogger();
        var cfg = CargadorConfiguracion.Desde(Archivo("Produccion"), logger);

        Assert.True(cfg.EsValida);
        Assert.Equal("https://appsineace2-servicioapp.azurewebsites.net/api/appSunedu/", cfg.UrlBaseApi.AbsoluteUri);
        Assert.Equal(30, cfg.TimeoutSegundos);
        Assert.Empty(logger.Registros);
    }

    [Fact]
    public void Pruebas_carga_la_url_de_pruebas_y_no_la_de_produccion()
    {
        var cfg = CargadorConfiguracion.Desde(Archivo("Pruebas"), new DobleLogger());

        Assert.True(cfg.EsValida);
        Assert.Equal("pruebas.ejemplo.invalid", cfg.UrlBaseApi.Host);
        Assert.DoesNotContain("azurewebsites", cfg.UrlBaseApi.AbsoluteUri);
    }

    // CA-019.1 (configuracion) / CP-S1-39 paso 2: UrlTiendaApp existe y esta vacia en cada entorno
    [Theory]
    [InlineData("Produccion")]
    [InlineData("Pruebas")]
    public void UrlTiendaApp_existe_con_valor_vacio_en_cada_entorno(string entorno)
    {
        var texto = Archivo(entorno);
        Assert.Contains("\"UrlTiendaApp\"", texto);

        var cfg = CargadorConfiguracion.Desde(texto, new DobleLogger());
        Assert.Equal(string.Empty, cfg.UrlTiendaApp);
    }

    // CA-014.3 / CP-S1-37: una URL que no sea https se rechaza y se registra
    [Theory]
    [InlineData("http://pruebas.ejemplo.invalid/api/")]
    [InlineData("HTTP://pruebas.ejemplo.invalid/api/")]
    [InlineData("ftp://pruebas.ejemplo.invalid/api/")]
    [InlineData("//pruebas.ejemplo.invalid/api/")]
    [InlineData("pruebas.ejemplo.invalid/api/")]
    [InlineData("")]
    public void Url_que_no_es_https_absoluta_se_rechaza_y_se_registra(string url)
    {
        var logger = new DobleLogger();
        var cfg = CargadorConfiguracion.Desde(Json(url), logger);

        Assert.False(cfg.EsValida);
        Assert.NotNull(cfg.Error);
        Assert.Contains(logger.Registros, r => r.Nivel == LogLevel.Error);
    }

    [Fact]
    public void Una_url_sin_barra_final_se_normaliza_con_barra()
    {
        var cfg = CargadorConfiguracion.Desde(Json("https://pruebas.ejemplo.invalid/api"), new DobleLogger());

        Assert.True(cfg.EsValida);
        Assert.Equal("https://pruebas.ejemplo.invalid/api/", cfg.UrlBaseApi.AbsoluteUri);
    }

    // RNF-SEG-DOS: el timeout no puede pasar de 30 s
    [Theory]
    [InlineData("0")]
    [InlineData("-5")]
    [InlineData("31")]
    [InlineData("\"30\"")]
    public void Timeout_fuera_de_1_a_30_se_rechaza(string timeout)
    {
        var cfg = CargadorConfiguracion.Desde(Json("https://pruebas.ejemplo.invalid/api/", timeout), new DobleLogger());

        Assert.False(cfg.EsValida);
    }

    [Theory]
    [InlineData("")]
    [InlineData("{")]
    [InlineData("[]")]
    [InlineData("{\"Api\":{}}")]
    public void Json_invalido_o_incompleto_se_rechaza(string json)
    {
        var logger = new DobleLogger();
        var cfg = CargadorConfiguracion.Desde(json, logger);

        Assert.False(cfg.EsValida);
        Assert.Contains(logger.Registros, r => r.Nivel == LogLevel.Error);
    }

    [Fact]
    public void Un_ensamblado_sin_el_recurso_de_configuracion_se_rechaza()
    {
        var logger = new DobleLogger();
        var cfg = CargadorConfiguracion.Cargar(typeof(CargadorConfiguracionTests).Assembly, logger);

        Assert.False(cfg.EsValida);
        Assert.Contains(logger.Registros, r => r.Nivel == LogLevel.Error);
    }
}
