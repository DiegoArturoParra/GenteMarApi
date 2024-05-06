using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business.Logica
{
    public class EntidadBO : IGenericCRUD<GENTEMAR_ENTIDAD_ANTECEDENTE, int>
    {
        public IEnumerable<GENTEMAR_ENTIDAD_ANTECEDENTE> GetAll(bool? activo = true)
        {
            using (var repo = new EntidadEstupefacienteRepository())
            {
                if (activo == null)
                {
                    return repo.GetAll();
                }
                else
                {
                    return repo.GetAllWithCondition(x => x.activo == activo);
                }
            }
        }

        public async Task<Respuesta> GetByIdAsync(int Id)
        {
            var entidad = await new EntidadEstupefacienteRepository().GetByIdAsync(Id);

            if (entidad == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No encuentra registrada la entidad"));

            return Responses.SetOkResponse(entidad);
        }
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_ENTIDAD_ANTECEDENTE objeto)
        {

            await ExisteByNombreAsync(objeto.entidad.Trim().ToUpper(), objeto.id_entidad);

            var respuesta = await GetByIdAsync(objeto.id_entidad);

            var obj = (GENTEMAR_ENTIDAD_ANTECEDENTE)respuesta.Data;
            obj.entidad = objeto.entidad;
            await new EntidadEstupefacienteRepository().Update(obj);

            return Responses.SetUpdatedResponse(obj);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            Respuesta respuesta = new Respuesta();

            var obj = await GetByIdAsync(Id);

            var entidad = (GENTEMAR_ENTIDAD_ANTECEDENTE)obj.Data;
            entidad.activo = !entidad.activo;
            await new EntidadEstupefacienteRepository().Update(entidad);
            if (entidad.activo)
            {
                respuesta.Mensaje = $"Se activo {entidad.entidad}";
            }
            else
            {
                respuesta.Mensaje = $"Se anulo {entidad.entidad}";
            }
            return Responses.SetOkResponse(entidad, respuesta.Mensaje);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_ENTIDAD_ANTECEDENTE entidad)
        {
            await ExisteByNombreAsync(entidad.entidad.Trim().ToUpper());

            entidad.entidad = entidad.entidad.Trim().ToUpper();
            await new EntidadEstupefacienteRepository().Create(entidad);

            return Responses.SetCreatedResponse(entidad);
        }

        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;

            if (Id == 0)
            {
                existe = await new EntidadEstupefacienteRepository().AnyWithConditionAsync(x => x.entidad.Equals(nombre));
            }
            else
            {
                existe = await new EntidadEstupefacienteRepository().AnyWithConditionAsync(x => x.entidad.Equals(nombre) && x.id_entidad != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la entidad {nombre}"));
        }

        public async Task<IEnumerable<GENTEMAR_ENTIDAD_ANTECEDENTE>> GetAllAsync(bool? activo = true)
        {
            using (var repo = new EntidadEstupefacienteRepository())
            {
                if (activo == null)
                {
                    return await repo.GetAllAsync();
                }
                else
                {
                    return await repo.GetAllWithConditionAsync(x => x.activo == activo);
                }
            }
        }
    }
}
