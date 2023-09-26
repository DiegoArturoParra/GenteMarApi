using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ReglaCargoBO
    {
        public async Task<Respuesta> GetIdByTablasForaneas(IdsTablasForaneasDTO idsTablas)
        {
            var CargoReglaId = await new ReglaCargoRepository().GetIdReglaCargo(idsTablas);
            return CargoReglaId == 0
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrado el id cargo regla."))
                : Responses.SetOkResponse(CargoReglaId);
        }

        public async Task<Respuesta> ExisteCargoTituloInDetalleRegla(int cargoId)
        {
            var HayCargoTituloInDetalleReglas = await new ReglaCargoRepository().ExisteCargoTituloInDetalleRegla(cargoId);
            return !HayCargoTituloInDetalleReglas
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("Todavia no hay reglas por el cargo del titulo digitado."))
                : Responses.SetOkResponse();
        }

        public async Task<Respuesta> GetById(int id)
        {
            using (var repo = new ReglaCargoRepository())
            {
                var existe = await repo.AnyWithCondition(x => x.id_cargo_regla == id);
                return !existe
                    ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No existe la relación."))
                    : Responses.SetOkResponse(await repo.GetDetalleById(id));
            }
        }

        public IEnumerable<ListadoDetalleCargoReglaDTO> GetListado(DetalleReglaFilter filtro)
        {
            return new ReglaCargoRepository().GetListado(filtro);
        }

        public async Task<Respuesta> CrearCargoRegla(GENTEMAR_REGLAS_CARGO data)
        {
            await ValidarFormularioAsync(data);
            await new ReglaCargoRepository().CrearRelacionReglaCargo(data);
            return Responses.SetCreatedResponse(data);
        }
        public async Task<Respuesta> EditarCargoRegla(GENTEMAR_REGLAS_CARGO data)
        {

            await ValidarFormularioAsync(data, true);

            using (var repo = new ReglaCargoRepository())
            {
                var entidad = await repo.GetWithCondition(x => x.id_cargo_regla == data.id_cargo_regla);
                if (entidad == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe la relación con cargo-regla."));
                entidad.Habilitaciones = data.Habilitaciones;
                entidad.id_nivel = data.id_nivel;
                entidad.id_capacidad = data.id_capacidad;
                await repo.ActualizarRelacionReglaCargo(entidad);
                return Responses.SetUpdatedResponse(entidad);

            }
        }

        private async Task ValidarFormularioAsync(GENTEMAR_REGLAS_CARGO entidad, bool Update = false)
        {

            if (entidad.id_cargo_regla > 0)
            {
                var existeCargoRegla = await new ReglaCargoRepository().AnyWithCondition(x => x.id_cargo_regla == entidad.id_cargo_regla);
                if (!existeCargoRegla)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe la relación con cargo-regla."));
            }

            var existeNivel = await new NivelTituloRepository().AnyWithCondition(x => x.id_nivel == entidad.id_nivel);
            if (!existeNivel)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el nivel."));

            var cargoTitulo = await new CargoTituloRepository().AnyWithCondition(x => x.id_cargo_titulo == entidad.id_cargo_titulo);
            if (!cargoTitulo)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe el cargo del titulo"));

            var existeRegla = await new ReglaRepository().AnyWithCondition(x => x.id_regla == entidad.id_regla);
            if (!existeRegla)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe la regla."));

            await ExisteHabilitaciones(entidad.Habilitaciones);

            if (!Update)
            {
                var existeRelacion = await new ReglaCargoRepository().AnyWithCondition(x => x.id_regla == entidad.id_regla && x.id_nivel == entidad.id_nivel
                                && x.id_cargo_titulo == entidad.id_cargo_titulo && x.id_capacidad == entidad.id_capacidad);

                if (existeRelacion)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse($"La relación ya existe no se puede insertar."));
                
            }
        }

        private async Task ExisteHabilitaciones(List<int> habilitacionesId)
        {
            var tasks = habilitacionesId.Select(item => new HabilitacionRepository().AnyWithCondition(x => x.id_habilitacion == item));
            var firstCompletedTask = await Task.WhenAny(tasks);
            var existehabilitacion = await firstCompletedTask;
            if (!existehabilitacion)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No existe la habilitación."));
        }
    }
}
