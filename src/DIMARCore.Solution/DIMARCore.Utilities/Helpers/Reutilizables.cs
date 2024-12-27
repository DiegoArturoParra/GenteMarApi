using DIMARCore.Utilities.Middleware;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Layout.Font;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DIMARCore.Utilities.Helpers
{
    public class Archivo
    {
        public string PathArchivo { get; set; }
        public string NombreArchivo { get; set; }

    }
    /// <summary>
    /// Clase que tiene metodos reutilizables en cualquier libreria.
    /// </summary>
    public static class Reutilizables
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static T CleanSpaces<T>(T obj) where T : class
        {
            // Obtener todas las propiedades públicas del objeto
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Verificar si la propiedad es de tipo string
                if (property.PropertyType == typeof(string))
                {
                    // Obtener el valor actual de la propiedad
                    var value = (string)property.GetValue(obj);

                    // Limpiar espacios en blanco al principio y al final
                    var cleanedValue = value?.Trim();

                    // Establecer el nuevo valor en la propiedad
                    property.SetValue(obj, cleanedValue);
                }
            }
            return obj;
        }

        public static List<string> GetDelimitedList(string input, char delimiter)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<string>();
            }

            string[] array = input.Split(delimiter);
            return new List<string>(array);
        }

        public static string ConvertirStringApuntosDeMil(object identificacion)
        {
            string documento = string.Empty;
            if (identificacion != null)
            {
                documento = Convert.ToString(identificacion);

                if (!string.IsNullOrWhiteSpace(documento))
                {
                    if (documento.Contains("."))
                    {
                        return documento;
                    }

                    var replace = Regex.Replace(documento, Constantes.REGEXPUNTOSMIL, Constantes.SEPARADORDOCUMENTO);
                    return replace;

                }
            }
            return documento;
        }


        /// <summary>
        /// Metodo que obtiene solo los numeros de una cadena
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static string ObtenerSoloNumeros(string cadena)
        {
            StringBuilder soloNumerosBuilder = new StringBuilder();

            foreach (char caracter in cadena)
            {
                if (char.IsDigit(caracter))
                {
                    soloNumerosBuilder.Append(caracter);
                }
            }

            return soloNumerosBuilder.ToString();
        }
        /// <summary>
        /// Metodo que obtiene el numero de una cadena en entero
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static int ObtenerNumeroDesdeCadena(string cadena)
        {
            string soloNumeros = ObtenerSoloNumeros(cadena);

            if (!string.IsNullOrEmpty(soloNumeros))
            {
                if (int.TryParse(soloNumeros, out int numero))
                {
                    return numero;
                }
            }

            return -1; // Valor predeterminado si no se puede convertir la cadena en un número.
        }


        /// <summary>
        /// Descarga el archivo del repositorio carpeta alojada en el servidor
        /// </summary>
        /// <param name="rutaNombreArchivo"></param>
        /// <param name="archivoBase64"></param>
        /// <returns></returns>
        public static Respuesta DescargarArchivo(string rutaNombreArchivo, out string archivoBase64)
        {
            archivoBase64 = null;
            if (string.IsNullOrEmpty(rutaNombreArchivo))
            {
                _logger.Error($"Error, la ruta del archivo no es valida. Por favor contacte al administrador del sistema.");
                return Responses.SetConflictResponse("Error, la ruta del archivo no es valida. Por favor contacte al administrador del sistema.");
            }
            try
            {
                var archivoBytes = File.ReadAllBytes(rutaNombreArchivo);
                archivoBase64 = Convert.ToBase64String(archivoBytes);
                _logger.Info($"Se descarga el archivo {rutaNombreArchivo}");
                return Responses.SetOkResponse(archivoBase64);
            }
            catch (Exception ex)
            {
                return Responses.SetInternalServerErrorResponse(ex, "Error al buscar el archivo. Por favor contacte al administrador del sistema.");
            }
        }

        public static string DescargarArchivoServidor(string rutaNombreArchivo)
        {
            try
            {
                // Acceder al archivo usando FileStream
                using (FileStream fileStream = new FileStream(rutaNombreArchivo, FileMode.Open, FileAccess.Read))
                {
                    // Leer el contenido del archivo
                    byte[] fileBytes = new byte[fileStream.Length];
                    fileStream.Read(fileBytes, 0, (int)fileStream.Length);

                    // Hacer algo con los datos del archivo, como convertirlo a Base64
                    return Convert.ToBase64String(fileBytes);
                }
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(Responses.SetNotFoundResponse($"El archivo no se encuentra en el repositorio {ex}"));
            }
        }

        public static Respuesta GuardarArchivoDeBytes(byte[] archivoBytes, string rutaInicial, string rutaModuloTipoDocumento, string nombreArchivo = "")
        {

            rutaInicial = string.IsNullOrEmpty(rutaInicial) ? rutaInicial.Trim() : rutaInicial;
            nombreArchivo = string.IsNullOrEmpty(nombreArchivo) ? nombreArchivo.Trim() : nombreArchivo;

            if (string.IsNullOrEmpty(rutaInicial))
                throw new HttpStatusCodeException(Responses.SetConflictResponse("Error, la ruta no es valida. Por favor contacte al administrador del sistema."));
            // valida existencia ruta inicial
            bool rutaInicialExiste = Directory.Exists(rutaInicial);

            if (!rutaInicialExiste)
                throw new HttpStatusCodeException(Responses.SetConflictResponse("Error. No se encuentra la ruta principal. Por favor contacte al administrador del sistema."));

            // arma la ruta
            var rutaCompletaModuloTipoDocumento = $@"{rutaInicial}\{rutaModuloTipoDocumento}";
            bool rutaModuloExiste = Directory.Exists(rutaCompletaModuloTipoDocumento);
            if (!rutaModuloExiste)
            {
                Directory.CreateDirectory(rutaCompletaModuloTipoDocumento);
            }
            if (string.IsNullOrEmpty(nombreArchivo))
            {
                nombreArchivo = Guid.NewGuid().ToString();
                nombreArchivo = $"{nombreArchivo}{Path.GetExtension(nombreArchivo)}";
            }
            // arma la ruta del archivo
            var rutaNombreArchivo = $@"{rutaCompletaModuloTipoDocumento}\{nombreArchivo}";
            File.WriteAllBytes(rutaNombreArchivo, archivoBytes);
            return Responses.SetOkResponse(new Archivo
            {
                NombreArchivo = nombreArchivo,
                PathArchivo = $@"{rutaModuloTipoDocumento}\{nombreArchivo}",
            });
        }

        /// <summary>
        /// guardar un archivo en el servidor
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rutaInicial"></param>
        /// <param name="rutaModuloTipoDocumento"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public static Respuesta GuardarArchivo(HttpPostedFile file, string rutaInicial, string rutaModuloTipoDocumento, string nombreArchivo = "")
        {
            Respuesta respuesta = null;
            rutaInicial = string.IsNullOrEmpty(rutaInicial) ? rutaInicial.Trim() : rutaInicial;
            nombreArchivo = string.IsNullOrEmpty(nombreArchivo) ? nombreArchivo.Trim() : nombreArchivo;

            if (string.IsNullOrEmpty(rutaInicial))
                throw new HttpStatusCodeException(Responses.SetConflictResponse("Error, la ruta no es valida. Por favor contacte al administrador del sistema."));

            // valida existencia ruta inicial
            bool rutaInicialExiste = Directory.Exists(rutaInicial);

            if (!rutaInicialExiste)
                throw new HttpStatusCodeException(Responses.SetConflictResponse("Error. No se encuentra la ruta principal. Por favor contacte al administrador del sistema."));
            try
            {
                // arma la ruta
                var rutaCompletaModuloTipoDocumento = $@"{rutaInicial}\{rutaModuloTipoDocumento}";
                bool rutaModuloExiste = Directory.Exists(rutaCompletaModuloTipoDocumento);
                if (!rutaModuloExiste)
                {
                    Directory.CreateDirectory(rutaCompletaModuloTipoDocumento);
                }

                if (string.IsNullOrEmpty(nombreArchivo))
                {
                    nombreArchivo = Guid.NewGuid().ToString();
                    nombreArchivo = $"{nombreArchivo}{Path.GetExtension(file.FileName)}";
                }

                // arma la ruta del archivo
                var rutaNombreArchivo = $@"{rutaCompletaModuloTipoDocumento}\{nombreArchivo}";

                file.SaveAs(rutaNombreArchivo);

                respuesta = Responses.SetOkResponse(new Archivo()
                {
                    PathArchivo = $@"{rutaModuloTipoDocumento}\{nombreArchivo}",
                    NombreArchivo = nombreArchivo
                });
                _logger.Info($"Se crea el archivo {nombreArchivo}");

            }
            catch (Exception ex)
            {
                respuesta = Responses.SetInternalServerErrorResponse(ex, "Error al guardar el archivo.");
            }
            return respuesta;
        }

        /// <summary>
        /// Eliminar un archivo del directorio de archivos
        /// </summary>
        /// <param name="rutaInicial">Ruta inicial directorio de archivos</param>
        /// <param name="rutaArchivo">Ruta interna (Nombre modulo/Tipo documento/Nombre Archivo)</param>
        /// <returns></returns>
        public static Respuesta EliminarArchivo(string rutaInicial, string rutaArchivo)
        {
            Respuesta respuesta = null;
            try
            {
                rutaInicial = string.IsNullOrEmpty(rutaInicial) ? rutaInicial.Trim() : rutaInicial;
                rutaArchivo = string.IsNullOrEmpty(rutaArchivo) ? rutaArchivo.Trim() : rutaArchivo;

                if (string.IsNullOrEmpty(rutaInicial))
                {
                    respuesta = Responses.SetConflictResponse("Error, la ruta no es valida. Por favor contacte al administrador del sistema.");
                }

                if (string.IsNullOrEmpty(rutaArchivo))
                {
                    respuesta = Responses.SetConflictResponse("Error, la ruta no es valida. Por favor contacte al administrador del sistema.");
                }

                // valida existencia ruta inicial
                bool rutaInicialExiste = Directory.Exists(rutaInicial);
                if (!rutaInicialExiste)
                {
                    respuesta = Responses.SetConflictResponse($"Error. No se encuentra la ruta principal. Por favor contacte al administrador del sistema.");
                    _logger.Error($"Error al eliminar el archivo. No se encuentra la ruta {rutaInicial}");
                }

                // arma la ruta completa
                var rutaCompletaArchivo = $@"{rutaInicial}\{rutaArchivo}";
                bool rutaCompletaArchivoExiste = File.Exists(rutaCompletaArchivo);
                if (!rutaCompletaArchivoExiste)
                {
                    _logger.Error($"Error al eliminar el archivo. No se pudo eliminar el archivo de la ruta {rutaCompletaArchivo}. Ruta o archivo no encontrado.");
                    respuesta = Responses.SetConflictResponse($"Error al eliminar el archivo. No se pudo eliminar el archivo de la ruta {rutaCompletaArchivo}. Ruta o archivo no encontrado.");
                }

                // se elimina
                File.Delete(rutaCompletaArchivo);
                _logger.Info($"Se elimna el archivo {rutaCompletaArchivo}");
                respuesta = Responses.SetOkResponse(null, "Se ha eliminado correctamente el archivo.");
            }
            catch (Exception ex)
            {
                Responses.SetInternalServerErrorResponse(ex, "Error al eliminar el archivo. Por favor contacte al administrador del sistema.");
            }
            return respuesta;
        }
        /// <summary>
        /// Describe el documento para la tabla repositorio archivos
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string DescribirDocumento(string extension)
        {
            string documento = string.Empty;
            string[] images = { ".png", ".jpg", ".jpeg" };
            string[] archivos = { ".pdf", ".doc", ".docx", ".xlsx" };

            if (archivos.Contains(extension.ToLower()))
            {
                documento = $"El archivo es de tipo documento con extensión: {extension.ToLower()}";
            }
            else if (images.Contains(extension.ToLower()))
            {
                documento = $"El archivo es de tipo Imagen con extensión: {extension.ToLower()}";
            }
            return documento;
        }
        /// <summary>
        /// generar pdf en base64 a partir de un html 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GeneratePdftoBase64(string html)
        {
            try
            {
                // Generar el archivo PDF desde la plantilla HTML
                MemoryStream outputMemoryStream = new MemoryStream();
                FontProvider fontProvider = new DefaultFontProvider(false, false, true);
                fontProvider.AddFont($"{AppDomain.CurrentDomain.BaseDirectory}Templates/Fonts/arial.ttdddf");
                fontProvider.AddFont($"{AppDomain.CurrentDomain.BaseDirectory}Templates/Fonts/arial_bold.ttdddf");
                // Step 3: Set up conversion properties
                ConverterProperties converterProperties = new ConverterProperties();
                converterProperties.SetFontProvider(fontProvider);

                HtmlConverter.ConvertToPdf(html, outputMemoryStream, converterProperties);

                byte[] outputBytes = outputMemoryStream.ToArray();
                string base64String = Convert.ToBase64String(outputBytes);

                return base64String;
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al generar la prevista PDF"));
            }
        }

        /// <summary>
        /// Une los pdf que se le pasen en el array  y devuelve el base 64 
        /// </summary>
        /// <param name="base64PdfList"></param>
        /// <returns></returns>
        public static string CombinePdfListInBase64(String[] base64PdfList)
        {
            using (MemoryStream outputMemoryStream = new MemoryStream())
            {
                // Abrir el documento PDF combinado
                Document document = new Document();
                PdfCopy pdfCopy = new PdfCopy(document, outputMemoryStream);
                document.Open();

                foreach (string base64Pdf in base64PdfList)
                {
                    // Convertir el base64 en bytes
                    byte[] pdfBytes = Convert.FromBase64String(base64Pdf);

                    // Abrir el documento PDF desde los bytes
                    PdfReader pdfReader = new PdfReader(pdfBytes);
                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, page));
                    }

                    // Cerrar el lector
                    pdfReader.Close();
                }

                // Cerrar el documento
                document.Close();

                // Convertir el PDF combinado a base64
                return Convert.ToBase64String(outputMemoryStream.ToArray());
            }
        }

        public static byte[] GenerateBase64toBytes(string base64String)
        {
            try
            {
                // Decodifica la cadena Base64 en un arreglo de bytes
                byte[] pdfBytes = Convert.FromBase64String(base64String);

                // Crear un MemoryStream y escribir los bytes decodificados en él
                using (MemoryStream memoryStream = new MemoryStream(pdfBytes))
                {
                    return memoryStream.ToArray();
                }

                // El archivo en memoria se liberará automáticamente al salir del bloque using
            }
            catch (Exception ex)
            {
                throw new HttpStatusCodeException(Responses.SetInternalServerErrorResponse(ex, "Error al convertir Base64 a PDF"));
            }
        }
        /// <summary>
        /// leer un archivo en el repocitorio local 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var buffer = new byte[4096]; // Tamaño del búfer de lectura
                    var stringBuilder = new StringBuilder();

                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        var chunk = new string(Encoding.UTF8.GetChars(buffer, 0, bytesRead));
                        stringBuilder.Append(chunk);
                    }

                    return stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// replazar las variables en una plantilla html 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ReplaceVariables(string template, object data, string htmlLabelReplace)
        {
            Type type = data.GetType();
            foreach (var property in type.GetProperties())
            {
                string variable = "{" + property.Name + "}";
                string value = property.GetValue(data)?.ToString() ?? "";
                // convierte las fechas a un formato dd/MM/yyyy
                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime dateTimeValue = (DateTime)property.GetValue(data);
                    value = dateTimeValue.ToString("dd/MM/yyyy");
                }
                // obtiene los valores de la lista de strings
                if (property.PropertyType == typeof(List<string>))
                {
                    List<string> listValue = (List<string>)property.GetValue(data);
                    if (listValue != null && listValue.Count > 0)
                    {
                        if (variable == Constantes.VARIABLE_LIMITANTES && listValue.Contains(Constantes.VARIABLE_NINGUNA))
                        {
                            value = "";
                        }
                        else
                        {
                            value = string.Join(htmlLabelReplace, listValue);
                        }
                    }
                }
                template = template.Replace(variable, value);
            }
            return template;
        }


        /// <summary>
        /// Construit Tabla en HTMl con una lista de objetos generico 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string TablaHtmlVariables(object data)
        {
            var tablaHtml = new StringBuilder();

            // Verifica si el objeto es una lista
            if (data is IList lista)
            {
                // Verifica si la lista tiene elementos
                if (lista.Count > 0)
                {
                    var itemType = lista[0].GetType();
                    var properties = itemType.GetProperties();
                    // Recorre los elementos de la lista y crea las filas de datos
                    foreach (var item in lista)
                    {
                        tablaHtml.AppendLine("<tr>");
                        string habilitacion = "";
                        foreach (var property in properties)
                        {
                            var propertyValue = property.GetValue(item);
                            if (propertyValue is IList subLista)
                            {
                                habilitacion = string.Join(" - ", subLista.Cast<object>());
                            }
                            else
                            {
                                if (property.Name is "Capacidad")
                                {
                                    tablaHtml.AppendLine($"<td>{propertyValue} - {habilitacion}</td>");
                                }
                                else
                                {
                                    tablaHtml.AppendLine($"<td>{propertyValue}</td>");
                                }

                            }

                        }
                        tablaHtml.AppendLine("</tr>");
                    }
                }
            }

            return tablaHtml.ToString();
        }
        /// <summary>
        /// Calcula la edad en base a la fecha de nacimiento y la fecha actual 
        /// </summary>
        /// <param name="fechaNacimiento"></param>
        /// <param name="fechaActual"></param>
        /// <returns></returns>
        public static short CalcularEdad(DateTime fechaNacimiento, DateTime? fechaActual = null)
        {
            if (!fechaActual.HasValue)
            {
                fechaActual = DateTime.Now;
            }

            short edad = (short)(fechaActual.Value.Year - fechaNacimiento.Year);

            // Verificar si el cumpleaños ya ocurrió este año
            if (fechaNacimiento > fechaActual.Value.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }

        public static (DateTime DateInitial, DateTime DateEnd) FormatDatesByRange(DateTime dateInitial, DateTime dateEnd)
        {
            dateInitial = new DateTime(dateInitial.Year, dateInitial.Month, dateInitial.Day, 0, 0, 0, 0);
            dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 0, 0, 0, 0);
            dateEnd = dateEnd.AddHours(24).AddSeconds(-1);
            return (dateInitial, dateEnd);
        }
    }

}
