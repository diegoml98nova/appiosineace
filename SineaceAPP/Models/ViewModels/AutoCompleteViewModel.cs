using SineaceAPP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models.ViewModels
{
    public class AutoCompleteViewModel : BindableObject
    {
        public class AutoCompleteDetalle
        {
            public string Universidad { get; set; }
            public string Programa { get; set; }
            public string Direccion { get; set; }
            public string TipoPrograma { get; set; }
            public string Distancia { get; set; }
            public string Estado { get; set; }
            public string Cordenadas { get; set; }

        }
        private ObservableCollection<AutoCompleteDetalle> listAutoCompleteDetalle;
        public ObservableCollection<AutoCompleteDetalle> ListAutoCompleteDetalle
        {
            get
            {
                return listAutoCompleteDetalle;
            }
            set
            {
                if (listAutoCompleteDetalle != value)
                {
                    listAutoCompleteDetalle = value;
                    OnPropertyChanged("UserItems");
                }
            }
        }
        //public AutoCompleteViewModel(string Tipo)
        //{
        //    SuneduService suneduService = new SuneduService();
        //    SuneduModel model= suneduService.GetProgramas(Tipo);
        //    ListAutoCompleteDetalle = new ObservableCollection<AutoCompleteDetalle>();
        //    foreach (Sunedu dto in model.data)
        //    {
        //        ListAutoCompleteDetalle.Add(new AutoCompleteDetalle
        //        {
        //            Universidad = dto.entidadNombre,
        //            Programa = dto.programaNombreEstandar,
        //            Direccion = dto.localDireccion,
        //            TipoPrograma = dto.programaNivelAcademico,
        //            Estado = Tipo.Equals("Acreditado") ? "Acreditado" : ""
        //        });
        //    }
        //}

    }
}
