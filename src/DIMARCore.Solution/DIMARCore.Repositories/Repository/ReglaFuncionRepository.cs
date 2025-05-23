﻿using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Repositories.Repository
{
    public class ReglaFuncionRepository : GenericRepository<GENTEMAR_REGLA_FUNCION>
    {
        public async Task<IEnumerable<InfoReglaFuncionDTO>> GetDetalles()
        {
            var listaAgrupada = _context.GENTEMAR_REGLA_FUNCION.Include(x => x.GENTEMAR_FUNCIONES).Include(x => x.GENTEMAR_REGLAS)
                .GroupBy(p => new { p.id_regla });

            return await listaAgrupada.Select(x => new InfoReglaFuncionDTO
            {
                ReglaId = x.Key.id_regla,
                Regla = x.Select(y => y.GENTEMAR_REGLAS.nombre_regla).FirstOrDefault(),
                Funciones = x.ToList().Select(y => new InfoFuncionDTO
                {
                    Descripcion = y.GENTEMAR_FUNCIONES.funcion,
                    Id = y.GENTEMAR_FUNCIONES.id_funcion,
                    IsActive = y.GENTEMAR_FUNCIONES.activo
                }).ToList()
            }).ToListAsync();
        }

        private async Task<ReglaFuncionDTO> FuncionesByReglaId(int ReglaId)
        {
            return await _context.GENTEMAR_REGLA_FUNCION.Where(x => x.id_regla == ReglaId).GroupBy
                 (x => x.id_regla).Select(x => new ReglaFuncionDTO
                 { Funciones = x.Select(y => y.id_funcion).ToList(), ReglaId = x.Key })
                 .FirstOrDefaultAsync();
        }
        private async Task<IEnumerable<int>> ReglasInDetalle() =>
                 await _context.GENTEMAR_REGLA_FUNCION.GroupBy(x => x.id_regla).Select(y => y.Key).ToListAsync();

        public async Task UpdateInCascade(List<GENTEMAR_REGLA_FUNCION> entidades, int reglaId)
        {
            _context.GENTEMAR_REGLA_FUNCION.RemoveRange(_context.GENTEMAR_REGLA_FUNCION.Where(x => x.id_regla == reglaId));
            await CreateInCascade(entidades);
        }

        public async Task CreateInCascade(List<GENTEMAR_REGLA_FUNCION> entidades)
        {
            _context.GENTEMAR_REGLA_FUNCION.AddRange(entidades);
            await SaveAllAsync();
        }

        public async Task<IEnumerable<ReglaDTO>> GetReglasSinFunciones()
        {
            var listado = await ReglasInDetalle();
            return await _context.GENTEMAR_REGLAS.Where(x => !listado.Contains(x.id_regla) && x.activo == Constantes.ACTIVO).Select(x => new ReglaDTO
            {
                Descripcion = x.nombre_regla,
                Id = x.id_regla,
                IsActive = x.activo
            }).ToListAsync();
        }
    }
}
