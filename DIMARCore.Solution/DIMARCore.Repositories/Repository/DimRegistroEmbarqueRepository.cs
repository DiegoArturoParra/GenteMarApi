using DIMARCore.UIEntities.DTOs;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class DimRegistroEmbarqueRepository : GenericRepository<DIM_REGISTRO_EMBARQUES>
    {
        public async Task<IEnumerable<DimRegistroEmbarqueDTO>> GetDimRegistroEmbarque(string identificacionConPuntos)
        {
            var query = await (from registrosEmbarque in _context.TABLA_DIM_REGISTRO_EMBARQUE
                               join dimPersonas in _context.TABLA_DIM_PERSONAS on
                                  registrosEmbarque.idpersona equals dimPersonas.idpersona
                               where dimPersonas.cedula.Equals(identificacionConPuntos)
                               select new DimRegistroEmbarqueDTO
                               {
                                   MatriculaOMI = registrosEmbarque.matriculaOMI,
                                   NombreNave = registrosEmbarque.nombreNave,
                                   Cargo = registrosEmbarque.cargo,
                                   Grado = registrosEmbarque.grado,
                                   FechaInicio = registrosEmbarque.fechaInicio,
                                   FechaFinal = registrosEmbarque.fechaFinal,
                                   FechaRegistro = registrosEmbarque.FechaRegistro,
                                   DifDias = DbFunctions.DiffDays(registrosEmbarque.fechaInicio, registrosEmbarque.fechaFinal),
                                   TotMes = DbFunctions.DiffMonths(registrosEmbarque.fechaInicio, registrosEmbarque.fechaFinal)
                               }).ToListAsync();
            return query;
        }
    }
}
