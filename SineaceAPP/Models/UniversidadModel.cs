using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SineaceAPP.Models
{
    public class Universidad
    {
        public string entidadTipo { get; set; }
        public string entidadCodigo { get; set; }
        public string entidadNombre { get; set; }
        public string entidadCodigoTipoGestion { get; set; }
        public string entidadTipoGestion { get; set; }
        public object entidadTipoAutorizacion { get; set; }
        public object programaNombreEstandar { get; set; }
        public object programaModalidadEstudio { get; set; }
        public object programaTipoNivelAcademico { get; set; }
        public object programaDescripcionCineNivel1 { get; set; }
        public object programaDescripcionCineNivel2 { get; set; }
        public object programaDescripcionCineNivel3 { get; set; }
        public object entidadProgramaDescripcionCineNivel4 { get; set; }
        public string localDepartamento { get; set; }
        public string localProvincia { get; set; }
        public string localDistrito { get; set; }
        public string localDireccion { get; set; }
        public object localReferencia { get; set; }
        public string latitudUbicacion { get; set; }
        public string longitudUbicacion { get; set; }
        public string esLocalPrincipal { get; set; }
        public object etapaAcre { get; set; }
        public object resolucionUrl { get; set; }
        public object feciniAcre { get; set; }
        public object fecfinAcre { get; set; }
        public string porc01 { get; set; }
        public string porc02 { get; set; }
        public object programaCodigoEntidadPrograma { get; set; }
        public object cui { get; set; }
        public object des_especialidad { get; set; }
        public object programaNivelAcademico { get; set; }
        public string tipoInstitucion { get; set; }
        public string iD_DIRECCION { get; set; }
        public string iD_TIPOIE { get; set; }
        public double Distancia { get; set; }
        public int ProgramasAcreditados { get; set; }
    }

    public class UniversidadModel
    {
        public string mensaje { get; set; }
        public ObservableCollection<Universidad> data { get; set; }
    }
}
