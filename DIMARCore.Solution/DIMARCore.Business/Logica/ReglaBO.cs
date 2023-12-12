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
    public class ReglaBO : IGenericCRUD<GENTEMAR_REGLAS, int>
    {
        public IEnumerable<GENTEMAR_REGLAS> GetAll(bool? activo = true)
        {
            using (var repo = new ReglaRepository())
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
            var regla = await new ReglaRepository().GetById(Id);

            return regla == null
                ? throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No existe la regla solicitada."))
                : Responses.SetOkResponse(regla);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_REGLAS entidad)
        {
            await ExisteByNombreAsync(entidad.nombre_regla.Trim().ToUpper());

            entidad.nombre_regla = entidad.nombre_regla.Trim().ToUpper();
            await new ReglaRepository().Create(entidad);

            return Responses.SetCreatedResponse(entidad);
        }

        public async Task<Respuesta> ActualizarAsync(GENTEMAR_REGLAS entidad)
        {
            await ExisteByNombreAsync(entidad.nombre_regla.Trim().ToUpper(), entidad.id_regla);

            var respuesta = await GetByIdAsync(entidad.id_regla);

            var objeto = (GENTEMAR_REGLAS)respuesta.Data;
            objeto.nombre_regla = entidad.nombre_regla.Trim().ToUpper();

            await new ReglaRepository().Actualizar(objeto);

            return Responses.SetUpdatedResponse(objeto);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);
            var entidad = (GENTEMAR_REGLAS)obj.Data;
            entidad.activo = !entidad.activo;
            await new ReglaRepository().Actualizar(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.nombre_regla}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.nombre_regla}";
            }
            return Responses.SetOkResponse(entidad, mensaje);
        }


        public async Task<IEnumerable<ReglaDTO>> GetReglasActivasByCargoTitulo(int cargoId)
        {
            return await new ReglaRepository().GetReglasActivasByCargoTitulo(cargoId);
        }


        public async Task<Respuesta> ExisteReglaById(int reglaId)
        {
            var existe = await new ReglaRepository().ExisteRegla(reglaId);
            if (!existe)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse("No se encuentra registrada la regla."));
            return Responses.SetOkResponse();
        }

        public async Task<Respuesta> Validaciones(int cargoId)
        {
            await new CargoTituloBO().ExisteCargoTituloById(cargoId);
            return await new ReglaCargoBO().ExisteCargoTituloInDetalleRegla(cargoId);
        }

        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;
            if (Id == 0)
            {
                existe = await new ReglaRepository().AnyWithCondition(x => x.nombre_regla.Equals(nombre));
            }
            else
            {
                existe = await new ReglaRepository().AnyWithCondition(x => x.nombre_regla.Equals(nombre) && x.id_regla != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrada la clase {nombre}"));
        }
    }
}
