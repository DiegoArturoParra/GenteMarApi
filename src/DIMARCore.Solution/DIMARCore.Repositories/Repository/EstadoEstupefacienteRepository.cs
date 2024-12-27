using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;

namespace DIMARCore.Repositories.Repository
{
    public class EstadoEstupefacienteRepository : GenericRepository<GENTEMAR_ESTADO_ANTECEDENTE>
    {
        public EstadoEstupefacienteRepository()
        {
        }
        public EstadoEstupefacienteRepository(GenteDeMarCoreContext context) : base(context)
        {
        }
    }
}
