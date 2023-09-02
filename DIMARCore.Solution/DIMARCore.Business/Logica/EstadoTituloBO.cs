using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business.Logica
{
    public class EstadoTituloBO : IGenericCRUD<GENTEMAR_ESTADO_TITULO, int>
    {
        public async Task<Respuesta> ActualizarAsync(GENTEMAR_ESTADO_TITULO entidad)
        {
            await ExisteByNombreAsync(entidad.descripcion_tramite.Trim(), entidad.id_estado_tramite);

            var respuesta = await GetByIdAsync(entidad.id_estado_tramite);

            var obj = (GENTEMAR_ESTADO_TITULO)respuesta.Data;
            obj.descripcion_tramite = entidad.descripcion_tramite;
            await new EstadoTituloRepository().Update(obj);

            return Responses.SetUpdatedResponse(obj);
        }

        public async Task<Respuesta> AnulaOrActivaAsync(int Id)
        {
            string mensaje;
            var obj = await GetByIdAsync(Id);

            var entidad = (GENTEMAR_ESTADO_ANTECEDENTE)obj.Data;
            entidad.activo = !entidad.activo;
            await new EstadoEstupefacienteRepository().Update(entidad);
            if (entidad.activo)
            {
                mensaje = $"Se activo {entidad.descripcion_estado_antecedente}";
            }
            else
            {
                mensaje = $"Se anulo {entidad.descripcion_estado_antecedente}";
            }

            return Responses.SetOkResponse(entidad, mensaje);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_ESTADO_TITULO entidad)
        {
            await ExisteByNombreAsync(entidad.descripcion_tramite.Trim().ToUpper());
            entidad.descripcion_tramite = entidad.descripcion_tramite.Trim();
            await new EstadoTituloRepository().Create(entidad);
            return Responses.SetCreatedResponse(entidad);
        }

        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;

            if (Id == 0)
            {
                existe = await new EstadoTituloRepository().AnyWithCondition(x => x.descripcion_tramite.Equals(nombre));
            }
            else
            {
                existe = await new EstadoTituloRepository().AnyWithCondition(x => x.descripcion_tramite.Equals(nombre) && x.id_estado_tramite != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el estado {nombre}"));
        }

        public IEnumerable<GENTEMAR_ESTADO_TITULO> GetAll(bool? activo = true)
        {
            using (var repo = new EstadoTituloRepository())
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
            var entidad = await new EstadoTituloRepository().GetById(Id);
            if (entidad == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encuentra el estado indicado."));

            return Responses.SetOkResponse(entidad);
        }
    }
}
