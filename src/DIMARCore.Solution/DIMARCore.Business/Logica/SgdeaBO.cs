using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class SgdeaBO
    {
        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosTitulosByCedula(string cedula)
        {
            return await new SGDEARepository().GetRadicadosTitulosByCedula(cedula);
        }

        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosLicenciasByCedula(string cedula)
        {
            return await new SGDEARepository().GetRadicadosLicenciaByCedula(cedula);
        }

        public async Task<IEnumerable<RadicadoInfoDTO>> GetRadicadosInfoPersonaParaEstupefacientes(RadicadoSGDEAFilter filter, CancellationToken cancellationToken)
        {
            using (var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var data = await new SGDEARepository().GetRadicadosInfoPersonaParaEstupefacientes(filter, tokenSource);
                return data;
            }
        }
        /// <summary>
        /// Obtiene las previstas por el numero de rtadicado el estado y tipo de tramite 
        /// </summary>
        /// <param name="radicado"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task<SGDEA_PREVISTAS> GetPrevistaEstado(decimal radicado, string estado, string tramite)
        {
            return await new SGDEARepository().GetWithConditionAsync(x => x.radicado == radicado
                 && (string.IsNullOrEmpty(tramite) || x.tipo_tramite.Contains(tramite))
                 && x.estado == estado);
        }

        public async Task ExisteRadicado(decimal radicado)
        {
            var existeRadicadoSGDEA = await new SGDEARepository().AnyWithConditionAsync(x => x.radicado.Equals(radicado)
                                                                                    && x.estado.Equals(Constantes.PREVISTATRAMITE));
            if (!existeRadicadoSGDEA)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el radicado: {radicado} en el SGDEA.");
        }
    }
}
