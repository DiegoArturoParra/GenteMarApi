using CsvHelper;
using CsvHelper.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Utilities.Helpers
{
    public interface IReadWriteToCSVFile
    {
        Task WriteNewCSV<T>(IEnumerable<T> listado, string filePath, string DelimiterCSV = "|");
        Task<string> WriteNewCSVToBase64<T>(IEnumerable<T> listado, string DelimiterCSV = "|");
    }
    public class ReadWriteToCSVFile : IReadWriteToCSVFile
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ReadWriteToCSVFile()
        {

        }

        /// <summary>
        /// Escribe un nuevo archivo CSV con el listado de objetos que se le envia y lo guarda en la ruta especificada
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listado"></param>
        /// <returns></returns>
        public async Task WriteNewCSV<T>(IEnumerable<T> listado, string directoryPath, string DelimiterCSV = "|")
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Mode = CsvMode.Escape,
                    Delimiter = DelimiterCSV,
                    IgnoreReferences = true,
                };
                var fileName = $"output_data_{DateTime.Now:dd-MM-yyyy_hh-mm-ss}.csv";
                var filePath = Path.Combine(directoryPath, fileName);
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    using (var csvOut = new CsvWriter(writer, config))
                    {
                        await csvOut.WriteRecordsAsync(listado);
                        await writer.FlushAsync();
                    };
                };
            }
            catch (Exception ex)
            {
                _logger.Error("Error al escribir en el archivo CSV.", ex);
                throw;
            }
        }
        public async Task<string> WriteNewCSVToBase64<T>(IEnumerable<T> listado, string DelimiterCSV = "|")
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Mode = CsvMode.Escape,
                    Delimiter = DelimiterCSV,
                    IgnoreReferences = true,
                };

                // Crear un MemoryStream para almacenar los datos del archivo CSV
                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                    {
                        using (var csvOut = new CsvWriter(writer, config))
                        {
                            await csvOut.WriteRecordsAsync(listado);
                            await writer.FlushAsync();

                            // Obtener el array de bytes del MemoryStream
                            byte[] csvBytes = memoryStream.ToArray();

                            // Convertir los bytes a formato base64
                            string base64String = Convert.ToBase64String(csvBytes);

                            return base64String;

                        };
                    };
                };
            }
            catch (Exception ex)
            {
                _logger.Error("Error al escribir en el archivo CSV.", ex);
                throw;
            }
        }

    }
}
