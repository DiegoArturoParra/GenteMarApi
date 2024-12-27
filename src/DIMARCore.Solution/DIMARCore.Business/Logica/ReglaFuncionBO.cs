using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DIMARCore.Business.Logica
{
    public class ReglaFuncionBO
    {

        public async Task<IEnumerable<InfoReglaFuncionDTO>> GetAll()
        {
            return await new ReglaFuncionRepository().GetDetalles();
        }

        public async Task<Respuesta> CrearAsync(ReglaFuncionDTO entidad)
        {
            using (var repo = new ReglaFuncionRepository())
            {
                List<GENTEMAR_REGLA_FUNCION> funcionesConReglaAdd = await IsExistReglaFuncion(entidad);
                await repo.CreateInCascade(funcionesConReglaAdd);
                return Responses.SetCreatedResponse();
            }
        }

        public async Task<Respuesta> ActualizarAsync(ReglaFuncionDTO entidad)
        {
            using (var repo = new ReglaFuncionRepository())
            {
                List<GENTEMAR_REGLA_FUNCION> funcionesConReglaAdd = await IsExistReglaFuncion(entidad, true);
                await repo.UpdateInCascade(funcionesConReglaAdd, entidad.ReglaId);
                return Responses.SetUpdatedResponse();
            }
        }

        public async Task<IEnumerable<ReglaDTO>> ReglasSinFunciones()
        {
            return await new ReglaFuncionRepository().GetReglasSinFunciones();
        }

        public async Task<List<GENTEMAR_REGLA_FUNCION>> IsExistReglaFuncion(ReglaFuncionDTO entidad, bool update = false)
        {
            bool existeRelacion = false;
            List<GENTEMAR_REGLA_FUNCION> entidades = new List<GENTEMAR_REGLA_FUNCION>();
            await new ReglaBO().ExisteReglaById(entidad.ReglaId);
            foreach (var item in entidad.Funciones)
            {
                var existeFuncion = await new FuncionRepository().AnyWithConditionAsync(y => y.id_funcion == item);

                if (!existeFuncion)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No existe la función solicitada."));

                if (!update)
                    existeRelacion = await new ReglaFuncionRepository().AnyWithConditionAsync(x => x.id_regla == entidad.ReglaId && x.id_funcion == item);

                if (existeRelacion)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("Ya existe la relación indicada, ingrese una valida."));


                entidades.Add(new GENTEMAR_REGLA_FUNCION()
                {
                    id_regla = entidad.ReglaId,
                    id_funcion = item
                });

            }
            return entidades;
        }
    }
}
