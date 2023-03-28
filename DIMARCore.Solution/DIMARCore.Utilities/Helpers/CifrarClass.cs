using System;
using System.Security.Cryptography;
using System.Text;

namespace DIMARCore.Utilities.Helpers
{
    public class CifrarClass
    {
        private const String keyEncript = @"Desde el faro que con su luz hoy brilla iluminado por la mano del Señor muy atentos para guiar los rumbos del 
                        viajero que busca un resplandor. Cruzan las naves el camino más seguro y en sus quillas llevan la sabia equidad esperando y buscando en la armonía la
                        ruta más limpia entre el cielo y el mar. Apoyo, confianza, justicia y verdad son fuerza creadoras que unidas han de estar para alcanzar las rutas 
                        que Dios nos trazó para alcanzar las metas de nuestra Dimar. Adelante navegante, buen viento y buena mar que los medios para avante te los ofrece Dimar.";

        /// <summary>
        /// Encriptar / codificar
        /// </summary>
        /// <param name="cadenaEncriptar">cadena a encriptar</param>
        /// <param name="llaveHash">Llave hash</param>
        /// <param name="usarHash">Usar hash</param>
        /// <returns></returns>
        public static string Encriptar(string cadenaEncriptar, string llaveHash, bool usarHash)
        {
            byte[] llaveArray;
            byte[] arregloEncriptar = UTF8Encoding.UTF8.GetBytes(cadenaEncriptar);

            if (usarHash)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                llaveArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(llaveHash));
                hashmd5.Clear();
            }
            else
            {
                llaveArray = UTF8Encoding.UTF8.GetBytes(llaveHash);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = llaveArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(arregloEncriptar, 0, arregloEncriptar.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Desencriptar, Descodificar, descifrar
        /// </summary>
        /// <param name="cadenaCifrada">Cadena encriptada/cifrada</param>
        /// <param name="llaveHash">Llave hash</param>
        /// <param name="usarHash">Usar hash</param>
        /// <returns></returns>
        public static string Desencriptar(string cadenaCifrada, string llaveHash, bool usarHash)
        {
            byte[] llaveArray;
            byte[] arregloCifrado = Convert.FromBase64String(cadenaCifrada);

            if (usarHash)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                llaveArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(llaveHash));
                hashmd5.Clear();
            }
            else
            {
                llaveArray = UTF8Encoding.UTF8.GetBytes(llaveHash);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = llaveArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = null;
            try
            {
                resultArray = cTransform.TransformFinalBlock(arregloCifrado, 0, arregloCifrado.Length);
            }
            catch (Exception ex)
            {
                // throw ex;
            }

            tdes.Clear();

            if (resultArray == null)
            {
                return null;
            }

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

    }
}
