using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using SineaceAPP.Pages.Otros;

namespace SineaceAPP.Services.Dispositivo;

/// <summary>
/// Pantalla de carga (C-13) sobre la API de popups de CommunityToolkit.Maui 15 (AD-08, AD-21).
/// Una instancia por pagina: mostrar mientras ya esta visible no hace nada (guarda de reentrada)
/// y cerrar es idempotente. Al cerrar se detiene la animacion (H-31, RNF-PER-003).
/// </summary>
public sealed class CargaPopup
{
    private static readonly PopupOptions Opciones = new()
    {
        CanBeDismissedByTappingOutsideOfPopup = false,
        PageOverlayColor = Colors.Transparent,
        Shape = null,
        Shadow = null,
    };

    private PaginaCarga _popup;

    public bool Visible => _popup is not null;

    public async Task MostrarAsync(Page pagina)
    {
        if (_popup is not null)
        {
            return;
        }

        var popup = new PaginaCarga();
        _popup = popup;
        pagina.ShowPopup(popup, Opciones);
        await popup.ShowStarAnimation();
    }

    public async Task CerrarAsync()
    {
        var popup = _popup;
        if (popup is null)
        {
            return;
        }

        _popup = null;
        popup.DetenerAnimacion();
        try
        {
            await popup.CloseAsync();
        }
        catch (PopupNotFoundException)
        {
            // ya estaba cerrado: cerrar es idempotente
        }
    }
}
