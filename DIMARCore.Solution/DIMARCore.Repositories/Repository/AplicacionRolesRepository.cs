using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Enums;

namespace DIMARCore.Repositories.Repository
{
    public class AplicacionRolesRepository : GenericRepository<APLICACIONES_ROLES>
    {
        public async Task<IEnumerable<RolSession>> GetRoles()
        {
            return await Table.Where(y => y.ID_APLICACION == (int)TipoAplicacionEnum.GenteDeMar && y.ID_ESTADO == 1)
                .Select(y => new RolSession()
                {
                    Id = y.ID_ROL,
                    NombreRol = y.DESCRIPCION,
                    EstadoId = y.ID_ESTADO
                }).ToListAsync();
        }
    }
}
