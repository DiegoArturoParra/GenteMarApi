using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class LicenciaRepository : GenericRepository<GENTEMAR_LICENCIAS>
    {
        public LicenciaRepository()
        {

        }

        public LicenciaRepository(GenteDeMarCoreContext context) : base(context) // Llama al constructor de la clase base
        {
        }
        /// <summary>
        /// Lista de licencias por id 
        /// <param name="id">Id del usuario</param>
        /// </summary>
        /// <returns> Lista de licencias por id</returns>
        public async Task<IList<LicenciaDTO>> GetlicenciasPorUsuarioId(long id)
        {

            var resultado = await (from licencia in _context.GENTEMAR_LICENCIAS
                                   join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on
                                   licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                                   join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on
                                   licencia.id_estado_licencia equals estadoLicencia.id_estado_licencias
                                   join capitania in _context.APLICACIONES_CAPITANIAS on
                                   licencia.id_capitania equals capitania.ID_CAPITANIA
                                   join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on
                                   licencia.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                                   where licencia.id_gentemar == id
                                   select new LicenciaDTO
                                   {
                                       Activo = licencia.activo,
                                       Radicado = licencia.radicado,
                                       IdLicencia = licencia.id_licencia,
                                       IdGentemar = licencia.id_gentemar,
                                       IdEstadoLicencia = licencia.id_estado_licencia,
                                       IdCargoLicencia = licencia.id_cargo_licencia,
                                       IdCapitaniaFirmante = licencia.id_capitania_firmante,
                                       IdCapitania = licencia.id_capitania,
                                       FechaVencimiento = licencia.fecha_vencimiento,
                                       FechaExpedicion = licencia.fecha_expedicion,
                                       Capitania = new CapitaniaDTO
                                       {
                                           Descripcion = capitania.DESCRIPCION,
                                           Id = capitania.ID_CAPITANIA,
                                           Sigla = capitania.SIGLA_CAPITANIA
                                       },
                                       CapitaniaFirmante = new CapitaniaDTO
                                       {
                                           Descripcion = capitaniaFirmante.DESCRIPCION,
                                           Id = capitaniaFirmante.ID_CAPITANIA,
                                           Sigla = capitaniaFirmante.SIGLA_CAPITANIA
                                       },
                                       CargoLicencia = new CargoInfoLicenciaDTO
                                       {
                                           IdCargoLicencia = cargoLicencia.id_cargo_licencia,
                                           CargoLicencia = cargoLicencia.cargo_licencia,
                                           CodigoLicencia = cargoLicencia.codigo_licencia
                                       },
                                       Estado = new EstadoLicenciaDTO
                                       {
                                           IdEstadoLicencias = estadoLicencia.id_estado_licencias,
                                           DescripcionEstado = estadoLicencia.descripcion_estado
                                       },
                                       ContienePrevista = _context.TABLA_SGDEA_PREVISTAS
                                       .Any(x => x.radicado.Equals(licencia.radicado) && x.estado == Constantes.PREVISTAGENERADA),
                                       ContieneFirmaCapitan = _context.TABLA_SGDEA_PREVISTAS
                                       .Any(x => x.radicado.Equals(licencia.radicado) && x.estado == Constantes.PREVISTAFIRMADA)
                                   }).AsNoTracking().OrderByDescending(x => x.FechaExpedicion).ToListAsync();

            return resultado;


        }

        public async Task<LicenciaDTO> GetlicenciaId(int id)
        {
            var resultado = (from licencia in _context.GENTEMAR_LICENCIAS
                             join usuario in _context.GENTEMAR_DATOSBASICOS on
                             licencia.id_gentemar equals usuario.id_gentemar
                             where licencia.id_licencia == id
                             select new LicenciaDTO
                             {
                                 Activo = licencia.activo,
                                 Radicado = licencia.radicado,
                                 IdLicencia = licencia.id_licencia,
                                 IdGentemar = licencia.id_gentemar,
                                 IdEstadoLicencia = licencia.id_estado_licencia,
                                 IdCargoLicencia = licencia.id_cargo_licencia,
                                 IdCapitaniaFirmante = licencia.id_capitania_firmante,
                                 IdCapitania = licencia.id_capitania,
                                 FechaVencimiento = licencia.fecha_vencimiento,
                                 FechaExpedicion = licencia.fecha_expedicion,
                                 IdLicenciaTitulo = licencia.id_licencia_titulo,
                                 Usuario = new UsuarioGenteMarDTO
                                 {
                                     Id = usuario.id_gentemar,
                                     Nombres = usuario.nombres,
                                     Apellidos = usuario.apellidos,
                                     DocumentoIdentificacion = usuario.documento_identificacion,
                                     FechaNacimiento = usuario.fecha_vencimiento
                                 },
                                 ListaNaves = (from licenciaNave in _context.GENTEMAR_LICENCIA_NAVES
                                               join naves in _context.NAVES_BASE on licenciaNave.identi equals naves.identi
                                               where licenciaNave.id_licencia == licencia.id_licencia
                                               select new
                                               {
                                                   naves
                                               }).Select(x => x.naves.identi).ToList()
                             }).FirstOrDefaultAsync();

            return await resultado;
        }

        /// <summary>
        /// Lista la liecncia por id 
        /// <param name="id">Id del la licencia</param>
        /// </summary>
        public async Task<LicenciaDTO> GetlicenciaIdView(int id)
        {

            var resultado = (from licencia in _context.GENTEMAR_LICENCIAS
                             join cargoLicencia in _context.GENTEMAR_CARGO_LICENCIA on
                             licencia.id_cargo_licencia equals cargoLicencia.id_cargo_licencia
                             join estadoLicencia in _context.GENTEMAR_ESTADO_LICENCIAS on
                             licencia.id_estado_licencia equals estadoLicencia.id_estado_licencias
                             join capitania in _context.APLICACIONES_CAPITANIAS on
                             licencia.id_capitania equals capitania.ID_CAPITANIA
                             join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on
                             licencia.id_capitania_firmante equals capitaniaFirmante.ID_CAPITANIA
                             join usuario in _context.GENTEMAR_DATOSBASICOS on
                             licencia.id_gentemar equals usuario.id_gentemar
                             where licencia.id_licencia == id
                             select new LicenciaDTO
                             {
                                 Activo = licencia.activo,
                                 Radicado = licencia.radicado,
                                 IdLicencia = licencia.id_licencia,
                                 IdGentemar = licencia.id_gentemar,
                                 IdEstadoLicencia = licencia.id_estado_licencia,
                                 IdCargoLicencia = licencia.id_cargo_licencia,
                                 IdCapitaniaFirmante = licencia.id_capitania_firmante,
                                 IdCapitania = licencia.id_capitania,
                                 FechaVencimiento = licencia.fecha_vencimiento,
                                 FechaExpedicion = licencia.fecha_expedicion,
                                 Capitania = new CapitaniaDTO
                                 {
                                     Descripcion = capitania.DESCRIPCION,
                                     Id = capitania.ID_CAPITANIA,
                                     Sigla = capitania.SIGLA_CAPITANIA
                                 },
                                 CapitaniaFirmante = new CapitaniaDTO
                                 {
                                     Descripcion = capitaniaFirmante.DESCRIPCION,
                                     Id = capitaniaFirmante.ID_CAPITANIA,
                                     Sigla = capitaniaFirmante.SIGLA_CAPITANIA
                                 },
                                 CargoLicencia = new CargoInfoLicenciaDTO
                                 {
                                     IdCargoLicencia = cargoLicencia.id_cargo_licencia,
                                     CargoLicencia = cargoLicencia.cargo_licencia,
                                     CodigoLicencia = cargoLicencia.codigo_licencia
                                 },
                                 Estado = new EstadoLicenciaDTO
                                 {
                                     IdEstadoLicencias = estadoLicencia.id_estado_licencias,
                                     DescripcionEstado = estadoLicencia.descripcion_estado
                                 },
                                 Usuario = new UsuarioGenteMarDTO
                                 {
                                     Id = usuario.id_gentemar,
                                     Nombres = usuario.nombres,
                                     Apellidos = usuario.apellidos,
                                     DocumentoIdentificacion = usuario.documento_identificacion,
                                     FechaNacimiento = usuario.fecha_vencimiento

                                 },

                             }).FirstOrDefaultAsync();

            return await resultado;


        }

        public async Task CrearLicencia(GENTEMAR_LICENCIAS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {

            try
            {
                BeginTransaction();
                datos.id_estado_licencia = (int)EstadosTituloLicenciaEnum.PROCESO;
                _context.GENTEMAR_LICENCIAS.Add(datos);
                await SaveAllAsync();
                var datosBasicos = new DatosBasicosRepository(_context);
                await datosBasicos.CambiarEstadoPersonaEstaInactiva(datos.id_gentemar, datos.id_estado_licencia);
                CrearObservacionDefault(datos.id_licencia, datos.id_estado_licencia);
                await CrateInCascadeNaves(datos.ListaNaves, datos.id_licencia);
                if (datos.Observacion != null)
                {
                    datos.Observacion.id_licencia = datos.id_licencia;
                    _context.GENTEMAR_OBSERVACIONES_LICENCIAS.Add(datos.Observacion);
                    await SaveAllAsync();
                    if (repositorio != null)
                    {
                        repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                        await SaveAllAsync();
                    }
                }
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, datos);
            }

        }
        private void CrearObservacionDefault(long id_licencia, int estado)
        {
            var estadoEnum = (EstadosTituloLicenciaEnum)estado;
            var observacion = new GENTEMAR_OBSERVACIONES_LICENCIAS
            {
                id_licencia = id_licencia,
                observacion = $"Registro de licencia creado, estado en {EnumConfig.GetDescription(estadoEnum)}"
            };
            _context.GENTEMAR_OBSERVACIONES_LICENCIAS.Add(observacion);
        }

        private async Task CrateInCascadeNaves(IList<string> data, long? idLicencia)
        {
            foreach (string item in data)
            {
                var categoria = new GENTEMAR_LICENCIA_NAVES
                {
                    id_licencia = (long)idLicencia,
                    identi = item
                };

                _context.GENTEMAR_LICENCIA_NAVES.Add(categoria);

            }
            await SaveAllAsync();
        }

        public async Task ActualizarLicencia(GENTEMAR_LICENCIAS datos, GENTEMAR_REPOSITORIO_ARCHIVOS repositorio = null)
        {

            try
            {
                BeginTransaction();
                await Update(datos);
                var datosBasicos = new DatosBasicosRepository(_context);
                await datosBasicos.CambiarEstadoPersonaEstaInactiva(datos.id_gentemar, datos.id_estado_licencia);
                if (datos.ListaNaves != null && datos.ListaNaves.Count() > 0)
                {
                    // elimina los resgistros de las naves existente 
                    _context.GENTEMAR_LICENCIA_NAVES.RemoveRange(_context.GENTEMAR_LICENCIA_NAVES.Where(x => x.id_licencia == datos.id_licencia)
                    );
                    //crea ls nuevas naves
                    await CrateInCascadeNaves(datos.ListaNaves, datos.id_licencia);
                }
                if (datos.Observacion != null)
                {
                    datos.Observacion.id_licencia = datos.id_licencia;
                    _context.GENTEMAR_OBSERVACIONES_LICENCIAS.Add(datos.Observacion);
                    await SaveAllAsync();
                    if (repositorio != null)
                    {
                        repositorio.IdModulo = datos.Observacion.id_observacion.ToString();
                        _context.GENTEMAR_REPOSITORIO_ARCHIVOS.Add(repositorio);
                        await SaveAllAsync();
                    }
                }
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ObtenerException(ex, datos);
            }
        }

        public async Task<long> GetIdLicencia(decimal radicado)
        {
            var id = await Table.Where(x => x.radicado.Equals(radicado)).Select(x => x.id_licencia).FirstOrDefaultAsync();
            return id;
        }
    }
}
