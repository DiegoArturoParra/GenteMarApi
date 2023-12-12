
using DIMARCore.Utilities.Config;
using GenteMarCore.Entities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GenteMarCore.Entities
{
    public partial class GenteDeMarCoreContext : DbContext
    {
        /// <summary>
        /// constructor por defecto
        /// </summary>
        public GenteDeMarCoreContext() : base("name=GenteMarContext")
        {
            Database.SetInitializer<GenteDeMarCoreContext>(null);
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        /// <summary>
        /// constructor para obtener la cadena de conexion desde el archivo de configuracion desencriptada
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public GenteDeMarCoreContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<GenteDeMarCoreContext>(null);
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }


        #region DBSET DE LAS TABLAS DE LA BD

        public virtual DbSet<SGDEA_PREVISTAS> TABLA_SGDEA_PREVISTAS { get; set; }
        public virtual DbSet<APLICACIONES> APLICACIONES { get; set; }
        public virtual DbSet<APLICACIONES_TIPO_REFRENDO> APLICACIONES_TIPO_REFRENDO { get; set; }
        public virtual DbSet<APLICACIONES_MENU> APLICACIONES_MENU { get; set; }
        public virtual DbSet<APLICACIONES_ROL_MENU> APLICACIONES_ROL_MENU { get; set; }
        public virtual DbSet<APLICACIONES_LOGIN_ROL> APLICACIONES_LOGIN_ROL { get; set; }
        public virtual DbSet<APLICACIONES_CAPITANIAS> APLICACIONES_CAPITANIAS { get; set; }
        public virtual DbSet<APLICACIONES_CATEGORIA> APLICACIONES_CATEGORIA { get; set; }
        public virtual DbSet<APLICACIONES_DEPARTAMENTO> APLICACIONES_DEPARTAMENTO { get; set; }
        public virtual DbSet<GENTEMAR_ESTADO_TITULO> GENTEMAR_ESTADO_TITULO { get; set; }
        public virtual DbSet<APLICACIONES_GENERO> APLICACIONES_GENERO { get; set; }
        public virtual DbSet<APLICACIONES_MUNICIPIO> APLICACIONES_MUNICIPIO { get; set; }
        public virtual DbSet<APLICACIONES_GRADO> APLICACIONES_GRADO { get; set; }
        public virtual DbSet<APLICACIONES_LOGINS> APLICACIONES_LOGINS { get; set; }
        public virtual DbSet<APLICACIONES_RANGO> APLICACIONES_RANGO { get; set; }
        public virtual DbSet<APLICACIONES_TIPO_DOCUMENTO> APLICACIONES_TIPO_DOCUMENTO { get; set; }
        public virtual DbSet<APLICACIONES_TIPO_SOLICITUD> APLICACIONES_TIPO_SOLICITUD { get; set; }
        public virtual DbSet<GENTEMAR_ACTIVIDAD> GENTEMAR_ACTIVIDAD { get; set; }
        public virtual DbSet<GENTEMAR_ANTECEDENTES> GENTEMAR_ANTECEDENTES { get; set; }
        public virtual DbSet<GENTEMAR_ANTECEDENTES_DATOSBASICOS> GENTEMAR_ANTECEDENTES_DATOSBASICOS { get; set; }
        public virtual DbSet<GENTEMAR_CAPACIDAD> GENTEMAR_CAPACIDAD { get; set; }
        public virtual DbSet<GENTEMAR_REGLA_CARGO_HABILITACION> GENTEMAR_REGLA_CARGO_HABILITACION { get; set; }
        public virtual DbSet<GENTEMAR_CARGO_LICENCIA> GENTEMAR_CARGO_LICENCIA { get; set; }
        public virtual DbSet<GENTEMAR_CARGO_LIMITACION> GENTEMAR_CARGO_LIMITACION { get; set; }
        public virtual DbSet<GENTEMAR_REGLAS_CARGO> GENTEMAR_REGLAS_CARGO { get; set; }
        public virtual DbSet<GENTEMAR_CARGO_TITULO> GENTEMAR_CARGO_TITULO { get; set; }
        public virtual DbSet<GENTEMAR_CLASE_TITULOS> GENTEMAR_CLASE_TITULOS { get; set; }
        public virtual DbSet<GENTEMAR_CLASE_LICENCIAS> GENTEMAR_CLASE_LICENCIAS { get; set; }
        public virtual DbSet<GENTEMAR_DATOSBASICOS> GENTEMAR_DATOSBASICOS { get; set; }
        public virtual DbSet<GENTEMAR_ENTIDAD_ANTECEDENTE> GENTEMAR_ENTIDAD_ANTECEDENTE { get; set; }
        public virtual DbSet<GENTEMAR_ESTADO> GENTEMAR_ESTADO { get; set; }
        public virtual DbSet<GENTEMAR_ESTADO_ANTECEDENTE> GENTEMAR_ESTADO_ANTECEDENTES { get; set; }
        public virtual DbSet<GENTEMAR_ESTADO_LICENCIA> GENTEMAR_ESTADO_LICENCIAS { get; set; }
        public virtual DbSet<GENTEMAR_FORMACION> GENTEMAR_FORMACION { get; set; }
        public virtual DbSet<GENTEMAR_FORMACION_GRADO> GENTEMAR_FORMACION_GRADO { get; set; }
        public virtual DbSet<GENTEMAR_FOTOGRAFIA> GENTEMAR_FOTOGRAFIA { get; set; }
        public virtual DbSet<GENTEMAR_FUNCIONES> GENTEMAR_FUNCIONES { get; set; }
        public virtual DbSet<GENTEMAR_HABILITACION> GENTEMAR_HABILITACION { get; set; }
        public virtual DbSet<GENTEMAR_LICENCIAS> GENTEMAR_LICENCIAS { get; set; }
        public virtual DbSet<GENTEMAR_LIMITACION> GENTEMAR_LIMITACION { get; set; }
        public virtual DbSet<GENTEMAR_NIVEL> GENTEMAR_NIVEL { get; set; }
        public virtual DbSet<GENTEMAR_OBSERVACIONES_ANTECEDENTES> GENTEMAR_OBSERVACIONES_ANTECEDENTES { get; set; }
        public virtual DbSet<GENTEMAR_OBSERVACIONES_TITULOS> GENTEMAR_OBSERVACIONES_TITULOS { get; set; }
        public virtual DbSet<GENTEMAR_OBSERVACIONES_DATOSBASICOS> GENTEMAR_OBSERVACIONES_DATOSBASICOS { get; set; }
        public virtual DbSet<GENTEMAR_REGLA_FUNCION> GENTEMAR_REGLA_FUNCION { get; set; }
        public virtual DbSet<GENTEMAR_REGLAS> GENTEMAR_REGLAS { get; set; }
        public virtual DbSet<GENTEMAR_SECCION_TITULOS> GENTEMAR_SECCION_TITULOS { get; set; }
        public virtual DbSet<GENTEMAR_SECCION_LICENCIAS> GENTEMAR_SECCION_LICENCIAS { get; set; }
        public virtual DbSet<GENTEMAR_TIPO_LICENCIA> GENTEMAR_TIPO_LICENCIA { get; set; }
        public virtual DbSet<GENTEMAR_TRAMITE_ANTECEDENTE> GENTEMAR_TRAMITE_ANTECEDENTE { get; set; }
        public virtual DbSet<GENTEMAR_TITULOS> GENTEMAR_TITULOS { get; set; }
        public virtual DbSet<GENTEMAR_REPOSITORIO_ARCHIVOS> GENTEMAR_REPOSITORIO_ARCHIVOS { get; set; }
        public virtual DbSet<PAISES> TABLA_NAV_BAND { get; set; }
        public virtual DbSet<DIM_IMPRESION> TABLA_DIM_IMPRESION { get; set; }
        public virtual DbSet<DIM_REGISTRO_EMBARQUES> TABLA_DIM_REGISTRO_EMBARQUE { get; set; }
        public virtual DbSet<DIM_PERSONAS> TABLA_DIM_PERSONAS { get; set; }
        public virtual DbSet<APLICACIONES_LOGIN_SUCURSAL> APLICACIONES_LOGIN_SUCURSAL { get; set; }
        public virtual DbSet<APLICACIONES_ROLES> APLICACIONES_ROLES { get; set; }
        public virtual DbSet<GENTEMAR_ACTIVIDAD_SECCION_LICENCIA> GENTEMAR_ACTIVIDAD_SECCION_LICENCIA { get; set; }
        public virtual DbSet<GENTEMAR_SECCION_CLASE> GENTEMAR_SECCION_CLASE { get; set; }
        public virtual DbSet<GENTEMAR_CARGO_LICENCIA_CATEGORIA> GENTEMAR_CARGO_LICENCIA_CATEGORIA { get; set; }
        public virtual DbSet<GENTEMAR_OBSERVACIONES_LICENCIAS> GENTEMAR_OBSERVACIONES_LICENCIAS { get; set; }
        public virtual DbSet<GENTEMAR_LIMITANTE> GENTEMAR_LIMITANTE { get; set; }
        public virtual DbSet<GENTEMAR_CARGO_LICENCIA_LIMITANTE> GENTEMAR_CARGO_LICENCIA_LIMITANTE { get; set; }
        public virtual DbSet<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES> GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES { get; set; }
        public virtual DbSet<GENTEMAR_HISTORIAL_DOCUMENTO> GENTEMAR_HISTORIAL_DOCUMENTO { get; set; }
        public virtual DbSet<GENTEMAR_LICENCIA_NAVES> GENTEMAR_LICENCIA_NAVES { get; set; }
        public virtual DbSet<NAVES_BASE> NAVES_BASE { get; set; }
        public virtual DbSet<GENTEMAR_TITULO_REGLA_CARGOS> GENTEMAR_TITULO_REGLA_CARGOS { get; set; }
        public virtual DbSet<GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION> GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION { get; set; }
        public virtual DbSet<GENTEMAR_TITULO_REGLA_CARGOS_FUNCION> GENTEMAR_TITULO_REGLA_CARGOS_FUNCION { get; set; }
        public virtual DbSet<GENTEMAR_CONSOLIDADO> GENTEMAR_CONSOLIDADO { get; set; }
        public virtual DbSet<GENTEMAR_EXPEDIENTE> GENTEMAR_EXPEDIENTE { get; set; }
        public virtual DbSet<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES> GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES { get; set; }
        public virtual DbSet<DETALLE_NAVE> TABLA_DETALLE_NAVE { get; set; }
        public virtual DbSet<GENTEMAR_LOGS> GENTEMAR_LOGS { get; set; }
        #endregion


        #region Se crea la auditoria por columnas antes de guardar a la base de datos

        public override int SaveChanges()
        {
            //ProcesarAuditoria();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            //ProcesarAuditoria();
            return base.SaveChangesAsync();
        }

        #endregion


        #region Metodo para insertar los datos de auditoria

        private void ProcesarAuditoria()
        {
            string usuario = ClaimsHelper.GetNombreCompletoUsuario();
            foreach (var item in ChangeTracker.Entries()
                         .Where(e => e.State == EntityState.Added && e.Entity is GENTEMAR_CAMPOS_AUDITORIA))
            {
                var entidad = item.Entity as GENTEMAR_CAMPOS_AUDITORIA;
                entidad.usuario_creador_registro = usuario;
                entidad.fecha_hora_creacion = DateTime.Now;
            }

            foreach (var item in ChangeTracker.Entries()
                         .Where(e => e.State == EntityState.Modified && e.Entity is GENTEMAR_CAMPOS_AUDITORIA))
            {
                var entidad = item.Entity as GENTEMAR_CAMPOS_AUDITORIA;
                entidad.usuario_actualiza_registro = usuario;
                entidad.fecha_hora_actualizacion = DateTime.Now;
            }
        }

        #endregion


        #region OnModel Creating Contexto de la base de datos como anotaciones o relaciones

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<APLICACIONES>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES>()
                .Property(e => e.VERSION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES>()
                .Property(e => e.LLAVE_APLICACION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_TIPO_REFRENDO>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_MENU>()
                .Property(e => e.VISTA)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_MENU>()
                .Property(e => e.CONTROLADOR)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_MENU>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_MENU>()
                .HasMany(e => e.APLICACIONES_ROL_MENU)
                .WithRequired(e => e.APLICACIONES_MENU)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<APLICACIONES_CAPITANIAS>()
                .Property(e => e.SIGLA_CAPITANIA)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_CAPITANIAS>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_CAPITANIAS>()
                .Property(e => e.consecutivo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_ROLES>()
                .Property(e => e.ROL)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_ROLES>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_CATEGORIA>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_CATEGORIA>()
                .Property(e => e.SIGLA_CATEGORIA)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_CATEGORIA>()
                .HasMany(e => e.APLICACIONES_CAPITANIAS)
                .WithRequired(e => e.APLICACIONES_CATEGORIA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<APLICACIONES_DEPARTAMENTO>()
                .Property(e => e.NOMBRE_DEPARTAMENTO)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ESTADO_TITULO>()
                .Property(e => e.descripcion_tramite)
                .IsUnicode(false);


            modelBuilder.Entity<APLICACIONES_GENERO>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_GRADO>()
                .Property(e => e.grado)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_GRADO>()
                .Property(e => e.sigla)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_LOGINS>()
                .Property(e => e.NOMBRES)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_LOGINS>()
                .Property(e => e.APELLIDOS)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_LOGINS>()
                .Property(e => e.LOGIN_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_LOGINS>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_LOGINS>()
                .Property(e => e.CORREO)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_RANGO>()
                .Property(e => e.rango)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_TIPO_DOCUMENTO>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_TIPO_DOCUMENTO>()
                .Property(e => e.SIGLA)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_TIPO_SOLICITUD>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<APLICACIONES_TIPO_SOLICITUD>()
                .HasMany(e => e.GENTEMAR_TITULOS)
                .WithRequired(e => e.APLICACIONES_TIPO_SOLICITUD)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<GENTEMAR_ACTIVIDAD>()
                .Property(e => e.actividad)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ANTECEDENTES>()
                .Property(e => e.numero_sgdea)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ANTECEDENTES>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ANTECEDENTES>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CAPACIDAD>()
                .Property(e => e.capacidad)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CARGO_LICENCIA>()
                .Property(e => e.cargo_licencia)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CARGO_LICENCIA>()
                .Property(e => e.codigo_licencia)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CARGO_LICENCIA>()
                .Property(e => e.vigencia)
                .HasPrecision(30, 6);

            modelBuilder.Entity<GENTEMAR_CARGO_TITULO>()
                .Property(e => e.cargo)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CLASE_TITULOS>()
                .Property(e => e.descripcion_clase)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CLASE_TITULOS>()
                .Property(e => e.sigla)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CLASE_LICENCIAS>()
                .Property(e => e.descripcion_clase)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_CLASE_LICENCIAS>()
                .Property(e => e.sigla)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.documento_identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.cod_pais)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.nombres)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.apellidos)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.telefono)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.correo_electronico)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.numero_movil)
                .IsUnicode(false);


            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_DATOSBASICOS>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ENTIDAD_ANTECEDENTE>()
                .Property(e => e.entidad)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ESTADO>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ESTADO>()
                .Property(e => e.sigla)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ESTADO_ANTECEDENTE>()
                .Property(e => e.descripcion_estado_antecedente)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_ESTADO_LICENCIA>()
                .Property(e => e.descripcion_estado)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_FORMACION>()
                .Property(e => e.formacion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_FOTOGRAFIA>()
                .Property(e => e.detalle)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_FOTOGRAFIA>()
                .Property(e => e.fotografia)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_FOTOGRAFIA>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_FOTOGRAFIA>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_FUNCIONES>()
                .Property(e => e.funcion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_FUNCIONES>()
                .Property(e => e.limitacion_funcion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_HABILITACION>()
                .Property(e => e.habilitacion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LICENCIAS>()
                .Property(e => e.radicado)
                .HasPrecision(20, 0);

            modelBuilder.Entity<GENTEMAR_LICENCIAS>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LICENCIAS>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LIMITACION>()
                .Property(e => e.limitaciones)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_NIVEL>()
                .Property(e => e.nivel)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_ANTECEDENTES>()
                .Property(e => e.observacion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_ANTECEDENTES>()
                .Property(e => e.ruta_archivo)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_ANTECEDENTES>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_ANTECEDENTES>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);


            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_DATOSBASICOS>()
                .Property(e => e.observacion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_DATOSBASICOS>()
                .Property(e => e.ruta_archivo)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_DATOSBASICOS>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_DATOSBASICOS>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_TITULOS>()
                .Property(e => e.observacion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_TITULOS>()
                .Property(e => e.ruta_archivo)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_TITULOS>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_OBSERVACIONES_TITULOS>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_REGLAS>()
                .Property(e => e.nombre_regla)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_SECCION_TITULOS>()
                .Property(e => e.actividad_a_bordo)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_SECCION_LICENCIAS>()
                .Property(e => e.actividad_a_bordo)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_TIPO_LICENCIA>()
                .Property(e => e.tipo_licencia)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_TRAMITE_ANTECEDENTE>()
                .Property(e => e.descripcion_tipo_tramite)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_TITULOS>()
                .Property(e => e.cod_pais)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_TITULOS>()
                .Property(e => e.radicado)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_TITULOS>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_TITULOS>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.radicado)
                .HasPrecision(20, 0);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.expediente)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.ruta_prevista)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.sigla_capitania_entrega)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.numero_identificacion_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.tipo_documento_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.numero_identificacion_firmante)
                .IsUnicode(false);

            modelBuilder.Entity<SGDEA_PREVISTAS>()
                .Property(e => e.tipo_tramite)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_REGLA_CARGO_HABILITACION>()
                .HasKey(x => new { x.id_habilitacion, x.id_cargo_regla });

            modelBuilder.Entity<GENTEMAR_REGLA_FUNCION>()
                .HasKey(x => new { x.id_regla, x.id_funcion });

            modelBuilder.Entity<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>()
                .Property(e => e.descripcion_observacion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_EXPEDIENTE_OBSERVACION_ANTECEDENTES>()
                .Property(e => e.usuario_actualiza_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION>()
             .HasKey(x => new { x.id_titulo_cargo_regla, x.id_habilitacion });

            modelBuilder.Entity<GENTEMAR_TITULO_REGLA_CARGOS_FUNCION>()
             .HasKey(x => new { x.id_titulo_cargo_regla, x.id_funcion });

            modelBuilder.Entity<GENTEMAR_CONSOLIDADO>()
               .Property(e => e.usuario_creador_registro)
               .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_EXPEDIENTE>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES>()
                .Property(e => e.detalle_aclaracion)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES>()
                .Property(e => e.ruta_archivo)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_HISTORIAL_ACLARACION_ANTECEDENTES>()
                .Property(e => e.usuario_creador_registro)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LOGS>()
                .Property(e => e.MESSAGE_WARNING)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LOGS>()
                .Property(e => e.MESSAGE_EXCEPTION)
                .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LOGS>()
               .Property(e => e.USER_SESSION)
               .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LOGS>()
               .Property(e => e.SEVERITY_LEVEL)
               .IsUnicode(false);

            modelBuilder.Entity<GENTEMAR_LOGS>()
               .Property(e => e.STACK_TRACE)
               .IsUnicode(false);
        }

        #endregion
    }
}