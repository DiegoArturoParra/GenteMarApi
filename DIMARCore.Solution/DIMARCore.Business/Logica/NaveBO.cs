using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class NaveBO
    {
        private NaveRepository _repositoryNaves;
        public NaveBO()
        {
            _repositoryNaves = new NaveRepository();
        }

        public async Task<ICollection<NavesDTO>> GetAll()
        {
            return await _repositoryNaves.GetNaves();
        }
    }
}
