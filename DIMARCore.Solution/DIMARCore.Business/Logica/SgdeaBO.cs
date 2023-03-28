using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using System.Collections.Generic;

namespace DIMARCore.Business.Logica
{
    public class SgdeaBO
    {
        public IEnumerable<RadicadoDTO> GetRadicadosTitulosByCedula(string cedula)
        {

            return new SGDEARepository().GetRadicadosTitulosByCedula(cedula);
        }

        public IEnumerable<RadicadoDTO> GetRadicadosLicenciasByCedula(string cedula)
        {

            return new SGDEARepository().GetRadicadosLicenciaByCedula(cedula);
        }
    }
}
