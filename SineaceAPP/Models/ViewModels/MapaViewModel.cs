using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models.ViewModels
{
    public class MapaViewModel
    {
        public MapaViewModel()
        {
            Local = new LocalesViewModel();
        }
        public static MapaViewModel Instance { get; } = new MapaViewModel();
        private LocalesViewModel _pocal;
        public LocalesViewModel Local
        {
            get { return _pocal; }
            set { _pocal = value; }
        }
    }
}
