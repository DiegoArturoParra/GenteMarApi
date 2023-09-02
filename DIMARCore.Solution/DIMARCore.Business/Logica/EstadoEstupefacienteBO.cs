using DIMARCore.Business.Interfaces;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIMARCore.Utilities.Middleware;
namespace DIMARCore.Business.Logica
{
    public class EstadoEstupefacienteBO : IGenericCRUD<GENTEMAR_ESTADO_ANTECEDENTE, int>
    {
        public IEnumerable<GENTEMAR_ESTADO_ANTECEDENTE> GetAll(bool? activo = true)
        {
            using (var repo = new EstadoEstupefacienteRepository())
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

            var entidad = await new EstadoEstupefacienteRepository().GetById(Id);
            if (entidad == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No se encuentra el estado indicado."));

            return Responses.SetOkResponse(entidad);
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_ESTADO_ANTECEDENTE entidad)
        {
            await ExisteByNombreAsync(entidad.descripcion_estado_antecedente.Trim().ToUpper());
            entidad.descripcion_estado_antecedente = entidad.descripcion_estado_antecedente.Trim();
            await new EstadoEstupefacienteRepository().Create(entidad);
            return Responses.SetCreatedResponse(entidad);
        }


        public async Task<Respuesta> ActualizarAsync(GENTEMAR_ESTADO_ANTECEDENTE objeto)
        {

            await ExisteByNombreAsync(objeto.descripcion_estado_antecedente.Trim(), objeto.id_estado_antecedente);

            var respuesta = await GetByIdAsync(objeto.id_estado_antecedente);

            var obj = (GENTEMAR_ESTADO_ANTECEDENTE)respuesta.Data;
            obj.descripcion_estado_antecedente = objeto.descripcion_estado_antecedente;
            await new EstadoEstupefacienteRepository().Update(obj);

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

       
        public async Task ExisteByNombreAsync(string nombre, int Id = 0)
        {
            bool existe;

            if (Id == 0)
            {
                existe = await new EstadoEstupefacienteRepository().AnyWithCondition(x => x.descripcion_estado_antecedente.Equals(nombre));
            }
            else
            {
                existe = await new EstadoEstupefacienteRepository().AnyWithCondition(x => x.descripcion_estado_antecedente.Equals(nombre) && x.id_estado_antecedente != Id);
            }
            if (existe)
                throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el estado {nombre}"));
        }   
    }
}
