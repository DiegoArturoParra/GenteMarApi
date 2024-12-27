using GenteMarCore.Entities;
using GenteMarCore.Entities.Models;

namespace DIMARCore.Repositories.Repository
{
    public class TramiteEstupefacienteRepository : GenericRepository<GENTEMAR_TRAMITE_ANTECEDENTE>
    {
        public TramiteEstupefacienteRepository()
        {

        }
        public TramiteEstupefacienteRepository(GenteDeMarCoreContext context) : base(context)
        {

        }
    }
}
