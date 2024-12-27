using ClosedXML.Excel;
using DIMARCore.Business.Helpers;
using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class ConsolidadoBO
    {
        private readonly int _numeroDeLotes;
        public ConsolidadoBO()
        {
            _numeroDeLotes = int.Parse(ConfigurationManager.AppSettings[Constantes.NUMERO_DE_LOTES]);
        }
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async Task<IEnumerable<GENTEMAR_CONSOLIDADO>> GetConsolidados()
        {
            return await new ConsolidadoEstupefacienteRepository().GetAllAsync();
        }

        public async Task<Respuesta> GenerarExcelConConsolidadoDeEstupefacientes(string email, CrearConsolidadoExcelDTO consolidadoDTO)
        {
            string numeroConsolidado = string.Empty;
            int totalEstupefacientesEstadoPorEnviar = 0;
            List<EstupefacientesExcelDTO> dataExcel = new List<EstupefacientesExcelDTO>();
            using (var repository = new EstupefacienteRepository())
            {
                using (var ConsolidadoRepository = new ConsolidadoEstupefacienteRepository())
                {
                    bool repiteConsolidado = consolidadoDTO.IsNew ?
                        await ConsolidadoRepository.AnyWithConditionAsync(y => y.numero_consolidado == consolidadoDTO.Consolidado) : false;

                    if (repiteConsolidado)
                        throw new HttpStatusCodeException(Responses.SetConflictResponse($"Ya se encuentra registrado el número de consolidado {consolidadoDTO.Consolidado}"));

                    dataExcel = await repository.GetEstupefacientesEstadoInicial();
                    totalEstupefacientesEstadoPorEnviar = dataExcel.Count();
                    if (!dataExcel.Any())
                        throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No hay estupefacientes en estado {EnumConfig.GetDescription(EstadoEstupefacienteEnum.ParaEnviar)} para exportar el Excel."));

                    IEnumerable<long> EstupefacientesIds = dataExcel.Select(x => x.EstupefacienteId).ToList();
                    var datosAEditar = await repository.GetAllWithConditionAsync(x => EstupefacientesIds.Contains(x.id_antecedente));
                    datosAEditar = datosAEditar.Select(entidad =>
                    {
                        // Realizar los cambios necesarios en cada objeto de la lista
                        entidad.id_estado_antecedente = (int)EstadoEstupefacienteEnum.Consulta;
                        return entidad;
                    }).ToList();
                    if (consolidadoDTO.IsNew)
                    {
                        consolidadoDTO.Consolidado = $"{Constantes.SIGLA_CONSOLIDADO}{consolidadoDTO.Consolidado.Trim()}";
                        numeroConsolidado = consolidadoDTO.Consolidado;
                        await repository.EditBulkWithconsolidated(datosAEditar.ToList(), _numeroDeLotes, consolidadoDTO.Consolidado, consolidadoDTO.ArrayExpedientesEntidad, true);
                    }
                    else
                    {
                        int consolidadoId = int.Parse(consolidadoDTO.Consolidado);
                        var consolidado = await ConsolidadoRepository.GetByIdAsync(consolidadoId)
                       ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No existe el consolidado.");
                        numeroConsolidado = consolidado.numero_consolidado;
                        var dataEnConsultaPorConsolidado = await ConsolidadoRepository.GetEstupefacientesEnConsultaPorConsolidado(consolidado.id_consolidado);
                        dataExcel.AddRange(dataEnConsultaPorConsolidado);
                        await repository.EditBulkWithconsolidated(datosAEditar.ToList(), _numeroDeLotes, consolidado.id_consolidado.ToString(), consolidadoDTO.ArrayExpedientesEntidad, false);
                    }
                }
            }
            var archivoBytes = GenerateExcel(dataExcel, numeroConsolidado);
            var archivoBase64 = Convert.ToBase64String(archivoBytes);


            String[] correos = { email };
            SendEmailRequest request = new SendEmailRequest
            {
                CorreosDestino = correos,
                Asunto = $"Se generó consolidado {numeroConsolidado} excel para enviar el día: {DateTime.Now:dd/MM/yyyy hh: mm:ss tt}",
                CuerpoDelMensaje = $"Se actualizaron {totalEstupefacientesEstadoPorEnviar} estupefacientes a estado EN CONSULTA para el consolidado con número: {numeroConsolidado}",
                Footer = Constantes.FOOTER_EMAIL
            };

            await new EnvioNotificacionesHelper().SendNotificationByEmail(request);

            return Responses.SetOkResponse(new ArchivoExcelDTO { ArchivoBase64 = archivoBase64, Extension = Constantes.EXTENSION_EXCEL });
        }

        #region Generar reporte en excel
        public byte[] GenerateExcel(IList<EstupefacientesExcelDTO> datosEstupefacientes, string numeroDeConsolidado)
        {

            using (var workbook = new XLWorkbook())
            {
                try
                {
                    var worksheet = workbook.Worksheets.Add($"consolidado_numero_{numeroDeConsolidado}");
                    var currentRow = 1;
                    // Create a style for the header cells
                    var headerStyle = worksheet.Style;
                    headerStyle.Font.Bold = true;
                    // Cabeceros
                    worksheet.Cell(currentRow, 1).Value = ExcelEstupefacienteConfig.ITEM;
                    worksheet.Cell(currentRow, 2).Value = ExcelEstupefacienteConfig.NUMERO_SGDEA;
                    worksheet.Cell(currentRow, 3).Value = ExcelEstupefacienteConfig.FECHA_SGDEA;
                    worksheet.Cell(currentRow, 4).Value = ExcelEstupefacienteConfig.LUGAR_TRAMITE;
                    worksheet.Cell(currentRow, 5).Value = ExcelEstupefacienteConfig.TIPO_TRAMITE;
                    worksheet.Cell(currentRow, 6).Value = ExcelEstupefacienteConfig.TIPO_DOCUMENTO;
                    worksheet.Cell(currentRow, 7).Value = ExcelEstupefacienteConfig.DOCUMENTO;
                    worksheet.Cell(currentRow, 8).Value = ExcelEstupefacienteConfig.NOMBRES;
                    worksheet.Cell(currentRow, 9).Value = ExcelEstupefacienteConfig.APELLIDOS;
                    worksheet.Cell(currentRow, 10).Value = ExcelEstupefacienteConfig.FECHA_NACIMIENTO;
                    worksheet.Cell(currentRow, 11).Value = ExcelEstupefacienteConfig.FECHA_SECE;
                    // Apply the header style to the entire row
                    worksheet.Row(currentRow).Style = headerStyle;
                    currentRow++;
                    // Data
                    foreach (var item in datosEstupefacientes)
                    {
                        worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                        worksheet.Cell(currentRow, 2).Style.NumberFormat.Format = "@";
                        worksheet.Cell(currentRow, 2).Value = item.Radicado;
                        worksheet.Cell(currentRow, 3).Value = item.FechaRadicadoFormato;
                        worksheet.Cell(currentRow, 4).Value = item.LugarTramiteCapitania;
                        worksheet.Cell(currentRow, 5).Value = item.TipoTramite;
                        worksheet.Cell(currentRow, 6).Value = item.TipoDocumento;
                        worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "@";
                        worksheet.Cell(currentRow, 7).Value = item.Documento;
                        worksheet.Cell(currentRow, 8).Value = item.Nombres;
                        worksheet.Cell(currentRow, 9).Value = item.Apellidos;
                        worksheet.Cell(currentRow, 10).Value = item.FechaNacimientoFormato;
                        worksheet.Cell(currentRow, 11).Value = item.FechaSolicitudSedeCentralFormato;
                        currentRow++;
                    }
                    // Create a table using the entire range of data
                    var dataRange = worksheet.Range(1, 1, currentRow - 1, 11);
                    var table = dataRange.CreateTable();
                    // Set the table style
                    worksheet.ColumnsUsed().AdjustToContents();
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return content;
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, $"Error: No se pudo generar el Excel, {ex.Message}");
                }
            }
        }
        #endregion

        public async Task<Respuesta> GetConsolidadosEnUso()
        {
            var data = await new ConsolidadoEstupefacienteRepository().GetConsolidadosEnUso();

            if (!data.Any())
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"No hay consolidados en uso actualmente.");

            return Responses.SetOkResponse(data);
        }

        public async Task<Respuesta> GetAllIdsEstupefacienteByConsolidado(int consolidadoId)
        {
            var data = await new ConsolidadoEstupefacienteRepository().GetAllIdsEstupefacienteByConsolidado(consolidadoId);
            return Responses.SetOkResponse(data);
        }

        public async Task<Respuesta> GetConsolidadoNext()
        {
            var lastConsolidado = await new ConsolidadoEstupefacienteRepository().GetLastConsolidado();
            if (lastConsolidado == null)
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"No hay consolidados registrados."));

            var nextConsolidado = Reutilizables.ObtenerNumeroDesdeCadena(lastConsolidado);
            if (nextConsolidado != -1)
            {
                nextConsolidado++;
                return Responses.SetOkResponse(new ConsolidadoDTO
                {
                    Descripcion = nextConsolidado.ToString(),
                });
            }
            return Responses.SetOkResponse();
        }
    }
}
