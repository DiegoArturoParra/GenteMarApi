using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class PrevistaRepository : GenericRepository<GENTEMAR_REPOSITORIO_ARCHIVOS>
    {

        /// <summary>
        /// metodo para obtener los datos del usuario y de la licencia para la plantilla
        /// por id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlantillaLicenciaDTO> GetPlantillaLicencia(long id)
        {
            var query = await (from licencia in _context.GENTEMAR_LICENCIAS
                               join datosBasicos in _context.GENTEMAR_DATOSBASICOS on licencia.id_gentemar equals datosBasicos.id_gentemar
                               join ciuExpDoc in _context.TABLA_NAV_BAND on datosBasicos.cod_pais equals ciuExpDoc.cod_pais
                               join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                               join actSecLicen in _context.GENTEMAR_ACTIVIDAD_SECCION_LICENCIA on cargoLicencia.id_actividad_seccion_licencia equals actSecLicen.id_actividad_seccion_licencia
                               join actividad in _context.GENTEMAR_ACTIVIDAD on actSecLicen.id_actividad equals actividad.id_actividad
                               join tipoLicencia in _context.GENTEMAR_TIPO_LICENCIA on actividad.id_tipo_licencia equals tipoLicencia.id_tipo_licencia
                               where licencia.id_licencia == id
                               select new PlantillaLicenciaDTO
                               {
                                   NumeroLicencia = licencia.id_licencia,
                                   NombreLicencia = cargoLicencia.cargo_licencia,
                                   Radicado = licencia.radicado,
                                   TipoLicencia = tipoLicencia,
                                   NombreCompleto = datosBasicos.nombres + " " + datosBasicos.apellidos,
                                   Documento = datosBasicos.documento_identificacion,
                                   TipoDocumento = (from tipoDoc in _context.APLICACIONES_TIPO_DOCUMENTO
                                                    where tipoDoc.ID_TIPO_DOCUMENTO == datosBasicos.id_tipo_documento
                                                    select tipoDoc.SIGLA).FirstOrDefault(),
                                   FechaNacimiento = datosBasicos.fecha_nacimiento,
                                   CiudadExpedicion = datosBasicos.id_municipio_expedicion.HasValue
                                                        ? (from munExpDoc in _context.APLICACIONES_MUNICIPIO
                                                           where munExpDoc.ID_MUNICIPIO == datosBasicos.id_municipio_expedicion
                                                           select munExpDoc.NOMBRE_MUNICIPIO + " " + ciuExpDoc.des_pais).FirstOrDefault() : ciuExpDoc.des_pais,
                                   FechaExpedicion = licencia.fecha_expedicion,
                                   FechaVencimiento = licencia.fecha_vencimiento,
                                   Genero = datosBasicos.id_genero == (int)GeneroEnum.Femenino ? Constantes.SENORA : Constantes.SENOR,
                                   IdGentemar = datosBasicos.id_gentemar,
                                   Foto = (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                           where archivo.IdModulo == datosBasicos.id_gentemar.ToString()
                                            && archivo.NombreModulo.Equals(Constantes.CARPETA_MODULO_DATOSBASICOS)
                                            && archivo.TipoDocumento.Equals(Constantes.CARPETA_IMAGENES)
                                           select new
                                           {
                                               archivo
                                           }).OrderByDescending(x => x.archivo.FechaCreacion)
                                           .Select(x => x.archivo.RutaArchivo).FirstOrDefault(),
                                   Limitantes = (from limitantes in _context.GENTEMAR_LIMITANTE
                                                 join cargoLimit in _context.GENTEMAR_CARGO_LICENCIA_LIMITANTE on limitantes.id_limitante equals cargoLimit.id_limitante
                                                 where cargoLimit.id_cargo_licencia == cargoLicencia.id_cargo_licencia
                                                 select new
                                                 {
                                                     limitantes
                                                 }).Select(x => x.limitantes.descripcion).ToList(),
                                   Limitacion = (from limitacion in _context.GENTEMAR_LIMITACION
                                                 join cargolimta in _context.GENTEMAR_CARGO_LIMITACION on limitacion.id_limitacion equals cargolimta.id_limitacion
                                                 where cargolimta.id_cargo_licencia == cargoLicencia.id_cargo_licencia
                                                 select new
                                                 {
                                                     limitacion
                                                 }).Select(x => x.limitacion.limitaciones).ToList(),
                                   CapitaniaFirmante = (from capitanias in _context.APLICACIONES_CAPITANIAS
                                                        where capitanias.ID_CAPITANIA == licencia.id_capitania_firmante
                                                        select new
                                                        {
                                                            capitanias
                                                        }).Select(x => x.capitanias.DESCRIPCION).FirstOrDefault(),
                                   IdLicenciaTituloPep  = licencia.id_licencia_titulo,                                                       
                                   ActividadLicencia = actividad,
                                   Naves = (from licNav in _context.GENTEMAR_LICENCIA_NAVES
                                            join naves in _context.NAVES_BASE on licNav.identi equals naves.identi
                                            where licNav.id_licencia == licencia.id_licencia
                                            select new NavesImpDocDTO
                                            {
                                                NombreNave = naves.nom_naves,
                                                MatriculaNave = naves.identi
                                            }).ToList()
                               }).AsNoTracking().FirstOrDefaultAsync();

            return query;
        }



        /// <summary>
        /// metodo para obtener los datos del usuario y del titulo para la plantilla
        /// por id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlantillaTituloDTO> GetPlantillaTitulo(long id)
        {
            var query = await (from titulo in _context.GENTEMAR_TITULOS
                               join datosBasicos in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals datosBasicos.id_gentemar
                               join ciuExpDoc in _context.TABLA_NAV_BAND on datosBasicos.cod_pais equals ciuExpDoc.cod_pais
                               join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                               where titulo.id_titulo == id
                               select new PlantillaTituloDTO
                               {
                                   Foto = (from archivo in _context.GENTEMAR_REPOSITORIO_ARCHIVOS
                                           where archivo.IdModulo == datosBasicos.id_gentemar.ToString()
                                            && archivo.NombreModulo.Equals(Constantes.CARPETA_MODULO_DATOSBASICOS)
                                            && archivo.TipoDocumento.Equals(Constantes.CARPETA_IMAGENES)
                                           select new
                                           {
                                               archivo
                                           }).OrderByDescending(x => x.archivo.FechaCreacion)
                                           .Select(x => x.archivo.RutaArchivo).FirstOrDefault(),
                                   NombreCompleto = datosBasicos.nombres + " " + datosBasicos.apellidos,
                                   Documento = datosBasicos.documento_identificacion,
                                   FechaNacimiento = datosBasicos.fecha_nacimiento,
                                   CiudadExpedicion = datosBasicos.id_municipio_expedicion.HasValue
                                                        ? (from munExpDoc in _context.APLICACIONES_MUNICIPIO
                                                           where munExpDoc.ID_MUNICIPIO == datosBasicos.id_municipio_expedicion
                                                           select munExpDoc.NOMBRE_MUNICIPIO + " " + ciuExpDoc.des_pais).FirstOrDefault() : string.Empty,
                                   FechaExpedicion = titulo.fecha_expedicion,
                                   FechaVencimiento = titulo.fecha_vencimiento,
                                   NumeroTitulo = titulo.id_titulo,
                                   Radicado = titulo.radicado,
                                   CapitaniaFirmante = capitaniaFirmante.DESCRIPCION,
                                   Genero = datosBasicos.id_genero == (int)GeneroEnum.Femenino ? Constantes.SENORA : Constantes.SENOR,
                                   TipoDocumento = (from tipoDoc in _context.APLICACIONES_TIPO_DOCUMENTO
                                                    where tipoDoc.ID_TIPO_DOCUMENTO == datosBasicos.id_tipo_documento
                                                    select tipoDoc.SIGLA).FirstOrDefault(),
                                   FirmaBase64 = (from dimFirma in _context.TABLA_DIM_PERSONAS
                                                  where dimFirma.cedula.Equals(datosBasicos.documento_identificacion)
                                                  select dimFirma != null ? dimFirma.FirmaBin : ""
                                                  ).FirstOrDefault(),
                                   Reglas = (from tituloReglaCargos in _context.GENTEMAR_TITULO_REGLA_CARGOS
                                             join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on tituloReglaCargos.id_cargo_regla equals reglaCargo.id_cargo_regla
                                             join regla in _context.GENTEMAR_REGLAS on reglaCargo.id_regla equals regla.id_regla
                                             where tituloReglaCargos.id_titulo == titulo.id_titulo && tituloReglaCargos.es_eliminado == false
                                             select new
                                             {
                                                 regla
                                             }).Select(x => x.regla.nombre_regla).ToList(),
                                   Funcion = (
                                   from tituloFuncion in _context.GENTEMAR_TITULO_REGLA_CARGOS_FUNCION
                                   join funciones in _context.GENTEMAR_FUNCIONES on tituloFuncion.id_funcion equals funciones.id_funcion
                                   join reglaFuncion in _context.GENTEMAR_REGLA_FUNCION on funciones.id_funcion equals reglaFuncion.id_funcion
                                   join regla in _context.GENTEMAR_REGLAS on reglaFuncion.id_regla equals regla.id_regla
                                   join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on regla.id_regla equals reglaCargo.id_regla
                                   join tituloReglaCargo in _context.GENTEMAR_TITULO_REGLA_CARGOS
                                   on reglaCargo.id_cargo_regla equals tituloReglaCargo.id_cargo_regla
                                   where tituloReglaCargo.id_titulo == titulo.id_titulo
                                   && tituloFuncion.id_titulo_cargo_regla == tituloReglaCargo.id_titulo_cargo_regla
                                   && tituloReglaCargo.es_eliminado == false
                                   group new { funciones, reglaCargo } by new { funciones.id_funcion, reglaCargo.id_nivel } into grupo
                                   select new
                                   {
                                       Descripcion = grupo.FirstOrDefault().funciones.funcion,
                                       Nivel = (from nivel in _context.GENTEMAR_NIVEL
                                                where nivel.id_nivel == grupo.FirstOrDefault().reglaCargo.id_nivel
                                                select nivel.nivel).FirstOrDefault(),
                                       Limitacion = grupo.FirstOrDefault().funciones.limitacion_funcion
                                   }).ToList(),
                                   Cargo = (from cargo in _context.GENTEMAR_CARGO_TITULO
                                            join reglaCargo in _context.GENTEMAR_REGLAS_CARGO on cargo.id_cargo_titulo equals reglaCargo.id_cargo_titulo
                                            join tituloReglaCargo in _context.GENTEMAR_TITULO_REGLA_CARGOS
                                            on reglaCargo.id_cargo_regla equals tituloReglaCargo.id_cargo_regla
                                            where tituloReglaCargo.id_titulo == titulo.id_titulo
                                            && tituloReglaCargo.es_eliminado == false
                                            select new
                                            {
                                                Cargo = cargo.cargo,
                                                Habilitaciones = (from habilitacion in _context.GENTEMAR_HABILITACION
                                                                  join tituloHabilitacion in _context.GENTEMAR_TITULO_REGLA_CARGOS_HABILITACION
                                                                  on habilitacion.id_habilitacion equals tituloHabilitacion.id_habilitacion
                                                                  where tituloHabilitacion.id_titulo_cargo_regla == tituloReglaCargo.id_titulo_cargo_regla
                                                                  select new
                                                                  {
                                                                      habilitacion
                                                                  }).Select(x => x.habilitacion.habilitacion).ToList(),
                                                Capacidad = (from capacidad in _context.GENTEMAR_CAPACIDAD
                                                             where capacidad.id_capacidad == reglaCargo.id_capacidad
                                                             select new
                                                             {
                                                                 capacidad
                                                             }).Select(x => x.capacidad.capacidad).FirstOrDefault(),

                                            }).ToList(),
                               }).AsNoTracking().FirstOrDefaultAsync();

            return query;
        }
    }
}
