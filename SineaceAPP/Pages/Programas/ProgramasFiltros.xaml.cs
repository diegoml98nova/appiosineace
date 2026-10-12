using Mopups.Services;
using SineaceAPP.Models;

namespace SineaceAPP.Pages.Programas;

public partial class ProgramasFiltros 
{
    TaskCompletionSource<ProgramaFiltroModel> _taskCompletionSource;
    public Task<ProgramaFiltroModel> PopupDismissedTask => _taskCompletionSource.Task;
    public ProgramaFiltroModel ReturnValue { get; set; }
    ProgramaFiltroModel insititucionesModel = new ProgramaFiltroModel();
    public ProgramasFiltros()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _taskCompletionSource = new TaskCompletionSource<ProgramaFiltroModel>();
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        _taskCompletionSource.SetResult(ReturnValue);
    }

    private void btnChkIns1_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoIns1)
        {
            insititucionesModel.TipoIns1 = false;
            btnChkIns1.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoIns1 = true;
            btnChkIns1.Source = "check1png.png";
        }
    }

    private void btnChkIns2_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoIns2)
        {
            insititucionesModel.TipoIns2 = false;
            btnChkIns2.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoIns2 = true;
            btnChkIns2.Source = "check1png.png";
        }
    }

    private void btnChkIns3_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoIns3)
        {
            insititucionesModel.TipoIns3 = false;
            btnChkIns3.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoIns3 = true;
            btnChkIns3.Source = "check1png.png";
        }
    }

    private void btnChkGes1_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoGes1)
        {
            insititucionesModel.TipoGes1 = false;
            btnChkGes1.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoGes1 = true;
            btnChkGes1.Source = "check1png.png";

        }
    }

    private void btnChkGes2_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoGes2)
        {
            insititucionesModel.TipoGes2 = false;
            btnChkGes2.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoGes2 = true;
            btnChkGes2.Source = "check1png.png";
        }
    }

    private void btnBuscar_Clicked(object sender, EventArgs e)
    {
        ReturnValue = insititucionesModel;
        MopupService.Instance.PopAsync();
    }

    private void btnAtras_Clicked(object sender, EventArgs e)
    {
        ReturnValue = new ProgramaFiltroModel();
        ReturnValue.IB_Atras = true;
        MopupService.Instance.PopAsync();
    }
}