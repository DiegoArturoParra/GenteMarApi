using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repo
{
    public class TituloRepository : GenericRepository<GENTEMAR_TITULOS>
    {
        private IQueryable<ListadoTituloDTO> FiltroTitulos(string Identificacion, long Id = 0)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on titulo.id_cargo_regla equals cargoRegla.id_cargo_regla
                         join reglas in _context.GENTEMAR_REGLAS on cargoRegla.id_regla equals reglas.id_regla
                         join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirmante.ID_CAPITANIA
                         join pais in _context.t_nav_band on titulo.cod_pais equals pais.cod_pais
                         join tramite in _context.APLICACIONES_ESTADO_TRAMITE on titulo.id_estado_tramite equals tramite.ID_ESTADO_TRAMITE
                         join solicitud in _context.APLICACIONES_TIPO_SOLICITUD on titulo.id_tipo_solicitud equals solicitud.ID_TIPO_SOLICITUD
                         join nivel in _context.GENTEMAR_NIVEL on cargoRegla.id_nivel equals nivel.id_nivel
                         select new
                         {
                             titulo,
                             usuario,
                             cargoRegla,
                             cargoTitulo,
                             pais,
                             tramite,
                             capitaniaFirma,
                             capitaniaFirmante,
                             solicitud,
                             reglas,
                             nivel,
                         });

            var lista = query.ToList();

            if (!string.IsNullOrWhiteSpace(Identificacion))
            {
                query = query.Where(x => x.usuario.documento_identificacion.Equals(Identificacion));
            }
            else if (Id > 0)
            {
                query = query.Where(x => x.titulo.id_gentemar == Id);
            }

            var listado = query.OrderBy(x => x.titulo.id_titulo).Select(m => new ListadoTituloDTO
            {
                FechaExpedicion = m.titulo.fecha_expedicion,
                NombreUsuario = m.usuario.nombres + " " + m.usuario.apellidos,
                DocumentoIdentificacion = m.usuario.documento_identificacion,
                CargoTitulo = m.cargoTitulo.cargo,
                CapitaniaFirma = m.capitaniaFirma.SIGLA_CAPITANIA + " " + m.capitaniaFirma.DESCRIPCION,
                CapitaniaFirmante = m.capitaniaFirmante.SIGLA_CAPITANIA + " " + m.capitaniaFirmante.DESCRIPCION,
                Solicitud = m.solicitud.DESCRIPCION,
                Nivel = m.nivel.nivel,
                Regla = m.reglas.Regla,
                EstadoTramite = m.tramite.DESCRIPCION_TRAMITE,
                FechaVencimiento = m.titulo.fecha_vencimiento,
                Id = m.titulo.id_titulo,
                Radicado = m.titulo.radicado
            });
            return listado;
        }


        public async Task<InfoTituloDTO> GetTituloById(long id)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on titulo.id_cargo_regla equals cargoRegla.id_cargo_regla
                         join regla in _context.GENTEMAR_REGLAS on cargoRegla.id_regla equals regla.id_regla
                         join capacidad in _context.GENTEMAR_CAPACIDAD on cargoRegla.id_capacidad equals capacidad.id_capacidad
                         join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                         join capitaniaFirma in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirma.ID_CAPITANIA
                         join capitaniaFirmante in _context.APLICACIONES_CAPITANIAS on titulo.id_capitania equals capitaniaFirmante.ID_CAPITANIA
                         join pais in _context.t_nav_band on titulo.cod_pais equals pais.cod_pais
                         join tramite in _context.APLICACIONES_ESTADO_TRAMITE on titulo.id_estado_tramite equals tramite.ID_ESTADO_TRAMITE
                         join solicitud in _context.APLICACIONES_TIPO_SOLICITUD on titulo.id_tipo_solicitud equals solicitud.ID_TIPO_SOLICITUD
                         join nivel in _context.GENTEMAR_NIVEL on cargoRegla.id_nivel equals nivel.id_nivel
                         where titulo.id_titulo == id
                         select new
                         {
                             titulo,
                             usuario,
                             cargoRegla,
                             cargoTitulo,
                             pais,
                             tramite,
                             capitaniaFirma,
                             capitaniaFirmante,
                             solicitud,
                             regla,
                             nivel,
                             capacidad
                         });


            var listado = await query.OrderBy(x => x.titulo.id_titulo).Select(m => new InfoTituloDTO
            {
                FechaExpedicion = m.titulo.fecha_expedicion,
                NombreUsuario = m.usuario.nombres + " " + m.usuario.apellidos,
                DocumentoIdentificacion = m.usuario.documento_identificacion,
                CargoTitulo = m.cargoTitulo.cargo,
                CapitaniaFirma = m.capitaniaFirma.SIGLA_CAPITANIA + " " + m.capitaniaFirma.DESCRIPCION,
                CapitaniaFirmante = m.capitaniaFirmante.SIGLA_CAPITANIA + " " + m.capitaniaFirmante.DESCRIPCION,
                Solicitud = m.solicitud.DESCRIPCION,
                Nivel = m.nivel.nivel,
                Tramite = m.tramite.DESCRIPCION_TRAMITE,
                FechaVencimiento = m.titulo.fecha_vencimiento,
                Radicado = m.titulo.radicado,
                Habilitaciones = (from cargoHabilitacion in _context.GENTEMAR_CARGO_HABILITACION
                                  join habilitacion in _context.GENTEMAR_HABILITACION on cargoHabilitacion.id_habilitacion
                                  equals habilitacion.id_habilitacion
                                  where cargoHabilitacion.id_cargo_regla == m.cargoRegla.id_cargo_regla
                                  select habilitacion.habilitacion).ToList(),
                Regla = m.regla.Regla,
                Capacidad = m.capacidad.capacidad,
                Funciones = (from reglaFuncion in _context.GENTEMAR_REGLA_FUNCION
                             join funciones in _context.GENTEMAR_FUNCIONES on reglaFuncion.id_funcion equals funciones.id_funcion
                             where reglaFuncion.id_regla == m.regla.id_regla
                             select funciones.funcion).ToList(),

            }).FirstOrDefaultAsync();

            return listado;
        }

        public async Task<(FechasDTO fechas, bool HayTitulosPorSeccionPuente)> GetFechasRadioOperadores(long idGenteMar)
        {
            bool hay = false;
            FechasDTO fechas = new FechasDTO();
            string seccionPuente = "SECCIÓN DE PUENTE";
            var idSeccionPuente = await _context.GENTEMAR_SECCION_TITULOS.Where(x => x.actividad_a_bordo.ToUpper().Equals(seccionPuente.ToUpper())).
                Select(x => x.id_seccion).FirstOrDefaultAsync();
            if (idSeccionPuente > 0)
            {
                fechas = await (from titulo in _context.GENTEMAR_TITULOS
                                join cargoRegla in _context.GENTEMAR_REGLAS_CARGO on titulo.id_cargo_regla equals cargoRegla.id_cargo_regla
                                join cargoTitulo in _context.GENTEMAR_CARGO_TITULO on cargoRegla.id_cargo_titulo equals cargoTitulo.id_cargo_titulo
                                join seccion in _context.GENTEMAR_SECCION_TITULOS on cargoTitulo.id_seccion equals seccion.id_seccion
                                where titulo.id_gentemar == idGenteMar && seccion.id_seccion == idSeccionPuente
                                orderby titulo.fecha_vencimiento descending
                                select new FechasDTO
                                {
                                    FechaExpedicion = titulo.fecha_expedicion,
                                    FechaVencimiento = titulo.fecha_vencimiento
                                }).FirstOrDefaultAsync();
                if (fechas != null)
                {
                    hay = true;
                }
            }
            return (fechas, hay);
        }

        public IQueryable<ListadoTituloDTO> GetTitulosQueryable()
        {
            var listado = FiltroTitulos(string.Empty);
            return listado;
        }

        public IQueryable<ListadoTituloDTO> GetTitulosFiltro(string identificacion, long id = 0)
        {
            var listado = FiltroTitulos(identificacion, id);
            if (listado.Count() == 0) return null;
            return listado;
        }



    }
}
