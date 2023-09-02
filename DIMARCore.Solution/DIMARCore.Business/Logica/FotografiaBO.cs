using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Helpers;
using GenteMarCore.Entities.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace DIMARCore.Business.Logica
{
    /// <summary>
    /// version anterior de la clase FotografiaBO para serializar en base de datos
    /// </summary>
    /// OBSOLETO
    [Obsolete]

    #region Obsolete
    public class FotografiaBO
    {
        public Respuesta ValidarArchivo(HttpPostedFile file)
        {
            Respuesta respuesta = new Respuesta();
            respuesta.Estado = true;
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    respuesta.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    respuesta.Mensaje = "El archivo es un dato requerido";
                    respuesta.Estado = false;
                }
                else
                {
                    string extencionArchivo = Path.GetExtension(file.FileName);
                    if (!extencionArchivo.ToUpper().Equals(".JPG") &&
                                !extencionArchivo.ToUpper().Equals(".PNG") &&
                                !extencionArchivo.ToUpper().Equals(".JPEG")
                                )
                    {

                        respuesta.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        respuesta.Mensaje = $"El formato del archivo no es compatible. Se permiten archivos con formato .jpg, .png o .jpeg {file.FileName}";
                        respuesta.Estado = false;


                    }

                }

            }
            catch (Exception ex)
            {
                respuesta.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                respuesta.MensajeExcepcion = ex.Message;
                respuesta.Estado = false;
                Debug.WriteLine(ex.Message);
            }

            return respuesta;

        }
        /// <summary>
        /// Serializa un documento
        /// </summary>
        /// <param name="documento">Documento</param>
        /// <returns></returns>
        public string SerializarArchivo(HttpPostedFile documento)
        {
            try
            {
                byte[] fileInBytes = new byte[documento.ContentLength];
                using (BinaryReader theReader = new BinaryReader(documento.InputStream))
                {
                    fileInBytes = theReader.ReadBytes(documento.ContentLength);
                }
                string fileAsString = Convert.ToBase64String(fileInBytes);
                return fileAsString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<Respuesta> CrearAsync(GENTEMAR_FOTOGRAFIA entidad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                await new FotografiaRepository().Create(entidad);
                respuesta.StatusCode = System.Net.HttpStatusCode.Created;
                respuesta.Mensaje = "creado.";
                respuesta.Estado = true;
            }

            catch (Exception ex)
            {
                respuesta.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                respuesta.MensajeExcepcion = ex.Message;
                Debug.WriteLine(ex.Message);
            }
            return respuesta;
        }

        public async Task<GENTEMAR_FOTOGRAFIA> GetByIduser(long id)
        {
            return await new FotografiaRepository().GetWithCondition(x => x.id_gentemar.Equals(id));
        }
    } 
    #endregion
}
