using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class SGDEARepository : GenericRepository<SGDEA_PREVISTAS>
    {

        public IEnumerable<RadicadoDTO> GetRadicadosTitulosByCedula(string cedula)
        {
            var query = (from titulo in _context.GENTEMAR_TITULOS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on titulo.id_gentemar equals usuario.id_gentemar
                         where usuario.documento_identificacion.Equals(cedula)
                         select titulo.radicado
                         ).ToList();

            List<RadicadoDTO> radicados = new List<RadicadoDTO>();
            radicados = (from listado in _context.TABLA_SGDEA_PREVISTAS
                         where listado.numero_identificacion_usuario.Equals(cedula) && listado.tipo_tramite.Contains(Constantes.TRAMITE_TITULOS)
                         && !query.Contains(listado.radicado.ToString())
                         group listado by listado.radicado into grouped
                         where grouped.Count() == 1
                         select new RadicadoDTO
                         {
                             Radicado = grouped.Key.ToString(),
                             Conteo = grouped.Count()
                         }).ToList();


            return radicados;
        }


        public IEnumerable<RadicadoDTO> GetRadicadosLicenciaByCedula(string cedula)
        {
            var query = (from licencia in _context.GENTEMAR_LICENCIAS
                         join usuario in _context.GENTEMAR_DATOSBASICOS on licencia.id_gentemar equals usuario.id_gentemar
                         where usuario.documento_identificacion.Equals(cedula)
                         select licencia.radicado
                         ).ToList();

            List<RadicadoDTO> radicados = new List<RadicadoDTO>();
            radicados = (from listado in _context.TABLA_SGDEA_PREVISTAS
                         where listado.numero_identificacion_usuario.Equals(cedula) && listado.tipo_tramite.Contains(Constantes.TRAMITE_LICENCIA)
                         && !query.Contains(listado.radicado.ToString())
                         group listado by listado.radicado into grouped
                         where grouped.Count() == 1
                         select new RadicadoDTO
                         {
                             Radicado = grouped.Key.ToString(),
                             Conteo = grouped.Count()
                         }).ToList();


            return radicados;
        }

        public async Task<(decimal? Radicado, DateTime? Fecha)> FechaRadicado(string numero_sgdea)
        {
            decimal Radicado = Convert.ToDecimal(numero_sgdea);
            var data = await _context.TABLA_SGDEA_PREVISTAS.Where(x => x.radicado == Radicado && x.estado.Equals(Constantes.TIPO_TRAMITE))
                 .Select(y => new { Radicado = y.radicado, Fecha = y.fecha_estado }).FirstOrDefaultAsync();

            if (data == null)
                return (null, null);

            return (data.Radicado, data.Fecha);

        }
    }
}
