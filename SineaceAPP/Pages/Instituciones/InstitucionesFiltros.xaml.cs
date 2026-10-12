using Mopups.Services;
using SineaceAPP.Models;

namespace SineaceAPP.Pages.Instituciones;

public partial class InstitucionesFiltros 
{
    TaskCompletionSource<InsititucionesModel> _taskCompletionSource;
    public Task<InsititucionesModel> PopupDismissedTask => _taskCompletionSource.Task;
    public InsititucionesModel ReturnValue { get; set; }
    InsititucionesModel insititucionesModel = new InsititucionesModel();
    public InstitucionesFiltros()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _taskCompletionSource = new TaskCompletionSource<InsititucionesModel>();
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        _taskCompletionSource.SetResult(ReturnValue);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        ReturnValue = insititucionesModel;
        MopupService.Instance.PopAsync();
    }

    private void btnAtras_Clicked(object sender, EventArgs e)
    {
        ReturnValue = new InsititucionesModel();
        ReturnValue.IB_Atras = true;
        MopupService.Instance.PopAsync();
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

    private void btnChkCal1_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoCal1)
        {
            insititucionesModel.TipoCal1 = false;
            btnChkCal1.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoCal1 = true;
            btnChkCal1.Source = "check1png.png";
        }
    }

    private void btnChkCal2_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoCal2)
        {
            insititucionesModel.TipoCal2 = false;
            btnChkCal2.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoCal2 = true;
            btnChkCal2.Source = "check1png.png";
        }
    }

    private void btnChkCal3_Clicked(object sender, EventArgs e)
    {
        if (insititucionesModel.TipoCal3)
        {
            insititucionesModel.TipoCal3 = false;
            btnChkCal3.Source = "check2png.png";
        } else
        {
            insititucionesModel.TipoCal3 = true;
            btnChkCal3.Source = "check1png.png";
        }
    }

    private void btnBuscar_Clicked(object sender, EventArgs e)
    {
        ReturnValue = insititucionesModel;
        MopupService.Instance.PopAsync();
    }
}
