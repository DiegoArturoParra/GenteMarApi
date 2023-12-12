using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;

namespace DIMARCore.Business.Logica
{
    public class CapacidadBO : IGenericCRUD<GENTEMAR_CAPACIDAD, int>
    {
        public IEnumerable<GENTEMAR_CAPACIDAD> GetAll(bool? activo = true)
        {
            using (var repo = new CapacidadRepository())
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
            var entidad = await new CapacidadRepository().GetById(Id);
            return entidad == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra la capacidad."))
                : Responses.SetOkResponse(entidad);
        }
        public async Task<Respuesta> ExisteCapacidadById(int capacidadId)
        {
            var existe = await new CapacidadRepository().AnyWithCondition(x => x.id_capacidad == capacidadId);
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra la capacidad."));
            return Responses.SetOkResponse();
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_CAPACIDAD entidad)
        {
            using (var repo = new CapacidadRepository())
            {
                await ExisteByNombreAsync(entidad.capacidad);
                entidad.capacidad = entidad.capacidad.Trim();
                await repo.Create(entidad);
            }
            return Responses.SetCreatedResponse();
        }

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_CAPACIDAD entidad)
        {

            await ExisteByNombreAsync(entidad.capacidad, entidad.id_capacidad);
            var respuesta = await GetByIdAsync(entidad.id_capacidad);
            var objeto = (GENTEMAR_CAPACIDAD)respuesta.Data;
            objeto.capacidad = entidad.capacidad.Trim();
            await new CapacidadRepository().Update(objeto);
            return Responses.SetUpdatedResponse();
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            Respuesta respuesta = new Respuesta();

            var obj = await GetByIdAsync(Id);

            var entidad = (GENTEMAR_CAPACIDAD)obj.Data;
            entidad.activo = !entidad.activo;
            await new CapacidadRepository().Update(entidad);
            if (entidad.activo)
            {
                respuesta.Mensaje = $"Se activo {entidad.capacidad}";
            }
            else
            {
                respuesta.Mensaje = $"Se anulo {entidad.capacidad}";
            }
            return Responses.SetOkResponse(obj, respuesta.Mensaje);
        }

        public async Task IsExistItemsCargoAndRegla(IdsLlaveCompuestaDTO itemsId)
        {
            await new ReglaBO().ExisteReglaById(itemsId.ReglaId);
            await new CargoTituloBO().ExisteCargoTituloById(itemsId.CargoTituloId);
        }
        public async Task<IEnumerable<GENTEMAR_REGLAS_CARGO>> CapacidadesActivasByReglaCargo(IdsLlaveCompuestaDTO items)
        {
            await IsExistItemsCargoAndRegla(items);
            return await new CapacidadRepository().CapacidadesActivasByReglaCargo(items);
        }
        public async Task ExisteByNombreAsync(string nombre, int id = 0)
        {
            bool existe;
            if (id == 0)
            {
                existe = await new CapacidadRepository().AnyWithCondition(x => x.capacidad.Equals(nombre.Trim().ToUpper()));
            }
            else
            {
                existe = await new CapacidadRepository().AnyWithCondition(x => x.capacidad.Equals(nombre.Trim().ToUpper()) && x.id_capacidad != id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la capacidad {nombre}"));

        }
    }
}
