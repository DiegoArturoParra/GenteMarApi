using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenteMarCore.Entities.Models
{
    [Table("m_nav_bas", Schema = "DBA")]
    public partial class m_nav_bas
    {
        [Key]
        public string identi { get; set; }
        public string grupo { get; set; }
        public string clase { get; set; }
        public string linea { get; set; }
        public string letras { get; set; }
        public string nom_naves { get; set; }
        public string nom_armad { get; set; }
        public string cedula_nit_propie { get; set; }
        public string tipo_cc_nit_propie { get; set; }
        public string expedido { get; set; }
        public string dire_propie { get; set; }
        public string tel_fax { get; set; }
        public string ciudad_tel { get; set; }
        public Nullable<System.DateTime> fech_compra { get; set; }
        public string carpeta { get; set; }
        public string activa_inacti { get; set; }
        public string cod_tipo { get; set; }
        public Nullable<System.DateTime> fech_ingreso { get; set; }
        public Nullable<decimal> control { get; set; }
        public string cod_pais { get; set; }
        public string afipa_nal { get; set; }
        public string puerto_matri { get; set; }
        public string puerto_registro { get; set; }
        public string nom_propie { get; set; }
        public string cc_nit_armador { get; set; }
        public string dire_armador { get; set; }
        public string cedula_nit_armador { get; set; }
        public string tel_fax_armador { get; set; }
        public string ciudad_armador { get; set; }
        public string numero_escritura_b { get; set; }
        public string notaria_ciudad_b { get; set; }
        public Nullable<System.DateTime> fecha_escritura_b { get; set; }
        public string expedicion_armador { get; set; }
        public Nullable<System.DateTime> fecha_constru { get; set; }
        public string correo_propi { get; set; }
        public string correo_arma { get; set; }
        public string activa { get; set; }
        public string norma { get; set; }
        public string tipo_buq { get; set; }
        public string subtipo { get; set; }
        public string servicio { get; set; }
        public string trafico { get; set; }
        public string observaciones { get; set; }
        public string navegacion { get; set; }
        public string puertooperacion { get; set; }
        public Nullable<decimal> maximo { get; set; }
        public string tiponave { get; set; }
        public string tiponavegacion { get; set; }
        public string categoria { get; set; }
        public Nullable<decimal> mmsi { get; set; }
        public Nullable<System.DateTime> fecha_entrega { get; set; }
        public string PUERTO_REGISTRO_SIGLA { get; set; }
        public string id_propietario_res { get; set; }
        public string id_propietario_res_armador { get; set; }
        public string IDENTIFICACION_PROPIETARIO_INSCRITO { get; set; }
        public string IDENTIFICACION_COMPANIA { get; set; }
    }
}
