using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<RadicadoInfoDTO>> GetRadicadosInfoPersonaParaEstupefacientes(bool isTitulo)
        {
            return await new SGDEARepository().GetRadicadosInfoPersonaParaEstupefacientes(isTitulo);
        }
        /// <summary>
        /// Obtiene las previstas por el numero de rtadicado el estado y tipo de tramite 
        /// </summary>
        /// <param name="radicado"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task<SGDEA_PREVISTAS> GetPrevistaEstado(decimal radicado, string estado, string tramite)
        {
            return await new SGDEARepository().GetWithCondition(x => x.radicado == radicado
            && x.tipo_tramite.Contains(tramite) && x.estado == estado);
        }
    }
}
