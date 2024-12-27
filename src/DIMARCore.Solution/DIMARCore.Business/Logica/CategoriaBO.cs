using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;

namespace DIMARCore.Business.Logica
{
    public class CategoriaBO
    {
        public IEnumerable<APLICACIONES_CATEGORIA> GetAll()
        {
            return new CategoriaRepository().GetAll();
        }
    }
}
