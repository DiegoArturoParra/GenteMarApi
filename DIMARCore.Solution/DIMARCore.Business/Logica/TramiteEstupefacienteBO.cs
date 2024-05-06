using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;

namespace DIMARCore.Business.Logica
{
    public class TramiteEstupefacienteBO : IGenericCRUD<GENTEMAR_TRAMITE_ANTECEDENTE, int>
    {

        public IEnumerable<GENTEMAR_TRAMITE_ANTECEDENTE> GetAll(bool? activo = true)
        {
            using (var repo = new TramiteEstupefacienteRepository())
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
            var entidad = await new TramiteEstupefacienteRepository().GetByIdAsync(Id);
            if (entidad == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra el tipo de tramite indicado"));
            return Responses.SetOkResponse(entidad);
        }
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_TRAMITE_ANTECEDENTE entidad)
        {
            await ExisteByNombreAsync(entidad.descripcion_tipo_tramite.Trim(), entidad.id_tipo_tramite);

            var respuesta = await GetByIdAsync(entidad.id_tipo_tramite);

            var obj = (GENTEMAR_TRAMITE_ANTECEDENTE)respuesta.Data;
            obj.descripcion_tipo_tramite = entidad.descripcion_tipo_tramite;
            await new TramiteEstupefacienteRepository().Update(obj);

            return Responses.SetUpdatedResponse(obj);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_TRAMITE_ANTECEDENTE entidad)
        {
            await ExisteByNombreAsync(entidad.descripcion_tipo_tramite.Trim().ToUpper());
            entidad.descripcion_tipo_tramite = entidad.descripcion_tipo_tramite.Trim();
            await new TramiteEstupefacienteRepository().Create(entidad);
            return Responses.SetCreatedResponse(entidad);
        }

        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;
            Respuesta respuesta = new Respuesta();
            if (Id == 0)
            {
                existe = await new TramiteEstupefacienteRepository().AnyWithConditionAsync(x => x.descripcion_tipo_tramite.Equals(nombre));
            }
            else
            {
                existe = await new TramiteEstupefacienteRepository().AnyWithConditionAsync(x => x.descripcion_tipo_tramite.Equals(nombre) && x.id_tipo_tramite != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el tramite {nombre}"));


        }
        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);

            var entidad = (GENTEMAR_TRAMITE_ANTECEDENTE)obj.Data;
            entidad.activo = !entidad.activo;
            await new TramiteEstupefacienteRepository().Update(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.descripcion_tipo_tramite}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.descripcion_tipo_tramite}";
            }
            return Responses.SetOkResponse(entidad, mensaje);
        }

        public async Task<IEnumerable<GENTEMAR_TRAMITE_ANTECEDENTE>> GetAllAsync(bool? activo = true)
        {
            using (var repo = new TramiteEstupefacienteRepository())
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
