using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;

namespace DIMARCore.Business.Logica
{
    public class TramiteEstupefacienteBO : IGenericCRUD<GENTEMAR_TIPO_TRAMITE, int>
    {

        public IEnumerable<GENTEMAR_TIPO_TRAMITE> GetAll(bool? activo = true)
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
            var entidad = await new TramiteEstupefacienteRepository().GetById(Id);
            if (entidad == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra el tipo de tramite indicado"));
            return Responses.SetOkResponse(entidad);
        }
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_TIPO_TRAMITE entidad)
        {
            await ExisteByNombreAsync(entidad.descripcion_tipo_tramite.Trim(), entidad.id_tipo_tramite);

            var respuesta = await GetByIdAsync(entidad.id_tipo_tramite);

            var obj = (GENTEMAR_TIPO_TRAMITE)respuesta.Data;
            obj.descripcion_tipo_tramite = entidad.descripcion_tipo_tramite;
            await new TramiteEstupefacienteRepository().Update(obj);

            return Responses.SetUpdatedResponse(obj);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_TIPO_TRAMITE entidad)
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
                existe = await new TramiteEstupefacienteRepository().AnyWithCondition(x => x.descripcion_tipo_tramite.Equals(nombre));
            }
            else
            {
                existe = await new TramiteEstupefacienteRepository().AnyWithCondition(x => x.descripcion_tipo_tramite.Equals(nombre) && x.id_tipo_tramite != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el tramite {nombre}"));


        }
        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);

            var entidad = (GENTEMAR_TIPO_TRAMITE)obj.Data;
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
    }
}
