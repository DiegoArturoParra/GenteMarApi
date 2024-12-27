using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
using System.Linq;
using System.Data;

namespace DIMARCore.Business.Logica
{
    public class ClaseTituloBO : IGenericCRUD<GENTEMAR_CLASE_TITULOS, int>
    {
        public IEnumerable<GENTEMAR_CLASE_TITULOS> GetAll(bool? activo = true)
        {
            using (var repo = new ClaseTitulosRepository())
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

            var entidad = await new ClaseTitulosRepository().GetByIdAsync(Id);
            return entidad == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No encuentra registrada la clase del titulo."))
                : Responses.SetOkResponse(entidad);
        }
        public async Task<Respuesta> CrearAsync(GENTEMAR_CLASE_TITULOS entidad)
        {

            await ExisteByNombreAsync(entidad.descripcion_clase);

            await new ClaseTitulosRepository().Create(entidad);

            return Responses.SetCreatedResponse(entidad);
        }
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_CLASE_TITULOS entidad)
        {

            await ExisteByNombreAsync(entidad.descripcion_clase, entidad.id_clase);
            var respuesta = await GetByIdAsync(entidad.id_clase);

            var objeto = (GENTEMAR_CLASE_TITULOS)respuesta.Data;
            objeto.descripcion_clase = entidad.descripcion_clase;
            objeto.sigla = entidad.sigla;
            await new ClaseTitulosRepository().Update(objeto);

            return Responses.SetUpdatedResponse(objeto);
        }
        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;
            if (Id == 0)
            {
                existe = await new ClaseTitulosRepository().AnyWithConditionAsync(x => x.descripcion_clase.Equals(nombre));
            }
            else
            {
                existe = await new ClaseTitulosRepository().AnyWithConditionAsync(x => x.descripcion_clase.Equals(nombre) && x.id_clase != Id);
            }

            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la clase {nombre}"));
        }
        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);
            var entidad = (GENTEMAR_CLASE_TITULOS)obj.Data;
            entidad.activo = !entidad.activo;
            await new ClaseTitulosRepository().Update(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.descripcion_clase}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.descripcion_clase}";
            }

            return Responses.SetOkResponse(entidad, mensaje);
        }

        public async Task<IEnumerable<GENTEMAR_CLASE_TITULOS>> GetAllAsync(bool? activo = true)
        {
            using (var repo = new ClaseTitulosRepository())
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






