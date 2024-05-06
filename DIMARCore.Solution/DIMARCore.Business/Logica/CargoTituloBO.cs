using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class CargoTituloBO
    {
        public async Task<IEnumerable<ListadoCargoTituloDTO>> GetAllByFilter(CargoTituloFilter Filtro)
        {
            using (var repo = new CargoTituloRepository())
            {
                return await repo.GetCargosTitulos(Filtro);
            }
        }

        public async Task<Respuesta> GetByIdAsync(int Id)
        {
            var entidad = await new CargoTituloRepository().GetByIdAsync(Id);
            return entidad == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encontro el cargo del titulo."))
                : Responses.SetOkResponse(entidad);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_CARGO_TITULO entidad)
        {
            await ExisteByNombreAsync(entidad);
            await new SeccionBO().ExisteSeccionTituloId(entidad.id_seccion);
            entidad.cargo = entidad.cargo.Trim();
            await new CargoTituloRepository().Create(entidad);
            return Responses.SetCreatedResponse(entidad);
        }



        public async Task<Respuesta> ActualizarAsync(GENTEMAR_CARGO_TITULO entidad)
        {
            await ExisteByNombreAsync(entidad);
            var respuesta = await GetByIdAsync(entidad.id_cargo_titulo);
            var objeto = (GENTEMAR_CARGO_TITULO)respuesta.Data;
            objeto.cargo = entidad.cargo.Trim();
            objeto.id_seccion = entidad.id_seccion;
            objeto.id_clase = entidad.id_clase;
            await new CargoTituloRepository().Update(objeto);
            return Responses.SetUpdatedResponse(objeto);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);
            var entidad = (GENTEMAR_CARGO_TITULO)obj.Data;
            entidad.activo = !entidad.activo;
            await new CargoTituloRepository().Update(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.cargo}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.cargo}";
            }
            return Responses.SetOkResponse(entidad, mensaje);
        }

        public async Task<IEnumerable<CargoTituloInfoDTO>> GetCargosTituloBySeccionId(int seccionId, bool showAll)
        {
            var data = await new CargoTituloRepository().GetCargosTituloBySeccionId(seccionId, showAll);

            if (!data.Any())
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existen cargos para la sección seleccionada.");

            return data;
        }

        public async Task<Respuesta> ExisteCargoTituloById(int cargoId)
        {
            var existeCargoTitulo = await new CargoTituloRepository().ExisteCargoTituloById(cargoId);
            if (!existeCargoTitulo)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"No existe el cargo del título.");
            return Responses.SetOkResponse();
        }



        private async Task ExisteByNombreAsync(GENTEMAR_CARGO_TITULO entidad)
        {
            bool existe;
            if (entidad.id_cargo_titulo == 0)
            {
                existe = await new CargoTituloRepository().AnyWithConditionAsync(x => x.cargo.Equals(entidad.cargo.Trim().ToUpper())
                    && x.id_seccion == entidad.id_seccion && x.id_clase == entidad.id_clase);
            }
            else
            {
                existe = await new CargoTituloRepository().AnyWithConditionAsync(x => x.cargo.Equals(entidad.cargo.Trim().ToUpper())
                    && x.id_seccion == entidad.id_seccion && x.id_clase == entidad.id_clase &&
                x.id_cargo_titulo != entidad.id_cargo_titulo);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el cargo {entidad.cargo} con la sección y clase seleccionada."));
        }

        public async Task<IEnumerable<CargoTituloDTO>> GetCargosActivos()
        {
            return await new CargoTituloRepository().GetCargosActivos();
        }

        public async Task<IEnumerable<CargoTituloDTO>> GetCargoTitulosBySeccionesIds(List<int> seccionesIds)
        {
            return await new CargoTituloRepository().GetCargoTitulosBySeccionesIds(seccionesIds);
        }
    }
}
