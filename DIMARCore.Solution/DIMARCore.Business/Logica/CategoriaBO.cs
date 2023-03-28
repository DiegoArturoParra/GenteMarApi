using DIMARCore.Repositories.Repository;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;

namespace DIMARCore.Business.Logica
{
    public class CategoriaBO
    {
        public IEnumerable<APLICACIONES_CATEGORIA> GetAll(bool? activo = true)
        {
            return new CategoriaRepository().GetAll();
        }
    }
}
