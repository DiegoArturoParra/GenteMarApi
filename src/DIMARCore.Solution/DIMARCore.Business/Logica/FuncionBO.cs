using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
using DIMARCore.UIEntities.DTOs;
using System.Linq;
namespace DIMARCore.Business.Logica
{
    public class FuncionBO : IGenericCRUD<GENTEMAR_FUNCIONES, int>
    {
        public IEnumerable<GENTEMAR_FUNCIONES> GetAll(bool? activo = true)
        {
            using (var repo = new FuncionRepository())
            {
                if (activo == null)
                {
                    return repo.GetAll();
                }
                return repo.GetAllWithCondition(x => x.activo == activo);
            }
        }

        public async Task<Respuesta> GetByIdAsync(int Id)
        {
            var funcion = await new FuncionRepository().GetByIdAsync(Id);
            return funcion == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra la función digitada."))
                : Responses.SetOkResponse(funcion);
        }
        public async Task<Respuesta> CrearAsync(GENTEMAR_FUNCIONES entidad)
        {
            await ExisteByNombreAsync(entidad.funcion.Trim().ToUpper());
            entidad.funcion = entidad.funcion.Trim().ToUpper();
            await new FuncionRepository().Create(entidad);
            return Responses.SetCreatedResponse(entidad);
        }

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_FUNCIONES entidad)
        {

            await ExisteByNombreAsync(entidad.funcion.Trim().ToLower(), entidad.id_funcion);

            var respuesta = await GetByIdAsync(entidad.id_funcion);

            var objeto = (GENTEMAR_FUNCIONES)respuesta.Data;
            objeto.funcion = entidad.funcion;
            objeto.limitacion_funcion = entidad.limitacion_funcion;
            await new FuncionRepository().Update(objeto);

            return Responses.SetUpdatedResponse(objeto);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            var obj = await GetByIdAsync(Id);
            var entidad = (GENTEMAR_FUNCIONES)obj.Data;
            entidad.activo = !entidad.activo;
            await new FuncionRepository().Update(entidad);
            if (entidad.activo)
                return Responses.SetOkResponse(entidad, $"Se activo {entidad.funcion}");

            return Responses.SetOkResponse(entidad, $"Se anulo {entidad.funcion}");
        }

        public async Task<Respuesta> IsExistRegla(int reglaId)
        {
            return await new ReglaBO().ExisteReglaById(reglaId);
        }

        public async Task<IEnumerable<FuncionDTO>> GetFuncionesByRegla(int reglaId, bool isShowAll)
        {
            await IsExistRegla(reglaId);
            var data = await new FuncionRepository().GetFuncionesByReglaId(reglaId, isShowAll);
            if (!data.Any())
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontraron funciones para la regla seleccionada."));
            return data;
        }

        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;

            if (Id == 0)
            {
                existe = await new FuncionRepository().AnyWithConditionAsync(x => x.funcion.Equals(nombre));
            }
            else
            {
                existe = await new FuncionRepository().AnyWithConditionAsync(x => x.funcion.Equals(nombre) && x.id_funcion != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la función {nombre}"));
        }

        public async Task<IEnumerable<GENTEMAR_FUNCIONES>> GetAllAsync(bool? activo = true)
        {
            using (var repo = new FuncionRepository())
            {
                if (activo == null)
                {
                    return await repo.GetAllAsync();
                }
                return await repo.GetAllWithConditionAsync(x => x.activo == activo);
            }
        }
    }
}
