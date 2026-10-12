using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models
{

    public class Sunedu
    {
        public string entidadTipo { get; set; }
        public string entidadCodigo { get; set; }
        public string entidadNombre { get; set; }
        public string entidadCodigoTipoGestion { get; set; }
        public string entidadTipoGestion { get; set; }
        public string entidadTipoAutorizacion { get; set; }
        public string programaNombreEstandar { get; set; }
        public string programaModalidadEstudio { get; set; }
        public object programaTipoNivelAcademico { get; set; }
        public object programaDescripcionCineNivel1 { get; set; }
        public object programaDescripcionCineNivel2 { get; set; }
        public object programaDescripcionCineNivel3 { get; set; }
        public object entidadProgramaDescripcionCineNivel4 { get; set; }
        public string localDepartamento { get; set; }
        public string localProvincia { get; set; }
        public string localDistrito { get; set; }
        public string localDireccion { get; set; }
        public string localReferencia { get; set; }
        public string latitudUbicacion { get; set; }
        public string longitudUbicacion { get; set; }
        public string esLocalPrincipal { get; set; }
        public string etapaAcre { get; set; }
        public string resolucionUrl { get; set; }
        public string feciniAcre { get; set; }
        public string fecfinAcre { get; set; }
        public object porc01 { get; set; }
        public string porc02 { get; set; }
        public string programaCodigoEntidadPrograma { get; set; }
        public string cui { get; set; }
        public string des_especialidad { get; set; }
        public string programaNivelAcademico { get; set; }
        public string tipoInstitucion { get; set; }
        public object iD_DIRECCION { get; set; }
        public object iD_TIPOIE { get; set; }
        public double Distancia { get; set; }

    }

    public class SuneduModel
    {
        public string mensaje { get; set; }
        public List<Sunedu> data { get; set; }
    }

}
