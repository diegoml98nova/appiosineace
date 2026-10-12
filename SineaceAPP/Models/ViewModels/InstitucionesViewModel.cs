using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models.ViewModels
{
    public class InstitucionesViewModel 
    {
        public string Institucion { get; set; }
        public string Tipo { get; set; }
        public string Gestion { get; set; }
        public string Estado { get; set; }
        public string ProgramasAcreditados { get; set; }
        public string Direccion { get; set; }
        public string Direccion2 { get; set; }
        public string Distancia { get; set; }
        public string Longitud { get; set; }
        public string Latitud { get; set; }
        public bool AcreditadosVisible{ get; set; }
    }
    //public class InstitucionesViewModel : INotifyPropertyChanged
    //{
    //    private String _Text;
    //    public String Text
    //    {
    //        get => _Text;
    //        set
    //        {
    //            if (_Text != value)
    //            {
    //                _Text = value;
    //                OnPropertyChanged();
    //            }
    //        }
    //    }
    //    private Boolean _Checked;
    //    public Boolean IsChecked
    //    {
    //        get => _Checked;
    //        set
    //        {
    //            if (_Checked != value)
    //            {
    //                _Checked = value;
    //                OnPropertyChanged();
    //            }
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void OnPropertyChanged([CallerMemberName] String propertyName = "")
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //}

    //public class Costumers : List<InstitucionesViewModel>, INotifyPropertyChanged
    //{
    //    private InstitucionesViewModel _CostumerSelected;
    //    public InstitucionesViewModel CostumerSelected
    //    {
    //        get => _CostumerSelected;
    //        set
    //        {
    //            if (_CostumerSelected != value)
    //            {
    //                if (_CostumerSelected != null) _CostumerSelected.IsChecked = false;
    //                _CostumerSelected = value;
    //                OnPropertyChanged();
    //                _CostumerSelected.IsChecked = true;
    //            }
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void OnPropertyChanged([CallerMemberName] String propertyName = "")
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }
    //}
}
