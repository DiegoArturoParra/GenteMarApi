using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace GenteMarCore.Entities.Models
{
    public partial class GENTEMAR_DATOSBASICOS
    {
        public GENTEMAR_DATOSBASICOS(long id, int idEstado, DateTime FechaHoraCreacion, int UsuarioCreador, string nombres, string apellidos,
            int id_genero, string documento_identificacion, int id_tipo_documento, int?
            id_municipio_expedicion, string cod_pais, DateTime? fecha_expedicion, DateTime?
            fecha_vencimiento, DateTime fecha_nacimiento, string id_pais_nacimiento,
            string direccion, int id_municipio_residencia, string telefono,
            string correo_electronico, string numero_movil, int id_formacion_grado, string id_pais_residencia)
        {
            this.id_gentemar = id;
            this.FechaCreacion = FechaHoraCreacion;
            this.LoginCreacionId = UsuarioCreador;
            this.id_estado = idEstado;
            this.nombres = nombres;
            this.apellidos = apellidos;
            this.id_genero = id_genero;
            this.documento_identificacion = documento_identificacion;
            this.id_tipo_documento = id_tipo_documento;
            this.id_municipio_expedicion = id_municipio_expedicion;
            this.cod_pais = cod_pais;
            this.fecha_expedicion = fecha_expedicion;
            this.fecha_vencimiento = fecha_vencimiento;
            this.fecha_nacimiento = fecha_nacimiento;
            this.id_pais_nacimiento = id_pais_nacimiento;
            this.id_pais_residencia = id_pais_residencia;
            this.direccion = direccion;
            this.id_municipio_residencia = id_municipio_residencia;
            this.telefono = telefono;
            this.correo_electronico = correo_electronico;
            this.numero_movil = numero_movil.Trim();
            this.id_formacion_grado = id_formacion_grado;
        }

        [NotMapped]
        public HttpPostedFile Archivo { get; set; }
        [NotMapped]
        public GENTEMAR_OBSERVACIONES_DATOSBASICOS observacion { get; set; }

        public static GENTEMAR_DATOSBASICOS UpdateId(long Id, int idEstado, DateTime fecha_hora_creacion, int usuario_creador_registro, GENTEMAR_DATOSBASICOS data) =>
           new GENTEMAR_DATOSBASICOS(Id, idEstado, fecha_hora_creacion, usuario_creador_registro, data.nombres, data.apellidos, data.id_genero, data.documento_identificacion, data.id_tipo_documento,
               data.id_municipio_expedicion, data.cod_pais, data.fecha_expedicion, data.fecha_vencimiento, data.fecha_nacimiento, data.id_pais_nacimiento, data.direccion,
               data.id_municipio_residencia, data.telefono, data.correo_electronico, data.numero_movil, data.id_formacion_grado, data.id_pais_residencia);
    }
}
