using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class EstupefacienteBO
    {
        public IQueryable<ListadoEstupefacientesDTO> GetEstupefacientesByFiltro(EstupefacientesFilter filtro)
        {
            return new EstupefacienteRepository().GetAntecedentesByFiltro(filtro);
        }


        public async Task<Respuesta> CrearAsync(GENTEMAR_ANTECEDENTES entidad, GENTEMAR_ANTECEDENTES_DATOSBASICOS datosBasicos)
        {
            using (var repo = new EstupefacienteRepository())
            {
                var existeRadicado = await repo.AnyWithCondition(x => x.numero_sgdea.Equals(entidad.numero_sgdea));
                if (existeRadicado)
                    throw new HttpStatusCodeException(Responses.SetConflictResponse("Ya se encuentra registrado el radicado."));

                var existeRadicadoSGDEA = await new SGDEARepository().FechaRadicado(entidad.numero_sgdea);
                if (existeRadicadoSGDEA.Radicado == null)
                    throw new HttpStatusCodeException(Responses.SetNotFoundResponse("El radicado no se encuentra registrado en el SGDEA."));

                if (entidad.fecha_sgdea == null)
                {
                    entidad.fecha_sgdea = existeRadicadoSGDEA.Fecha;
                }

                datosBasicos.identificacion = Reutilizables.ConvertirStringApuntosDeMil(datosBasicos.identificacion);
                long idEstupefaciente = await repo.GetIdGenteMarAntecedente(datosBasicos.identificacion);
                if (idEstupefaciente > 0)
                {
                    entidad.id_gentemar_antecedente = idEstupefaciente;
                    entidad.fecha_solicitud_sede_central = DateTime.Now;
                    await repo.Create(entidad);
                }
                else
                {
                    entidad.fecha_solicitud_sede_central = DateTime.Now;
                    await repo.CreateWithGenteMar(entidad, datosBasicos);
                }
            }
            return Responses.SetCreatedResponse(entidad);
        }

        public async Task<Respuesta> EditarAsync(GENTEMAR_ANTECEDENTES entidad)
        {

            using (var repo = new EstupefacienteRepository())
            {
                await repo.Update(entidad);
            }
            return Responses.SetUpdatedResponse(entidad);
        }

        public async Task<IEnumerable<string>> GetRadicadosLicenciasByDocumento(string identificacion)
        {
            return await new EstupefacienteRepository().GetRadicadosLicenciasByDocumento(identificacion);
        }
        public async Task<IEnumerable<string>> GetRadicadosTitulosByDocumento(string identificacion)
        {
            return await new EstupefacienteRepository().GetRadicadosTitulosByDocumento(identificacion);
        }

        public async Task<Respuesta> GetDatosGenteMarEstupefaciente(string identificacionConPuntos)
        {
            var data = await new EstupefacienteRepository().GetDatosGenteMarEstupefaciente(identificacionConPuntos);
            if (data == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No se encuentra registrada la persona.");
            data.ExisteEnDatosBasicosEstupefaciente = true;
            return Responses.SetOkResponse(data);
        }

        public async Task<Respuesta> GetDetallePersonaEstupefaciente(long id)
        {
            var data = await new EstupefacienteRepository().GetDetallePersonaEstupefaciente(id);
            if (data == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el registro del estupefaciente.");
            return Responses.SetOkResponse(data);
        }
    }
}
