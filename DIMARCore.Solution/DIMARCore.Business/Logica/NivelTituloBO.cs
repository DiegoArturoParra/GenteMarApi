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
    public class NivelTituloBO : IGenericCRUD<GENTEMAR_NIVEL, int>
    {
        public IEnumerable<GENTEMAR_NIVEL> GetAll(bool? activo = true)
        {
            using (var repo = new NivelTituloRepository())
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
            var nivel = await new NivelTituloRepository().GetByIdAsync(Id);
            return nivel == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No existe el nivel solicitado."))
                : Responses.SetOkResponse(nivel);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_NIVEL entidad)
        {
            await ExisteByNombreAsync(entidad.nivel.Trim().ToUpper());
            entidad.nivel = entidad.nivel.Trim().ToUpper();
            await new NivelTituloRepository().Create(entidad);
            return Responses.SetCreatedResponse(entidad);
        }

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_NIVEL entidad)
        {

            await ExisteByNombreAsync(entidad.nivel.Trim().ToUpper(), entidad.id_nivel);

            var respuesta = await GetByIdAsync(entidad.id_nivel);

            var objeto = (GENTEMAR_NIVEL)respuesta.Data;
            objeto.nivel = entidad.nivel.Trim().ToUpper();
            await new NivelTituloRepository().Update(objeto);

            return Responses.SetUpdatedResponse(objeto);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);

            var entidad = (GENTEMAR_NIVEL)obj.Data;
            entidad.activo = !entidad.activo;
            await new NivelTituloRepository().Update(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.nivel}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.nivel}";
            }
            return Responses.SetOkResponse(entidad, mensaje);
        }


        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;

            if (Id == 0)
            {
                existe = await new NivelTituloRepository().AnyWithConditionAsync(x => x.nivel.Equals(nombre));
            }
            else
            {
                existe = await new NivelTituloRepository().AnyWithConditionAsync(x => x.nivel.Equals(nombre) && x.id_nivel != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el nivel {nombre}"));
        }

        public async Task<IEnumerable<NivelDTO>> GetNivelTituloByCargoReglaId(IdsTablasForaneasDTO ids)
        {
            var Nivel = await new ReglaCargoRepository().GetIdNivelForReglaCargo(ids);
            return Nivel ?? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No hay ninguna relación con la regla, capacidad y cargo solicitado."));
        }

        public async Task<IEnumerable<GENTEMAR_NIVEL>> GetAllAsync(bool? activo = true)
        {
            using (var repo = new NivelTituloRepository())
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
