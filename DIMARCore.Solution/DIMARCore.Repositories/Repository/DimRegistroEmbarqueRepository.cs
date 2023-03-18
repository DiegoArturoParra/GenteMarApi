using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class DimRegistroEmbarqueRepository : GenericRepository<DIM_REGISTRO_EMBARQUES>
    {
        public List<DimRegistroEmbarqueDTO> GetDimRegistroEmbarque(string identificacionConPuntos)
        {
            var query = (from registrosEmbarque in _context.TABLA_DIM_REGISTRO_EMBARQUE
                         join dimPersonas in _context.TABLA_DIM_PERSONAS on
                            registrosEmbarque.idpersona equals dimPersonas.idpersona
                         where dimPersonas.cedula == identificacionConPuntos
                         select new DimRegistroEmbarqueDTO
                         {
                             MatriculaOMI = registrosEmbarque.matriculaOMI,
                             NombreNave = registrosEmbarque.nombreNave,
                             Cargo = registrosEmbarque.cargo,
                             Grado = registrosEmbarque.grado,
                             FechaInicio = registrosEmbarque.fechaInicio,
                             FechaFinal = registrosEmbarque.fechaFinal,
                             FechaRegistro = registrosEmbarque.FechaRegistro,                             
                         }).ToList();

            foreach (var data in query)
            {
                data.DifDias = (data.FechaFinal - data.FechaInicio).Days;
                data.TotMes = (data.FechaFinal - data.FechaInicio).Days / 30;
            }

            return query;
        }
    }
}
