using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models
{
    public class MapaFiltroModel
    {
        public bool TipoIns1 { get; set; }
        public bool TipoIns2 { get; set; }
        public bool TipoIns3 { get; set; }
        public bool TipoGes1 { get; set; }
        public bool TipoGes2 { get; set; }
        public string Distancia { get; set; }
        public bool SinLimite { get; set; } = true;
        public bool IB_Atras { get; set; }
        public int TipoMapa { get; set; }


    }
}
