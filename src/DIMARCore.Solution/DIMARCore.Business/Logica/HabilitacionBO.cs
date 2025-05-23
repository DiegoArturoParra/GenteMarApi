﻿using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DIMARCore.Business.Logica
{
    public class HabilitacionBO : IGenericCRUD<GENTEMAR_HABILITACION, int>
    {
        public IEnumerable<GENTEMAR_HABILITACION> GetAll(bool? activo = true)
        {
            using (var repo = new HabilitacionRepository())
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
            var habilitacion = await new HabilitacionRepository().GetByIdAsync(Id);
            return habilitacion == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la habilitación."))
                : Responses.SetOkResponse(habilitacion);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_HABILITACION entidad)
        {
            await ExisteByNombreAsync(entidad.habilitacion);
            entidad.habilitacion = entidad.habilitacion.Trim();
            await new HabilitacionRepository().Create(entidad);
            return Responses.SetCreatedResponse(entidad);
        }
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_HABILITACION entidad)
        {

            await ExisteByNombreAsync(entidad.habilitacion, entidad.id_habilitacion);

            var respuesta = await GetByIdAsync(entidad.id_habilitacion);

            var objeto = (GENTEMAR_HABILITACION)respuesta.Data;
            objeto.habilitacion = entidad.habilitacion.Trim();
            await new HabilitacionRepository().Update(objeto);

            return Responses.SetUpdatedResponse(objeto);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);

            var entidad = (GENTEMAR_HABILITACION)obj.Data;
            entidad.activo = !entidad.activo;
            await new HabilitacionRepository().Update(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.habilitacion}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.habilitacion}";
            }
            return Responses.SetOkResponse(entidad, mensaje);
        }

        public async Task<IEnumerable<GENTEMAR_REGLA_CARGO_HABILITACION>> GetHabilitacionesActivasByReglaCargoId(int CargoReglaId)
        {
            return await new HabilitacionRepository().GetHabilitacionesActivasByReglaCargoId(CargoReglaId);
        }

        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;

            if (Id == 0)
            {
                existe = await new HabilitacionRepository().AnyWithConditionAsync(x => x.habilitacion.Equals(nombre.Trim().ToUpper()));
            }
            else
            {
                existe = await new HabilitacionRepository().AnyWithConditionAsync(x => x.habilitacion.Equals(nombre.Trim().ToUpper()) && x.id_habilitacion != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la habilitación {nombre}"));
        }

        public async Task<IEnumerable<GENTEMAR_HABILITACION>> GetAllAsync(bool? activo = true)
        {
            using (var repo = new HabilitacionRepository())
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
