﻿using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Interfaces
{
    public interface ITitulosBO
    {
        Task<Respuesta> ActualizarAsync(GENTEMAR_TITULOS entidad, string pathActual);
        Task<Respuesta> CrearAsync(GENTEMAR_TITULOS entidad, string pathActual);
        Task ExistById(long id);
        Task<Respuesta> ExistePersonaByIdentificacion(string identificacionConPuntos);
        Task<PlantillaTituloDTO> GetPlantillaTitulos(long id);
        Task<Respuesta> GetTituloById(long id);
        Task<IEnumerable<ListarTituloDTO>> GetTitulosFiltro(string identificacionConPuntos, long Id = 0);
        IQueryable<ListarTituloDTO> GetTitulosQueryable();
    }
}
