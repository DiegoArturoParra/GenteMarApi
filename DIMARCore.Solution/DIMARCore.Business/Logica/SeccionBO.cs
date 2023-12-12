using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
using System;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace DIMARCore.Business
{
    public class SeccionBO
    {
        #region seccion titulos
        public async Task<IEnumerable<GENTEMAR_SECCION_TITULOS>> GetSeccionesTitulos(bool? activo = true)
        {
            using (var repo = new SeccionTitulosRepository())
            {
                if (activo == null)
                {
                    return await repo.GetAllAsync();
                }
                return await repo.GetAllWithConditionAsync(x => x.activo == activo);
            }
        }

        public async Task<Respuesta> GetSeccionTitulo(int id)
        {
            var seccionTitulo = await new SeccionTitulosRepository().GetById(id);
            return seccionTitulo == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra la sección del titulo."))
                : Responses.SetOkResponse(seccionTitulo);
        }


        public async Task<Respuesta> CrearSeccionTitulo(GENTEMAR_SECCION_TITULOS obj)
        {
            using (var repo = new SeccionTitulosRepository())
            {
                await ExisteByNombreSeccionTituloAsync(obj.actividad_a_bordo.Trim().ToUpper());

                obj.actividad_a_bordo = obj.actividad_a_bordo.Trim().ToUpper();
                await repo.Create(obj);
                return Responses.SetCreatedResponse(obj);
            }
        }

        public async Task<Respuesta> ExisteSeccionTituloId(int id)
        {
            var existe = await new SeccionTitulosRepository().AnyWithCondition(x => x.id_seccion == id);
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontro la sección del titulo."));
            return Responses.SetOkResponse();
        }

        public async Task<Respuesta> EditarSeccionTitulo(GENTEMAR_SECCION_TITULOS obj)
        {
            await ExisteByNombreSeccionTituloAsync(obj.actividad_a_bordo.Trim().ToUpper(), obj.id_seccion);

            var respuesta = await GetSeccionTitulo(obj.id_seccion);

            using (var repo = new SeccionTitulosRepository())
            {
                var objeto = (GENTEMAR_SECCION_TITULOS)respuesta.Data;
                objeto.actividad_a_bordo = obj.actividad_a_bordo.Trim().ToUpper();
                await repo.Update(objeto);
                return Responses.SetUpdatedResponse(objeto);
            }
        }

        public async Task<Respuesta> AnulaOrActivaSeccionTitulo(int Id)
        {
            string mensaje;
            var obj = await GetSeccionTitulo(Id);
            var entidad = (GENTEMAR_SECCION_TITULOS)obj.Data;
            entidad.activo = !entidad.activo;
            await new SeccionTitulosRepository().Update(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.actividad_a_bordo}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.actividad_a_bordo}";
            }
            return Responses.SetOkResponse(entidad, mensaje);
        }

        public async Task ExisteByNombreSeccionTituloAsync(string nombre, int Id = 0)
        {
            bool existe;
            if (Id == 0)
            {
                existe = await new SeccionTitulosRepository().AnyWithCondition(x => x.actividad_a_bordo.Equals(nombre));
            }
            else
            {
                existe = await new SeccionTitulosRepository().AnyWithCondition(x => x.actividad_a_bordo.Equals(nombre) && x.id_seccion != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la sección {nombre}."));
        }

        #endregion

        #region seccion licencias
        public async Task<IEnumerable<SeccionDTO>> GetSeccionesLicencias()
        {
            return await new SeccionLicenciasRepository().GetTableSecciones();
        }

        public async Task<IEnumerable<GENTEMAR_SECCION_LICENCIAS>> GetSeccionesLicenciasActivas()
        {
            return await new SeccionLicenciasRepository().GetAllWithConditionAsync(x => x.activo == true);
        }

        /// <summary>
        /// Obtiene la lista de secciones dependiendo el id de la actividad
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SeccionDTO>> GetSeccionesActividad(int id)
        {
            return await new SeccionLicenciasRepository().GetSeccionActividad(id);
        }


        public async Task<Respuesta> CrearSeccionLicencia(GENTEMAR_SECCION_LICENCIAS entidad, IList<GENTEMAR_ACTIVIDAD> actividad)
        {
            using (var repo = new SeccionLicenciasRepository())
            {
                var validate = await repo.AnyWithCondition(x => x.actividad_a_bordo.Equals(entidad.actividad_a_bordo));
                if (validate)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la sección {entidad.actividad_a_bordo}."));
                entidad.activo = true;
                await repo.CrearActividadSeccion(entidad, actividad);
                return Responses.SetCreatedResponse(entidad);
            }
        }

        public async Task<Respuesta> EditarSeccionLicencia(GENTEMAR_SECCION_LICENCIAS entidad, IList<GENTEMAR_ACTIVIDAD> actividad)
        {
            using (var repo = new SeccionLicenciasRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_seccion == entidad.id_seccion);

                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la sección."));

                entidad.id_seccion = validate.id_seccion;
                entidad.activo = validate.activo;
                await new SeccionLicenciasRepository().ActualizarActividadSeccion(entidad, actividad);
                return Responses.SetUpdatedResponse(entidad);
            }
        }

        public async Task<Respuesta> GetSeccionLicencia(int id)
        {
            var seccionlicencia = await new SeccionLicenciasRepository().GetById(id);
            return seccionlicencia == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la sección."))
                : Responses.SetOkResponse(seccionlicencia);
        }

        public async Task<Respuesta> InactivarSeccionLicencia(int id)
        {
            using (var repo = new SeccionLicenciasRepository())
            {
                var validate = await repo.GetWithCondition(x => x.id_seccion == id);
                if (validate == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la sección."));
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }

        public async Task<IEnumerable<SeccionDTO>> GetSeccionesPorActividadesIds(List<int> ids)
        {
            return await new SeccionLicenciasRepository().GetSeccionesPorActividadesIds(ids);
        }
        #endregion
    }
}






