using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models
{
    public class CarrucelGuiaModel
    {
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string Descripcion1 { get; set; }
        public string Descripcion2 { get; set; }
        public string Descripcion3 { get; set; }
        public string Descripcion4 { get; set; }
        public string Descripcion5 { get; set; }
        public string Descripcion6 { get; set; }
        public string Descripcion7 { get; set; }
        public bool ImgVisible { get; set; } = false;
        public bool TituloVisible { get; set; } = true;
        public bool SubTituloVisible { get; set; } = false;
        public bool Descripcion1Visible { get; set; } = true;
        public bool Descripcion2Visible { get; set; } = true;
        public bool Descripcion3Visible { get; set; } = true;
        public bool Descripcion4Visible { get; set; } = true;
        public bool Descripcion5Visible { get; set; } = true;
        public bool Descripcion6Visible { get; set; } = true;
        public bool Descripcion7Visible { get; set; } = true;
        public LayoutOptions Posicion { get; set; }
        public ImageSource imgRuta { get; set; }
    }
}
