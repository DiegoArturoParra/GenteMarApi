namespace GenteMarCore.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DBA.APLICACIONES",
                c => new
                {
                    ID_APLICACION = c.Int(nullable: false, identity: true),
                    NOMBRE = c.String(nullable: false, maxLength: 150, unicode: false),
                    VERSION = c.String(nullable: false, maxLength: 20, unicode: false),
                    FECHA_ACTUALIZACION = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ID_TIPO_AUTENTICACION = c.Int(nullable: false),
                    ID_ESTADO = c.Byte(nullable: false),
                    LLAVE_APLICACION = c.String(unicode: false),
                })
                .PrimaryKey(t => t.ID_APLICACION);

            CreateTable(
                "DBA.APLICACIONES_CAPITANIAS",
                c => new
                {
                    ID_CAPITANIA = c.Int(nullable: false, identity: true),
                    SIGLA_CAPITANIA = c.String(nullable: false, maxLength: 10, unicode: false),
                    DESCRIPCION = c.String(maxLength: 50, unicode: false),
                    ID_CATEGORIA = c.Int(nullable: false),
                    consecutivo = c.String(nullable: false, maxLength: 6, fixedLength: true, unicode: false),
                    ID_ESTADO = c.Short(nullable: false),
                })
                .PrimaryKey(t => t.ID_CAPITANIA)
                .ForeignKey("DBA.APLICACIONES_CATEGORIA", t => t.ID_CATEGORIA)
                .Index(t => t.ID_CATEGORIA);

            CreateTable(
                "DBA.APLICACIONES_CATEGORIA",
                c => new
                {
                    ID_CATEGORIA = c.Int(nullable: false, identity: true),
                    DESCRIPCION = c.String(maxLength: 50, unicode: false),
                    SIGLA_CATEGORIA = c.String(maxLength: 5, unicode: false),
                })
                .PrimaryKey(t => t.ID_CATEGORIA);

            CreateTable(
                "DBA.APLICACIONES_DEPARTAMENTO",
                c => new
                {
                    ID_DEPARTAMENTO = c.Int(nullable: false, identity: true),
                    CODIGO_DEPARTAMENTO = c.Int(nullable: false),
                    NOMBRE_DEPARTAMENTO = c.String(nullable: false, maxLength: 30, unicode: false),
                })
                .PrimaryKey(t => t.ID_DEPARTAMENTO);

            CreateTable(
                "DBA.APLICACIONES_ESTADO",
                c => new
                {
                    ID_ESTADO = c.Byte(nullable: false, identity: true),
                    DESCRIPCION = c.String(nullable: false, maxLength: 30, unicode: false),
                })
                .PrimaryKey(t => t.ID_ESTADO);

            CreateTable(
                "DBA.APLICACIONES_ROLES",
                c => new
                {
                    ID_ROL = c.Int(nullable: false, identity: true),
                    ROL = c.String(nullable: false, maxLength: 100, unicode: false),
                    DESCRIPCION = c.String(nullable: false, maxLength: 500, unicode: false),
                    FECHA_CREACION = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ID_APLICACION = c.Int(nullable: false),
                    ID_ESTADO = c.Byte(nullable: false),
                })
                .PrimaryKey(t => t.ID_ROL)
                .ForeignKey("DBA.APLICACIONES_ESTADO", t => t.ID_ESTADO)
                .Index(t => t.ID_ESTADO);

            CreateTable(
                "DBA.GENTEMAR_ESTADO_TITULO",
                c => new
                {
                    id_estado_tramite = c.Int(nullable: false, identity: true),
                    descripcion_tramite = c.String(maxLength: 100, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_estado_tramite);

            CreateTable(
                "DBA.APLICACIONES_GENERO",
                c => new
                {
                    ID_GENERO = c.Int(nullable: false, identity: true),
                    DESCRIPCION = c.String(maxLength: 100, unicode: false),
                })
                .PrimaryKey(t => t.ID_GENERO);

            CreateTable(
                "DBA.APLICACIONES_GRADO",
                c => new
                {
                    id_grado = c.Int(nullable: false, identity: true),
                    grado = c.String(maxLength: 50, unicode: false),
                    id_rango = c.Int(),
                    sigla = c.String(maxLength: 6, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_grado)
                .ForeignKey("DBA.APLICACIONES_RANGO", t => t.id_rango)
                .Index(t => t.id_rango);

            CreateTable(
                "DBA.APLICACIONES_RANGO",
                c => new
                {
                    id_rango = c.Int(nullable: false, identity: true),
                    rango = c.String(maxLength: 20, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_rango);

            CreateTable(
                "DBA.GENTEMAR_FORMACION_GRADO",
                c => new
                {
                    id_formacion_grado = c.Int(nullable: false, identity: true),
                    id_formacion = c.Int(),
                    id_grado = c.Int(),
                })
                .PrimaryKey(t => t.id_formacion_grado)
                .ForeignKey("DBA.APLICACIONES_GRADO", t => t.id_grado)
                .ForeignKey("DBA.GENTEMAR_FORMACION", t => t.id_formacion)
                .Index(t => t.id_formacion)
                .Index(t => t.id_grado);

            CreateTable(
                "DBA.GENTEMAR_FORMACION",
                c => new
                {
                    id_formacion = c.Int(nullable: false, identity: true),
                    formacion = c.String(maxLength: 50, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_formacion);

            CreateTable(
                "DBA.APLICACIONES_LOGIN_ROL",
                c => new
                {
                    ID_LOGIN_ROL = c.Int(nullable: false, identity: true),
                    ID_LOGIN = c.Int(nullable: false),
                    ID_ROL = c.Int(nullable: false),
                    FECHA_ASIGNACION = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    FECHA_MODIFICACION = c.DateTime(precision: 7, storeType: "datetime2"),
                    ID_ESTADO = c.Byte(),
                })
                .PrimaryKey(t => t.ID_LOGIN_ROL);

            CreateTable(
                "DBA.APLICACIONES_LOGIN_SUCURSAL",
                c => new
                {
                    ID_LOGIN_SUCURSAL = c.Int(nullable: false, identity: true),
                    ID_LOGIN = c.Int(nullable: false),
                    ID_SUCURSAL = c.Int(nullable: false),
                    ID_APLICACION = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_LOGIN_SUCURSAL);

            CreateTable(
                "DBA.APLICACIONES_LOGINS",
                c => new
                {
                    ID_LOGIN = c.Int(nullable: false, identity: true),
                    NOMBRES = c.String(nullable: false, maxLength: 100, unicode: false),
                    APELLIDOS = c.String(nullable: false, maxLength: 100, unicode: false),
                    LOGIN_NAME = c.String(nullable: false, maxLength: 50, unicode: false),
                    PASSWORD = c.String(nullable: false, maxLength: 250, unicode: false),
                    CORREO = c.String(nullable: false, maxLength: 50, unicode: false),
                    FECHA_CREACION = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    FECHA_MODIFICACION = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    NUMERO_INTENTOS = c.Byte(nullable: false),
                    ID_ESTADO = c.Byte(nullable: false),
                    ID_UNIDAD = c.Int(nullable: false),
                    ID_TIPO_ESTADO = c.Byte(nullable: false),
                    ID_USUARIO_REGISTRO = c.Int(nullable: false),
                    ID_CAPITANIA = c.Int(),
                })
                .PrimaryKey(t => t.ID_LOGIN);

            CreateTable(
                "DBA.APLICACIONES_MENU",
                c => new
                {
                    ID_MENU = c.Int(nullable: false, identity: true),
                    VISTA = c.String(nullable: false, maxLength: 50, unicode: false),
                    CONTROLADOR = c.String(nullable: false, maxLength: 50, unicode: false),
                    NOMBRE = c.String(nullable: false, maxLength: 50, unicode: false),
                    ID_PADRE = c.Int(nullable: false),
                    ID_APLICACION = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_MENU);

            CreateTable(
                "DBA.APLICACIONES_ROL_MENU",
                c => new
                {
                    ID_ROL_MENU = c.Int(nullable: false, identity: true),
                    ID_ROL = c.Int(nullable: false),
                    ID_MENU = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_ROL_MENU)
                .ForeignKey("DBA.APLICACIONES_MENU", t => t.ID_MENU)
                .Index(t => t.ID_MENU);

            CreateTable(
                "DBA.APLICACIONES_MUNICIPIO",
                c => new
                {
                    ID_MUNICIPIO = c.Int(nullable: false, identity: true),
                    CODIGO_MUNICIPIO = c.Int(nullable: false),
                    NOMBRE_MUNICIPIO = c.String(),
                    ID_DEPARTAMENTO = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_MUNICIPIO);

            CreateTable(
                "DBA.APLICACIONES_TIPO_DOCUMENTO",
                c => new
                {
                    ID_TIPO_DOCUMENTO = c.Int(nullable: false, identity: true),
                    DESCRIPCION = c.String(nullable: false, maxLength: 50, unicode: false),
                    SIGLA = c.String(maxLength: 6, unicode: false),
                })
                .PrimaryKey(t => t.ID_TIPO_DOCUMENTO);

            CreateTable(
                "DBA.APLICACIONES_TIPO_SOLICITUD",
                c => new
                {
                    ID_TIPO_SOLICITUD = c.Int(nullable: false, identity: true),
                    DESCRIPCION = c.String(maxLength: 100, unicode: false),
                })
                .PrimaryKey(t => t.ID_TIPO_SOLICITUD);

            CreateTable(
                "DBA.GENTEMAR_TITULOS",
                c => new
                {
                    id_titulo = c.Long(nullable: false, identity: true),
                    id_cargo_regla = c.Int(nullable: false),
                    id_gentemar = c.Long(nullable: false),
                    fecha_vencimiento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    fecha_expedicion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    cod_pais = c.String(maxLength: 3, fixedLength: true, unicode: false),
                    id_estado_tramite = c.Int(nullable: false),
                    id_capitania = c.Int(nullable: false),
                    id_tipo_solicitud = c.Int(nullable: false),
                    radicado = c.String(unicode: false),
                    id_capitania_firmante = c.Int(nullable: false),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_titulo)
                .ForeignKey("DBA.GENTEMAR_ESTADO_TITULO", t => t.id_estado_tramite, cascadeDelete: true)
                .ForeignKey("DBA.APLICACIONES_TIPO_SOLICITUD", t => t.id_tipo_solicitud)
                .Index(t => t.id_estado_tramite)
                .Index(t => t.id_tipo_solicitud);

            CreateTable(
                "DBA.GENTEMAR_ACLARACION_ANTECEDENTES",
                c => new
                {
                    id_aclaracion = c.Long(nullable: false, identity: true),
                    id_antecedente = c.Long(nullable: false),
                    id_entidad = c.Int(nullable: false),
                    descripcion = c.String(nullable: false, unicode: false),
                    verificacion_exitosa = c.Boolean(nullable: false),
                    numero_expediente = c.String(nullable: false),
                    fecha_respuesta_entidad = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_aclaracion);

            CreateTable(
                "DBA.GENTEMAR_ACTIVIDAD",
                c => new
                {
                    id_actividad = c.Int(nullable: false, identity: true),
                    actividad = c.String(unicode: false),
                    activo = c.Boolean(),
                    id_tipo_licencia = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id_actividad)
                .ForeignKey("DBA.GENTEMAR_TIPO_LICENCIA", t => t.id_tipo_licencia, cascadeDelete: true)
                .Index(t => t.id_tipo_licencia);

            CreateTable(
                "DBA.GENTEMAR_TIPO_LICENCIA",
                c => new
                {
                    id_tipo_licencia = c.Int(nullable: false, identity: true),
                    tipo_licencia = c.String(unicode: false),
                    activo = c.Boolean(),
                })
                .PrimaryKey(t => t.id_tipo_licencia);

            CreateTable(
                "DBA.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA",
                c => new
                {
                    id_actividad_seccion_licencia = c.Int(nullable: false, identity: true),
                    id_actividad = c.Int(nullable: false),
                    id_seccion = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id_actividad_seccion_licencia)
                .ForeignKey("DBA.GENTEMAR_ACTIVIDAD", t => t.id_actividad, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_SECCION_LICENCIAS", t => t.id_seccion, cascadeDelete: true)
                .Index(t => t.id_actividad)
                .Index(t => t.id_seccion);

            CreateTable(
                "DBA.GENTEMAR_SECCION_LICENCIAS",
                c => new
                {
                    id_seccion = c.Int(nullable: false, identity: true),
                    actividad_a_bordo = c.String(unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_seccion);

            CreateTable(
                "DBA.GENTEMAR_ANTECEDENTES",
                c => new
                {
                    id_antecedente = c.Long(nullable: false, identity: true),
                    id_gentemar_antecedente = c.Long(nullable: false),
                    numero_sgdea = c.String(unicode: false),
                    fecha_sgdea = c.DateTime(precision: 7, storeType: "datetime2"),
                    id_capitania = c.Int(nullable: false),
                    id_estado_antecedente = c.Int(nullable: false),
                    id_tipo_tramite = c.Int(nullable: false),
                    fecha_solicitud_sede_central = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    fecha_aprobacion = c.DateTime(precision: 7, storeType: "datetime2"),
                    fecha_vigencia = c.DateTime(precision: 7, storeType: "datetime2"),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_antecedente)
                .ForeignKey("DBA.APLICACIONES_CAPITANIAS", t => t.id_capitania, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_ESTADO_ANTECEDENTE", t => t.id_estado_antecedente, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_TIPO_TRAMITE", t => t.id_tipo_tramite, cascadeDelete: true)
                .Index(t => t.id_capitania)
                .Index(t => t.id_estado_antecedente)
                .Index(t => t.id_tipo_tramite);

            CreateTable(
                "DBA.GENTEMAR_ESTADO_ANTECEDENTE",
                c => new
                {
                    id_estado_antecedente = c.Int(nullable: false, identity: true),
                    activo = c.Boolean(nullable: false),
                    descripcion_estado_antecedente = c.String(maxLength: 250, unicode: false),
                })
                .PrimaryKey(t => t.id_estado_antecedente);

            CreateTable(
                "DBA.GENTEMAR_TIPO_TRAMITE",
                c => new
                {
                    id_tipo_tramite = c.Int(nullable: false, identity: true),
                    activo = c.Boolean(nullable: false),
                    descripcion_tipo_tramite = c.String(maxLength: 250, unicode: false),
                })
                .PrimaryKey(t => t.id_tipo_tramite);

            CreateTable(
                "DBA.GENTEMAR_ANTECEDENTES_DATOSBASICOS",
                c => new
                {
                    id_gentemar_antecedente = c.Long(nullable: false, identity: true),
                    id_tipo_documento = c.Int(nullable: false),
                    identificacion = c.String(maxLength: 20),
                    nombres = c.String(),
                    apellidos = c.String(),
                    fecha_nacimiento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_creador_registro = c.String(nullable: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_gentemar_antecedente);

            CreateTable(
                "DBA.GENTEMAR_CAPACIDAD",
                c => new
                {
                    id_capacidad = c.Int(nullable: false, identity: true),
                    capacidad = c.String(unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_capacidad);

            CreateTable(
                "DBA.GENTEMAR_CARGO_HABILITACION",
                c => new
                {
                    id_habilitacion = c.Int(nullable: false),
                    id_cargo_regla = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.id_habilitacion, t.id_cargo_regla })
                .ForeignKey("DBA.GENTEMAR_HABILITACION", t => t.id_habilitacion, cascadeDelete: true)
                .Index(t => t.id_habilitacion);

            CreateTable(
                "DBA.GENTEMAR_HABILITACION",
                c => new
                {
                    id_habilitacion = c.Int(nullable: false, identity: true),
                    habilitacion = c.String(unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_habilitacion);

            CreateTable(
                "DBA.GENTEMAR_CARGO_LICENCIA",
                c => new
                {
                    id_cargo_licencia = c.Int(nullable: false, identity: true),
                    cargo_licencia = c.String(unicode: false),
                    codigo_licencia = c.String(unicode: false),
                    vigencia = c.Decimal(nullable: false, precision: 30, scale: 6),
                    activo = c.Boolean(nullable: false),
                    id_actividad_seccion_licencia = c.Int(nullable: false),
                    id_seccion_clase = c.Int(nullable: false),
                    nave = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_cargo_licencia);

            CreateTable(
                "DBA.GENTEMAR_CARGO_LICENCIA_CATEGORIA",
                c => new
                {
                    id_cargo_categoria = c.Int(nullable: false, identity: true),
                    id_categoria = c.Int(nullable: false),
                    id_cargo_licencia = c.Int(nullable: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_cargo_categoria);

            CreateTable(
                "DBA.GENTEMAR_CARGO_LICENCIA_LIMITANTE",
                c => new
                {
                    id_cargo_licencia_limitante = c.Int(nullable: false, identity: true),
                    id_cargo_licencia = c.Int(nullable: false),
                    id_limitante = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id_cargo_licencia_limitante);

            CreateTable(
                "DBA.GENTEMAR_CARGO_LIMITACION",
                c => new
                {
                    id_cargo_limitacion = c.Int(nullable: false, identity: true),
                    id_limitacion = c.Int(),
                    id_cargo_licencia = c.Int(),
                })
                .PrimaryKey(t => t.id_cargo_limitacion);

            CreateTable(
                "DBA.GENTEMAR_CARGO_TITULO",
                c => new
                {
                    id_cargo_titulo = c.Int(nullable: false, identity: true),
                    cargo = c.String(unicode: false),
                    id_clase = c.Int(nullable: false),
                    id_seccion = c.Int(nullable: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_cargo_titulo);

            CreateTable(
                "DBA.GENTEMAR_REGLA_CARGO",
                c => new
                {
                    id_cargo_regla = c.Int(nullable: false, identity: true),
                    id_regla = c.Int(nullable: false),
                    id_cargo_titulo = c.Int(nullable: false),
                    id_nivel = c.Int(nullable: false),
                    id_capacidad = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id_cargo_regla)
                .ForeignKey("DBA.GENTEMAR_CAPACIDAD", t => t.id_capacidad, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_CARGO_TITULO", t => t.id_cargo_titulo, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_NIVEL", t => t.id_nivel, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_REGLA", t => t.id_regla, cascadeDelete: true)
                .Index(t => t.id_regla)
                .Index(t => t.id_cargo_titulo)
                .Index(t => t.id_nivel)
                .Index(t => t.id_capacidad);

            CreateTable(
                "DBA.GENTEMAR_NIVEL",
                c => new
                {
                    id_nivel = c.Int(nullable: false, identity: true),
                    nivel = c.String(maxLength: 100, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_nivel);

            CreateTable(
                "DBA.GENTEMAR_REGLA",
                c => new
                {
                    id_regla = c.Int(nullable: false, identity: true),
                    Regla = c.String(maxLength: 20, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_regla);

            CreateTable(
                "DBA.GENTEMAR_CLASE_LICENCIAS",
                c => new
                {
                    id_clase = c.Int(nullable: false, identity: true),
                    descripcion_clase = c.String(maxLength: 100, unicode: false),
                    sigla = c.String(maxLength: 2, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_clase);

            CreateTable(
                "DBA.GENTEMAR_CLASE_TITULOS",
                c => new
                {
                    id_clase = c.Int(nullable: false, identity: true),
                    descripcion_clase = c.String(maxLength: 100, unicode: false),
                    sigla = c.String(maxLength: 2, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_clase);

            CreateTable(
                "DBA.GENTEMAR_DATOSBASICOS",
                c => new
                {
                    id_gentemar = c.Long(nullable: false, identity: true),
                    documento_identificacion = c.String(maxLength: 16, unicode: false),
                    id_tipo_documento = c.Int(nullable: false),
                    id_municipio_expedicion = c.Int(),
                    cod_pais = c.String(maxLength: 3, fixedLength: true, unicode: false),
                    fecha_expedicion = c.DateTime(precision: 7, storeType: "datetime2"),
                    fecha_vencimiento = c.DateTime(precision: 7, storeType: "datetime2"),
                    nombres = c.String(unicode: false),
                    apellidos = c.String(unicode: false),
                    id_genero = c.Int(nullable: false),
                    fecha_nacimiento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    id_pais_nacimiento = c.Int(),
                    id_pais_residencia = c.Int(),
                    direccion = c.String(maxLength: 100, unicode: false),
                    id_municipio_residencia = c.Int(nullable: false),
                    telefono = c.String(maxLength: 18, unicode: false),
                    correo_electronico = c.String(maxLength: 100, unicode: false),
                    numero_movil = c.String(maxLength: 20, unicode: false),
                    id_estado = c.Int(nullable: false),
                    id_formacion_grado = c.Int(nullable: false),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_gentemar);

            CreateTable(
                "DBA.GENTEMAR_ENTIDAD",
                c => new
                {
                    id_entidad = c.Int(nullable: false, identity: true),
                    entidad = c.String(maxLength: 100, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_entidad);

            CreateTable(
                "DBA.GENTEMAR_ESTADO",
                c => new
                {
                    id_estado = c.Int(nullable: false, identity: true),
                    descripcion = c.String(nullable: false, unicode: false),
                    sigla = c.String(nullable: false, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_estado);

            CreateTable(
                "DBA.GENTEMAR_ESTADO_LICENCIA",
                c => new
                {
                    id_estado_licencias = c.Int(nullable: false, identity: true),
                    descripcion_estado = c.String(maxLength: 100, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_estado_licencias);

            CreateTable(
                "DBA.GENTEMAR_FOTOGRAFIA",
                c => new
                {
                    id_fotografia = c.Int(nullable: false, identity: true),
                    id_gentemar = c.Long(nullable: false),
                    detalle = c.String(maxLength: 100, unicode: false),
                    fotografia = c.String(nullable: false, unicode: false),
                    activo = c.Boolean(nullable: false),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_fotografia);

            CreateTable(
                "DBA.GENTEMAR_FUNCION",
                c => new
                {
                    id_funcion = c.Int(nullable: false, identity: true),
                    funcion = c.String(unicode: false),
                    limitacion_funcion = c.String(unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_funcion);

            CreateTable(
                "DBA.GENTEMAR_HISTORIAL_DOCUMENTO",
                c => new
                {
                    id_historial_documento = c.Int(nullable: false, identity: true),
                    id_gentemar = c.Long(nullable: false),
                    documento_identificacion = c.String(),
                    id_tipo_documento = c.Int(nullable: false),
                    fecha_cambio = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_historial_documento);

            CreateTable(
                "DBA.GENTEMAR_LICENCIA_NAVES",
                c => new
                {
                    id_licencia_nave = c.Long(nullable: false, identity: true),
                    id_licencia = c.Long(nullable: false),
                    identi = c.String(),
                })
                .PrimaryKey(t => t.id_licencia_nave);

            CreateTable(
                "DBA.GENTEMAR_LICENCIAS",
                c => new
                {
                    id_licencia = c.Long(nullable: false, identity: true),
                    id_gentemar = c.Long(nullable: false),
                    id_cargo_licencia = c.Int(),
                    fecha_expedicion = c.DateTime(precision: 7, storeType: "datetime2"),
                    fecha_vencimiento = c.DateTime(precision: 7, storeType: "datetime2"),
                    id_capitania = c.Int(),
                    id_estado_licencia = c.Int(),
                    id_tramite = c.Int(),
                    radicado = c.String(unicode: false),
                    id_capitania_firmante = c.Int(),
                    activo = c.Boolean(),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_licencia);

            CreateTable(
                "DBA.GENTEMAR_LIMITACION",
                c => new
                {
                    id_limitacion = c.Int(nullable: false, identity: true),
                    limitaciones = c.String(maxLength: 600, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_limitacion);

            CreateTable(
                "DBA.GENTEMAR_LIMITANTE",
                c => new
                {
                    id_limitante = c.Int(nullable: false, identity: true),
                    descripcion = c.String(),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_limitante);

            CreateTable(
                "DBA.GENTEMAR_OBSERVACIONES_ANTECEDENTES",
                c => new
                {
                    id_observacion = c.Long(nullable: false, identity: true),
                    id_antecedente = c.Long(nullable: false),
                    observacion = c.String(unicode: false),
                    ruta_archivo = c.String(unicode: false),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_observacion);

            CreateTable(
                "DBA.GENTEMAR_OBSERVACIONES_DATOSBASICOS",
                c => new
                {
                    id_observacion = c.Long(nullable: false, identity: true),
                    id_gentemar = c.Int(nullable: false),
                    observacion = c.String(unicode: false),
                    ruta_archivo = c.String(unicode: false),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_observacion);

            CreateTable(
                "DBA.GENTEMAR_OBSERVACIONES_LICENCIAS",
                c => new
                {
                    id_observacion = c.Long(nullable: false, identity: true),
                    id_licencia = c.Int(nullable: false),
                    observacion = c.String(),
                    ruta_archivo = c.String(),
                    usuario_creador_registro = c.String(nullable: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_observacion);

            CreateTable(
                "DBA.GENTEMAR_OBSERVACIONES_TITULOS",
                c => new
                {
                    id_observacion = c.Long(nullable: false, identity: true),
                    id_titulo = c.Long(nullable: false),
                    observacion = c.String(unicode: false),
                    ruta_archivo = c.String(unicode: false),
                    usuario_creador_registro = c.String(nullable: false, unicode: false),
                    fecha_hora_creacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    usuario_actualiza_registro = c.String(unicode: false),
                    fecha_hora_actualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.id_observacion);

            CreateTable(
                "DBA.GENTEMAR_REGLA_FUNCION",
                c => new
                {
                    id_regla = c.Int(nullable: false),
                    id_funcion = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.id_regla, t.id_funcion })
                .ForeignKey("DBA.GENTEMAR_FUNCION", t => t.id_funcion, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_REGLA", t => t.id_regla, cascadeDelete: true)
                .Index(t => t.id_regla)
                .Index(t => t.id_funcion);

            CreateTable(
                "DBA.GENTEMAR_REPOSITORIO_ARCHIVOS",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    IdAplicacion = c.Int(),
                    IdModulo = c.String(),
                    NombreModulo = c.String(),
                    TipoDocumento = c.String(),
                    FechaCargue = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    Nombre = c.String(),
                    DescripcionDocumento = c.String(),
                    NombreArchivo = c.String(),
                    RutaArchivo = c.String(),
                    IdUsuarioCreador = c.String(),
                    FechaHoraCreacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    IdUsuarioUltimaActualizacion = c.String(),
                    FechaHoraUltimaActualizacion = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "DBA.GENTEMAR_SECCION_CLASE",
                c => new
                {
                    id_seccion_clase = c.Int(nullable: false, identity: true),
                    id_clase = c.Int(nullable: false),
                    id_seccion = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id_seccion_clase)
                .ForeignKey("DBA.GENTEMAR_CLASE_LICENCIAS", t => t.id_clase, cascadeDelete: true)
                .ForeignKey("DBA.GENTEMAR_SECCION_LICENCIAS", t => t.id_seccion, cascadeDelete: true)
                .Index(t => t.id_clase)
                .Index(t => t.id_seccion);

            CreateTable(
                "DBA.GENTEMAR_SECCION_TITULOS",
                c => new
                {
                    id_seccion = c.Int(nullable: false, identity: true),
                    actividad_a_bordo = c.String(maxLength: 100, unicode: false),
                    activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id_seccion);

            CreateTable(
                "DBA.GENTEMAR_TERRITORIO",
                c => new
                {
                    id_territorio = c.Int(nullable: false, identity: true),
                    territorio = c.String(unicode: false),
                    activo = c.Boolean(),
                })
                .PrimaryKey(t => t.id_territorio);

            CreateTable(
                "DBA.GENTEMAR_TITULO_FUNCION",
                c => new
                {
                    id_titulo = c.Long(nullable: false),
                    id_funcion = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.id_titulo, t.id_funcion });

            CreateTable(
                "DBA.GENTEMAR_TITULO_HABILITACION",
                c => new
                {
                    id_titulo = c.Long(nullable: false),
                    id_habilitacion = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.id_titulo, t.id_habilitacion });

            CreateTable(
                "DBA.m_nav_bas",
                c => new
                {
                    identi = c.String(nullable: false, maxLength: 128),
                    grupo = c.String(),
                    clase = c.String(),
                    linea = c.String(),
                    letras = c.String(),
                    nom_naves = c.String(),
                    nom_armad = c.String(),
                    cedula_nit_propie = c.String(),
                    tipo_cc_nit_propie = c.String(),
                    expedido = c.String(),
                    dire_propie = c.String(),
                    tel_fax = c.String(),
                    ciudad_tel = c.String(),
                    fech_compra = c.DateTime(precision: 7, storeType: "datetime2"),
                    carpeta = c.String(),
                    activa_inacti = c.String(),
                    cod_tipo = c.String(),
                    fech_ingreso = c.DateTime(precision: 7, storeType: "datetime2"),
                    control = c.Decimal(precision: 18, scale: 2),
                    cod_pais = c.String(),
                    afipa_nal = c.String(),
                    puerto_matri = c.String(),
                    puerto_registro = c.String(),
                    nom_propie = c.String(),
                    cc_nit_armador = c.String(),
                    dire_armador = c.String(),
                    cedula_nit_armador = c.String(),
                    tel_fax_armador = c.String(),
                    ciudad_armador = c.String(),
                    numero_escritura_b = c.String(),
                    notaria_ciudad_b = c.String(),
                    fecha_escritura_b = c.DateTime(precision: 7, storeType: "datetime2"),
                    expedicion_armador = c.String(),
                    fecha_constru = c.DateTime(precision: 7, storeType: "datetime2"),
                    correo_propi = c.String(),
                    correo_arma = c.String(),
                    activa = c.String(),
                    norma = c.String(),
                    tipo_buq = c.String(),
                    subtipo = c.String(),
                    servicio = c.String(),
                    trafico = c.String(),
                    observaciones = c.String(),
                    navegacion = c.String(),
                    puertooperacion = c.String(),
                    maximo = c.Decimal(precision: 18, scale: 2),
                    tiponave = c.String(),
                    tiponavegacion = c.String(),
                    categoria = c.String(),
                    mmsi = c.Decimal(precision: 18, scale: 2),
                    fecha_entrega = c.DateTime(precision: 7, storeType: "datetime2"),
                    PUERTO_REGISTRO_SIGLA = c.String(),
                    id_propietario_res = c.String(),
                    id_propietario_res_armador = c.String(),
                    IDENTIFICACION_PROPIETARIO_INSCRITO = c.String(),
                    IDENTIFICACION_COMPANIA = c.String(),
                })
                .PrimaryKey(t => t.identi);

            CreateTable(
                "DBA.DimImpresion",
                c => new
                {
                    idimpresion = c.Decimal(nullable: false, precision: 18, scale: 2),
                    idusuario = c.Int(),
                    cedula = c.String(),
                    impreso = c.Boolean(nullable: false),
                    fechaimpreso = c.DateTime(precision: 7, storeType: "datetime2"),
                    etiqueta = c.Int(),
                    libreta = c.Int(),
                    idPersona = c.Int(),
                    idDocumentos = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.idimpresion);

            CreateTable(
                "DBA.DimPersonas",
                c => new
                {
                    idpersona = c.Decimal(nullable: false, precision: 18, scale: 2),
                    idusuario = c.Int(),
                    cedula = c.String(),
                    firmanueva = c.String(),
                    huellanueva = c.String(),
                    fechacaptura = c.DateTime(precision: 7, storeType: "datetime2"),
                    impreso = c.Boolean(nullable: false),
                    fechaimpreso = c.DateTime(precision: 7, storeType: "datetime2"),
                    nuevo = c.Boolean(nullable: false),
                    nombre = c.String(),
                    apellido = c.String(),
                    etiqueta = c.Int(),
                    libreta = c.Int(),
                    fechaexpiracion = c.DateTime(precision: 7, storeType: "datetime2"),
                    huella = c.String(),
                    dedo = c.Int(),
                    FirmaBin = c.String(),
                    FormatoFirma = c.String(),
                    FotoBin = c.String(),
                    FormatoFoto = c.String(),
                    FechaEdicionCaptura = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.idpersona);

            CreateTable(
                "DBA.DimRegistroEmbarques",
                c => new
                {
                    idEmbarque = c.Int(nullable: false, identity: true),
                    matriculaOMI = c.String(),
                    nombreNave = c.String(),
                    cargo = c.String(),
                    grado = c.String(),
                    trb = c.Decimal(nullable: false, precision: 18, scale: 2),
                    potencia = c.String(),
                    fechaInicio = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    fechaFinal = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    nombreArchivo = c.String(),
                    rutaArchivo = c.String(),
                    idpersona = c.Decimal(nullable: false, precision: 18, scale: 2),
                    FechaRegistro = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    FechaModificacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ArchivoBin = c.String(),
                    FormatoArchivo = c.String(),
                })
                .PrimaryKey(t => t.idEmbarque);

            CreateTable(
                "DBA.t_nav_band",
                c => new
                {
                    cod_pais = c.String(nullable: false, maxLength: 128),
                    des_pais = c.String(),
                    sigla = c.String(),
                    sigla_2 = c.String(),
                    cod_continente = c.String(),
                    nacionalidad = c.String(),
                    cod_andino = c.String(),
                    emailsNotificacion = c.String(),
                    esComunidadAndina = c.Boolean(),
                    tieneConvenioNotificacion = c.Boolean(),
                })
                .PrimaryKey(t => t.cod_pais);

            CreateTable(
                "DBA.SGDEA_PREVISTAS",
                c => new
                {
                    id_sgdea_prevista = c.Long(nullable: false, identity: true),
                    radicado = c.Decimal(nullable: false, precision: 20, scale: 0, storeType: "numeric"),
                    expediente = c.String(maxLength: 20, unicode: false),
                    ruta_prevista = c.String(maxLength: 200, unicode: false),
                    estado = c.String(maxLength: 50, unicode: false),
                    fecha_estado = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    sigla_capitania_entrega = c.String(maxLength: 50, unicode: false),
                    numero_identificacion_usuario = c.String(maxLength: 15, unicode: false),
                    tipo_documento_usuario = c.String(maxLength: 20, unicode: false),
                    numero_identificacion_firmante = c.String(maxLength: 15, unicode: false),
                    tipo_tramite = c.String(maxLength: 200, unicode: false),
                })
                .PrimaryKey(t => t.id_sgdea_prevista);

        }

        public override void Down()
        {
            DropForeignKey("DBA.GENTEMAR_SECCION_CLASE", "id_seccion", "DBA.GENTEMAR_SECCION_LICENCIAS");
            DropForeignKey("DBA.GENTEMAR_SECCION_CLASE", "id_clase", "DBA.GENTEMAR_CLASE_LICENCIAS");
            DropForeignKey("DBA.GENTEMAR_REGLA_FUNCION", "id_regla", "DBA.GENTEMAR_REGLA");
            DropForeignKey("DBA.GENTEMAR_REGLA_FUNCION", "id_funcion", "DBA.GENTEMAR_FUNCION");
            DropForeignKey("DBA.GENTEMAR_REGLA_CARGO", "id_regla", "DBA.GENTEMAR_REGLA");
            DropForeignKey("DBA.GENTEMAR_REGLA_CARGO", "id_nivel", "DBA.GENTEMAR_NIVEL");
            DropForeignKey("DBA.GENTEMAR_REGLA_CARGO", "id_cargo_titulo", "DBA.GENTEMAR_CARGO_TITULO");
            DropForeignKey("DBA.GENTEMAR_REGLA_CARGO", "id_capacidad", "DBA.GENTEMAR_CAPACIDAD");
            DropForeignKey("DBA.GENTEMAR_CARGO_HABILITACION", "id_habilitacion", "DBA.GENTEMAR_HABILITACION");
            DropForeignKey("DBA.GENTEMAR_ANTECEDENTES", "id_tipo_tramite", "DBA.GENTEMAR_TIPO_TRAMITE");
            DropForeignKey("DBA.GENTEMAR_ANTECEDENTES", "id_estado_antecedente", "DBA.GENTEMAR_ESTADO_ANTECEDENTE");
            DropForeignKey("DBA.GENTEMAR_ANTECEDENTES", "id_capitania", "DBA.APLICACIONES_CAPITANIAS");
            DropForeignKey("DBA.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA", "id_seccion", "DBA.GENTEMAR_SECCION_LICENCIAS");
            DropForeignKey("DBA.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA", "id_actividad", "DBA.GENTEMAR_ACTIVIDAD");
            DropForeignKey("DBA.GENTEMAR_ACTIVIDAD", "id_tipo_licencia", "DBA.GENTEMAR_TIPO_LICENCIA");
            DropForeignKey("DBA.GENTEMAR_TITULOS", "id_tipo_solicitud", "DBA.APLICACIONES_TIPO_SOLICITUD");
            DropForeignKey("DBA.GENTEMAR_TITULOS", "id_estado_tramite", "DBA.GENTEMAR_ESTADO_TITULO");
            DropForeignKey("DBA.APLICACIONES_ROL_MENU", "ID_MENU", "DBA.APLICACIONES_MENU");
            DropForeignKey("DBA.GENTEMAR_FORMACION_GRADO", "id_formacion", "DBA.GENTEMAR_FORMACION");
            DropForeignKey("DBA.GENTEMAR_FORMACION_GRADO", "id_grado", "DBA.APLICACIONES_GRADO");
            DropForeignKey("DBA.APLICACIONES_GRADO", "id_rango", "DBA.APLICACIONES_RANGO");
            DropForeignKey("DBA.APLICACIONES_ROLES", "ID_ESTADO", "DBA.APLICACIONES_ESTADO");
            DropForeignKey("DBA.APLICACIONES_CAPITANIAS", "ID_CATEGORIA", "DBA.APLICACIONES_CATEGORIA");
            DropIndex("DBA.GENTEMAR_SECCION_CLASE", new[] { "id_seccion" });
            DropIndex("DBA.GENTEMAR_SECCION_CLASE", new[] { "id_clase" });
            DropIndex("DBA.GENTEMAR_REGLA_FUNCION", new[] { "id_funcion" });
            DropIndex("DBA.GENTEMAR_REGLA_FUNCION", new[] { "id_regla" });
            DropIndex("DBA.GENTEMAR_REGLA_CARGO", new[] { "id_capacidad" });
            DropIndex("DBA.GENTEMAR_REGLA_CARGO", new[] { "id_nivel" });
            DropIndex("DBA.GENTEMAR_REGLA_CARGO", new[] { "id_cargo_titulo" });
            DropIndex("DBA.GENTEMAR_REGLA_CARGO", new[] { "id_regla" });
            DropIndex("DBA.GENTEMAR_CARGO_HABILITACION", new[] { "id_habilitacion" });
            DropIndex("DBA.GENTEMAR_ANTECEDENTES", new[] { "id_tipo_tramite" });
            DropIndex("DBA.GENTEMAR_ANTECEDENTES", new[] { "id_estado_antecedente" });
            DropIndex("DBA.GENTEMAR_ANTECEDENTES", new[] { "id_capitania" });
            DropIndex("DBA.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA", new[] { "id_seccion" });
            DropIndex("DBA.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA", new[] { "id_actividad" });
            DropIndex("DBA.GENTEMAR_ACTIVIDAD", new[] { "id_tipo_licencia" });
            DropIndex("DBA.GENTEMAR_TITULOS", new[] { "id_tipo_solicitud" });
            DropIndex("DBA.GENTEMAR_TITULOS", new[] { "id_estado_tramite" });
            DropIndex("DBA.APLICACIONES_ROL_MENU", new[] { "ID_MENU" });
            DropIndex("DBA.GENTEMAR_FORMACION_GRADO", new[] { "id_grado" });
            DropIndex("DBA.GENTEMAR_FORMACION_GRADO", new[] { "id_formacion" });
            DropIndex("DBA.APLICACIONES_GRADO", new[] { "id_rango" });
            DropIndex("DBA.APLICACIONES_ROLES", new[] { "ID_ESTADO" });
            DropIndex("DBA.APLICACIONES_CAPITANIAS", new[] { "ID_CATEGORIA" });
            DropTable("DBA.SGDEA_PREVISTAS");
            DropTable("DBA.t_nav_band");
            DropTable("DBA.DimRegistroEmbarques");
            DropTable("DBA.DimPersonas");
            DropTable("DBA.DimImpresion");
            DropTable("DBA.m_nav_bas");
            DropTable("DBA.GENTEMAR_TITULO_HABILITACION");
            DropTable("DBA.GENTEMAR_TITULO_FUNCION");
            DropTable("DBA.GENTEMAR_TERRITORIO");
            DropTable("DBA.GENTEMAR_SECCION_TITULOS");
            DropTable("DBA.GENTEMAR_SECCION_CLASE");
            DropTable("DBA.GENTEMAR_REPOSITORIO_ARCHIVOS");
            DropTable("DBA.GENTEMAR_REGLA_FUNCION");
            DropTable("DBA.GENTEMAR_OBSERVACIONES_TITULOS");
            DropTable("DBA.GENTEMAR_OBSERVACIONES_LICENCIAS");
            DropTable("DBA.GENTEMAR_OBSERVACIONES_DATOSBASICOS");
            DropTable("DBA.GENTEMAR_OBSERVACIONES_ANTECEDENTES");
            DropTable("DBA.GENTEMAR_LIMITANTE");
            DropTable("DBA.GENTEMAR_LIMITACION");
            DropTable("DBA.GENTEMAR_LICENCIAS");
            DropTable("DBA.GENTEMAR_LICENCIA_NAVES");
            DropTable("DBA.GENTEMAR_HISTORIAL_DOCUMENTO");
            DropTable("DBA.GENTEMAR_FUNCION");
            DropTable("DBA.GENTEMAR_FOTOGRAFIA");
            DropTable("DBA.GENTEMAR_ESTADO_LICENCIA");
            DropTable("DBA.GENTEMAR_ESTADO");
            DropTable("DBA.GENTEMAR_ENTIDAD");
            DropTable("DBA.GENTEMAR_DATOSBASICOS");
            DropTable("DBA.GENTEMAR_CLASE_TITULOS");
            DropTable("DBA.GENTEMAR_CLASE_LICENCIAS");
            DropTable("DBA.GENTEMAR_REGLA");
            DropTable("DBA.GENTEMAR_NIVEL");
            DropTable("DBA.GENTEMAR_REGLA_CARGO");
            DropTable("DBA.GENTEMAR_CARGO_TITULO");
            DropTable("DBA.GENTEMAR_CARGO_LIMITACION");
            DropTable("DBA.GENTEMAR_CARGO_LICENCIA_LIMITANTE");
            DropTable("DBA.GENTEMAR_CARGO_LICENCIA_CATEGORIA");
            DropTable("DBA.GENTEMAR_CARGO_LICENCIA");
            DropTable("DBA.GENTEMAR_HABILITACION");
            DropTable("DBA.GENTEMAR_CARGO_HABILITACION");
            DropTable("DBA.GENTEMAR_CAPACIDAD");
            DropTable("DBA.GENTEMAR_ANTECEDENTES_DATOSBASICOS");
            DropTable("DBA.GENTEMAR_TIPO_TRAMITE");
            DropTable("DBA.GENTEMAR_ESTADO_ANTECEDENTE");
            DropTable("DBA.GENTEMAR_ANTECEDENTES");
            DropTable("DBA.GENTEMAR_SECCION_LICENCIAS");
            DropTable("DBA.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA");
            DropTable("DBA.GENTEMAR_TIPO_LICENCIA");
            DropTable("DBA.GENTEMAR_ACTIVIDAD");
            DropTable("DBA.GENTEMAR_ACLARACION_ANTECEDENTES");
            DropTable("DBA.GENTEMAR_TITULOS");
            DropTable("DBA.APLICACIONES_TIPO_SOLICITUD");
            DropTable("DBA.APLICACIONES_TIPO_DOCUMENTO");
            DropTable("DBA.APLICACIONES_MUNICIPIO");
            DropTable("DBA.APLICACIONES_ROL_MENU");
            DropTable("DBA.APLICACIONES_MENU");
            DropTable("DBA.APLICACIONES_LOGINS");
            DropTable("DBA.APLICACIONES_LOGIN_SUCURSAL");
            DropTable("DBA.APLICACIONES_LOGIN_ROL");
            DropTable("DBA.GENTEMAR_FORMACION");
            DropTable("DBA.GENTEMAR_FORMACION_GRADO");
            DropTable("DBA.APLICACIONES_RANGO");
            DropTable("DBA.APLICACIONES_GRADO");
            DropTable("DBA.APLICACIONES_GENERO");
            DropTable("DBA.GENTEMAR_ESTADO_TITULO");
            DropTable("DBA.APLICACIONES_ROLES");
            DropTable("DBA.APLICACIONES_ESTADO");
            DropTable("DBA.APLICACIONES_DEPARTAMENTO");
            DropTable("DBA.APLICACIONES_CATEGORIA");
            DropTable("DBA.APLICACIONES_CAPITANIAS");
            DropTable("DBA.APLICACIONES");
        }
    }
}
