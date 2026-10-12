using CommunityToolkit.Maui.Views;

namespace SineaceAPP.Pages.Otros;

public partial class PaginaCarga : Popup
{
    private CancellationTokenSource _cts;

    public PaginaCarga()
    {
        InitializeComponent();
    }

    // Las demoras (500 y 100 ms) son las del legacy; su centralizacion es de [EP4]-07 (PA-08, AD-21).
    public async Task ShowStarAnimation()
    {
        DetenerAnimacion();
        var cts = new CancellationTokenSource();
        _cts = cts;
        var ct = cts.Token;
        try
        {
            await Task.Delay(500, ct);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        img1.IsAnimationPlaying = false;
        img1.IsAnimationPlaying = true;
        img2.IsAnimationPlaying = false;
        img2.IsAnimationPlaying = true;
        _ = AnimarAlaIzquierdaAsync(ct);
        _ = AnimarAlaDerechaAsync(ct);
    }

    // Detiene los dos bucles de animacion (H-31: antes se llamaban recursivamente sin condicion de parada).
    public void DetenerAnimacion()
    {
        var cts = _cts;
        _cts = null;
        if (cts is null)
        {
            return;
        }
        cts.Cancel();
        cts.Dispose();
    }

    private async Task AnimarAlaIzquierdaAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                ocultar();
                img1.IsVisible = true;
                await Task.Delay(100, ct);
                ocultar();
                img2.IsVisible = true;
                await Task.Delay(100, ct);
                ocultar();
                img3.IsVisible = true;
                await Task.Delay(100, ct);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task AnimarAlaDerechaAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                ocultar2();
                imgd1.IsVisible = true;
                await Task.Delay(100, ct);
                ocultar2();
                imgd2.IsVisible = true;
                await Task.Delay(100, ct);
                ocultar2();
                imgd3.IsVisible = true;
                await Task.Delay(100, ct);
            }
        }
        catch (OperationCanceledException)
        {
        }
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
}
