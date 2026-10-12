using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models.ViewModels
{
    public class ProgramasViewModel
    {
        public string Programa { get; set; }
        public string Cui { get; set; }
        public string Institucion { get; set; }
        public string TipoPrograma { get; set; }
        public List<ResolucionesViewModel> Resoluciones { get; set; }
        public string LocalesAcreditados { get; set; }
        public string Distancia { get; set; }
        public bool AcreditadosVisible { get; set; }
        public bool AcreditadosVisible2 { get; set; }
        public List<LocalesViewModel> Locales { get; set; }
    }
}
