using log4net;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ConvertirStringApuntosDeMil(object identificacion)
        {
            string documento = string.Empty;

            try
            {
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

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

            }
            return documento;
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
            try
            {
                rutaInicial = string.IsNullOrEmpty(rutaInicial) ? rutaInicial.Trim() : rutaInicial;
                nombreArchivo = string.IsNullOrEmpty(nombreArchivo) ? nombreArchivo.Trim() : nombreArchivo;

                if (string.IsNullOrEmpty(rutaInicial))
                {
                    string mensaje = "Error, la ruta no es valida. Por favor contacte al administrador del sistema.";
                    _logger.Error(mensaje);
                    respuesta = Responses.SetConflictResponse(mensaje);
                }

                // valida existencia ruta inicial
                bool rutaInicialExiste = Directory.Exists(rutaInicial);
                if (!rutaInicialExiste)
                {
                    string mensaje = "Error. No se encuentra la ruta principal. Por favor contacte al administrador del sistema.";
                    _logger.Error(mensaje);
                    respuesta = Responses.SetConflictResponse(mensaje);
                }

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

                respuesta =  Responses.SetOkResponse(new Archivo()
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
                respuesta = Responses.SetOkResponse("Se ha eliminado correctamente el archivo.");
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

            if (images.Contains(extension.ToLower()))
            {
                documento = $"El archivo es de tipo documento con extensión: {extension.ToLower()}";
            }
            else if (archivos.Contains(extension.ToLower()))
            {
                documento = $"El archivo es de tipo Imagen con extensión: {extension.ToLower()}";
            }
            return documento;
        }
    }
}
