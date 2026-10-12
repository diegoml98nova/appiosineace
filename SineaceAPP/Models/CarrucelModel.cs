using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models
{
    public class CarrucelModel
    {
        public string Titulo { get; set; }
        public string Titulo2 { get; set; }
        public string Titulo3 { get; set; }
        public string SubTitulo { get; set; }
        public string Descripcion3v1 { get; set; }
        public string Descripcion3v2 { get; set; }
        public string Descripcion3v3 { get; set; }
        public string Descripcion3v4 { get; set; }
        public string Descripcion3v5 { get; set; }
        public string Descripcion3v6 { get; set; }
        public string Descripcion1 { get; set; }
        public string Descripcion2 { get; set; }
        public string Descripcion3 { get; set; }
        public string Descripcion4 { get; set; }
        public string Descripcion5 { get; set; }
        public string Descripcion6 { get; set; }
        public string Descripcion7 { get; set; }
        public bool ImgVisible { get; set; } = false;
        public bool TituloVisible { get; set; } = true;
        public bool Titulo2Visible { get; set; } = false;
        public bool Titulo3Visible { get; set; } = false;
        public bool SubTituloVisible { get; set; } = false;
        public bool Descripcion3v1Visible { get; set; } = false;
        public bool Descripcion3v2Visible { get; set; } = false;
        public bool Descripcion3v3Visible { get; set; } = false;
        public bool Descripcion3v4Visible { get; set; } = false;
        public bool Descripcion3v5Visible { get; set; } = false;
        public bool Descripcion3v6Visible { get; set; } = false;
        public bool Descripcion1Visible { get; set; } = true;
        public bool Descripcion2Visible { get; set; } = true;
        public bool Descripcion3Visible { get; set; } = true;
        public bool Descripcion4Visible { get; set; } = true;
        public bool Descripcion5Visible { get; set; } = true;
        public bool Descripcion6Visible { get; set; } = true;
        public bool Descripcion7Visible { get; set; } = true;
        public int ButtomVisible { get; set; }
        public LayoutOptions Posicion { get; set; }
    }
    public class Carrucel2Model
    {
        public string Titulo { get; set; }
        public string SubTitulo1 { get; set; }
        public string SubTitulo11 { get; set; }
        public string SubTitulo2 { get; set; }
        public string SubTitulo22 { get; set; }
        public string Descripcion1 { get; set; }
        public string Descripcion2 { get; set; }
        public string Descripcion3 { get; set; }
        public bool TituloVisible { get; set; } = true;
        public bool SubTitulo1Visible { get; set; } = false;
        public bool SubTitulo11Visible { get; set; } = false;
        public bool SubTitulo2Visible { get; set; } = false;
        public bool SubTitulo22Visible { get; set; } = false;
        public bool Descripcion1Visible { get; set; } = true;
        public bool Descripcion2Visible { get; set; } = true;
        public bool Descripcion3Visible { get; set; } = true;
    }
}
