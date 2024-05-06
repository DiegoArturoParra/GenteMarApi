using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.UIEntities.QueryFilters.Reports;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class CargoLicenciaBO
    {

        private CargoLicenciaRepository _repository;

        public CargoLicenciaBO()
        {
            _repository = new CargoLicenciaRepository();
        }


        /// <summary>
        /// lista de cargos por un filtro
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CargoInfoLicenciaDTO>> GetCargosLicenciaAsync(CargoLicenciaFilter filtro)
        {
            var data = await _repository.GetCargosLicenciaAsync(filtro);
            return data;
        }


        /// <summary>
        /// Lista de Cargos licencia activo
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public async Task<IEnumerable<CargoInfoLicenciaDTO>> GetCargosLicenciaActivosPorCapitaniaCategoria()
        {
            var claimIdCategoria = ClaimsHelper.GetCategoriaUsuario();
            var data = await _repository.GetCargosLicenciaActivosPorCapitaniaCategoria(claimIdCategoria);
            return data;
        }

        /// <summary>
        /// Actializar  cargo licencia
        /// </summary>
        /// <returns>respuesta </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_CARGO_LICENCIA entidad)
        {
            Respuesta respuesta = new Respuesta();

            using (var repo = _repository)
            {
                var existeCargoLicencia = await repo.AnyWithConditionAsync(x => x.cargo_licencia.Equals(entidad.cargo_licencia)
                && x.id_cargo_licencia != entidad.id_cargo_licencia);

                if (existeCargoLicencia)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El cargo licencia {entidad.cargo_licencia} ya se encuentra registrado."));

                var data = await repo.GetByIdAsync(entidad.id_cargo_licencia);

                var actividadSeccion = await new ActividadSeccionLicenciasRepository().GetWithConditionAsync(x => x.id_actividad
                     == entidad.IdActividad && x.id_seccion == entidad.IdSeccion);
                if (actividadSeccion == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La relación de actividad y sección no existe."));

                var seccionClase = await new SeccionClaseRepository().GetWithConditionAsync(x => x.id_seccion == entidad.IdSeccion
                && x.id_clase == entidad.IdClase);

                if (seccionClase == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La relación de sección y clase no existe."));

                data.cargo_licencia = entidad.cargo_licencia;
                data.vigencia = entidad.vigencia;
                data.IdTipoLicencia = entidad.IdTipoLicencia;
                data.IdActividad = entidad.IdActividad;
                data.IdSeccion = entidad.IdSeccion;
                data.IdClase = entidad.IdClase;
                data.id_actividad_seccion_licencia = actividadSeccion.id_actividad_seccion_licencia;
                data.id_seccion_clase = seccionClase.id_seccion_clase;
                data.IdCategoria = entidad.IdCategoria;
                data.IdLimitacion = entidad.IdLimitacion;
                data.IdLimitante = entidad.IdLimitante;
                await _repository.ModificarCargoLimitacion(data);
                return Responses.SetUpdatedResponse();
            }
        }

        /// <summary>
        /// Crear cargo licencia
        /// </summary>
        /// <returns>respuesta </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>

        public async Task<Respuesta> CrearAsync(GENTEMAR_CARGO_LICENCIA entidad)
        {
            Respuesta respuesta = new Respuesta();

            using (var repo = _repository)
            {
                var data = await repo.AnyWithConditionAsync(x => x.cargo_licencia.Equals(entidad.cargo_licencia));
                if (data)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"El cargo licencia {entidad.cargo_licencia} ya se encuentra registrado."));

                var actividadSeccion = await new ActividadSeccionLicenciasRepository().GetWithConditionAsync(x => x.id_actividad
                == entidad.IdActividad && x.id_seccion == entidad.IdSeccion);
                if (actividadSeccion == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La relación de actividad y sección no existe."));

                var seccionClase = await new SeccionClaseRepository().GetWithConditionAsync(x => x.id_seccion == entidad.IdSeccion
                && x.id_clase == entidad.IdClase);

                if (seccionClase == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"La relación de sección y clase no existe."));

                entidad.id_actividad_seccion_licencia = actividadSeccion.id_actividad_seccion_licencia;
                entidad.id_seccion_clase = seccionClase.id_seccion_clase;
                await repo.CrearCargoLimitacion(entidad);
                entidad.codigo_licencia = await CodigoLicenciaAsync(entidad, 0);
                await _repository.Update(entidad);
                return Responses.SetCreatedResponse();
            }
        }


        /// <summary>
        /// codigo licencia 
        /// </summary>
        /// <returns>codigo licencia generado  </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        private async Task<string> CodigoLicenciaAsync(GENTEMAR_CARGO_LICENCIA licencia, int fecha)
        {
            var codigo = "";
            if (fecha == 0)
            {
                fecha = DateTime.Now.Year;
            }
            else
            {
                fecha = fecha + 1;
            }
            codigo = $"{licencia.cargo_licencia.Substring(0, 1)}{fecha}{licencia.id_cargo_licencia}";
            var data = await _repository.AnyWithConditionAsync(x => x.codigo_licencia == codigo);
            if (data)
            {
                codigo = await CodigoLicenciaAsync(licencia, fecha);
            }

            return codigo;
        }

        /// <summary>
        /// Lista de cargo licencia id
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public async Task<CargoInfoLicenciaDTO> GetCargoLicenciaIdAsync(long id)
        {
            var data = await _repository.GetCargoLicenciaIdAsync(id)
                ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro la licencia solicitada");
            // Obtiene la lista
            return data;
        }
        /// <summary>
        /// Lista de cargo licencia id detalle
        /// </summary>
        /// <returns>Lista de Cargo </returns>
        /// <entidad>CARGO </entidad>
        /// <tabla>GENTEMAR_CARGO_LICENCIA</tabla>
        public async Task<CargoInfoLicenciaDTO> GetCargoLicenciaIdDetalleAsync(long id)
        {
            var data = await _repository.GetCargoLicenciaIdDetalleAsync(id);
            data.FechaExpedicion = DateTime.Now.Date;
            data.FechaVencimiento = DateTime.Now.AddDays((double)data.Vigencia).Date;
            data.MaxDateFechaVencimiento = data.FechaVencimiento.AddMonths(1).Date;
            // Obtiene la lista
            return data;
        }


        /// <summary>
        /// cambia el estado del Cargo Licencia 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respuesta> CambiarCargoLicencia(int id)
        {
            using (var repo = _repository)
            {
                var validate = await repo.GetWithConditionAsync(x => x.id_cargo_licencia == id)
                    ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encontro la licencia solicitada");
                validate.activo = !validate.activo;
                await repo.Update(validate);
                return Responses.SetUpdatedResponse(validate);
            }
        }

        /// <summary>
        /// metodo para obtener los datos del usuario y de la licencia para la plantilla
        /// por id de la licencia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlantillaLicenciaDTO> GetPlantillaLicencias(long id)
        {
            var data = await _repository.GetPlantillaLicencias(id);

            return data;
        }

        public async Task<IEnumerable<CargoLicenciaDTO>> GetCargosLicenciaActivosPorFiltroParaReporte(CargoLicenciaReportFilter cargoLicenciaFilter)
        {
            return await _repository.GetCargosLicenciaActivosPorFiltroParaReporte(cargoLicenciaFilter);
        }
    }
}
