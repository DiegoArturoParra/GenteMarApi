using DIMARCore.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GenteMarCore.Tests.Utilties.Helpers
{
    [TestClass]
    public class ReutilizablesTests
    {
        private const string RutaInicial = "Recursos";
        private const string RutaModuloTipoDocumento = "DATOS_BASICOS/IMAGENES_GENTE_MAR";
        private const string NombreArchivo = "imagen-test.png";

        [TestMethod]
        public void GetDelimitedList_ReturnsCorrectList_WhenInputHasElements()
        {
            // Arrange
            string input = "apple,banana,cherry";
            char delimiter = ',';
            List<string> expected = new List<string> { "apple", "banana", "cherry" };

            // Act
            List<string> result = Reutilizables.GetDelimitedList(input, delimiter);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DescribirDocumento_WhenGivenValidExtension_ShouldReturnCorrectDescription()
        {
            // Arrange
            var extension = ".pdf";
            var descripcionEsperada = $"El archivo es de tipo documento con extensión: {extension}";

            // Act
            var resultado = Reutilizables.DescribirDocumento(extension);

            // Assert
            Assert.AreEqual(descripcionEsperada, resultado);
        }

        [TestMethod]
        public void GeneratePdftoBase64_WhenGivenValidHtml_ShouldReturnBase64String()
        {
            // Arrange
            var html = "<html><body><h1>Hello, World!</h1></body></html>";

            // Act
            var resultado = Reutilizables.GeneratePdftoBase64(html);

            // Assert
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CombinePdfListInBase64_WhenGivenValidPdfList_ShouldReturnCombinedBase64String()
        {

            var html = "<html><body><h1>Hello, World!</h1></body></html>";
            var html2 = "<html><body><h1>Hello, GOOD AFTERNOON!</h1></body></html>";

            // Act
            var resultadoPDF1 = Reutilizables.GeneratePdftoBase64(html);
            var resultadoPDF2 = Reutilizables.GeneratePdftoBase64(html2);
            // Arrange
            var base64PdfList = new string[] { resultadoPDF1, resultadoPDF2 };
            var resultado = Reutilizables.CombinePdfListInBase64(base64PdfList);

            // Assert
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void GenerateBase64toBytes_ConvierteCorrectamenteDesdeBase64()
        {
            // Arrange
            string base64String = "SGVsbG8gd29ybGQh"; // Reemplaza con tu cadena Base64

            // Act
            byte[] resultBytes = Reutilizables.GenerateBase64toBytes(base64String);

            // Assert
            Assert.IsNotNull(resultBytes);
            // Agrega más aserciones según sea necesario para verificar la conversión correcta
        }



        [TestMethod]
        public void ReplaceVariables_ReemplazaVariablesCorrectamente()
        {
            // Arrange
            string template = "Hola {Nombre}, hoy es {Fecha}";
            var data = new { Nombre = "Usuario", Fecha = DateTime.Now };

            // Act
            string result = Reutilizables.ReplaceVariables(template, data, "");

            // Assert
            Assert.AreEqual("Hola Usuario, hoy es " + DateTime.Now.ToString("dd/MM/yyyy"), result);
        }

        [TestMethod]
        public void TablaHtmlVariables_CreaTablaHtmlCorrectamente()
        {
            // Arrange
            var dataList = new List<object>
        {
            new { Nombre = "Usuario1", Edad = 25 },
            new { Nombre = "Usuario2", Edad = 30 }
        };

            // Act
            string resultTable = Reutilizables.TablaHtmlVariables(dataList);

            // Assert
            Assert.IsNotNull(resultTable);
            // Agrega más aserciones según sea necesario para verificar la creación correcta de la tabla HTML
        }



        [TestMethod]
        public void CleanSpaces_ShouldRemoveSpacesFromProperties()
        {

            var myObject = new MyObject
            {
                Property1 = "   Texto con espacios   ",
                Property2 = "Otra cadena   ",
                Property3 = "   Cadena con espacios   ",
                Property4 = DateTime.Now
                // Otros campos...
            };

            // Act
            var cleanedObject = Reutilizables.CleanSpaces(myObject);

            // Assert
            Assert.AreEqual("Texto con espacios", cleanedObject.Property1);
            Assert.AreEqual("Otra cadena", cleanedObject.Property2);
            Assert.AreEqual("Cadena con espacios", cleanedObject.Property3);
        }

        [TestMethod]
        [DataRow(1234567, "1.234.567")]
        [DataRow("639609832", "639.609.832")]
        [DataRow("103061EF832", "103.061EF.832")]
        public void ConvertirStringApuntosDeMil_WhenGivenValidInput_ShouldReturnCorrectResult(object identificacion, string expected)
        {
            // Act
            string result = Reutilizables.ConvertirStringApuntosDeMil(identificacion);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("abc123xyz456", "123456")]
        [DataRow("abc456xyz789", "456789")]
        public void ObtenerSoloNumeros_WhenGivenValidInput_ShouldReturnOnlyNumbers(string cadena, string expected)
        {
            // Act
            string result = Reutilizables.ObtenerSoloNumeros(cadena);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("abc123xyz456", 123456)]
        [DataRow("abc456xyz789", 456789)]
        [DataRow("abc123xyz38!=?5", 123385)]
        public void ObtenerNumeroDesdeCadena_WhenGivenValidInput_ShouldReturnCorrectNumber(string cadena, int expected)
        {
            // Act
            int result = Reutilizables.ObtenerNumeroDesdeCadena(cadena);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DescargarArchivo_WhenGivenValidFilePath_ShouldReturnOkResponseAndBase64String()
        {
            // Arrange
            string rutaNombreArchivo = AppDomain.CurrentDomain.BaseDirectory.Insert(AppDomain.CurrentDomain.BaseDirectory.Length, $"\\{RutaInicial}\\Titulo.html");

            string archivoBase64;

            // Act
            Respuesta respuesta = Reutilizables.DescargarArchivo(rutaNombreArchivo, out archivoBase64);

            // Assert
            Assert.IsTrue(respuesta.Estado);
            Assert.IsNotNull(archivoBase64);
            // Agrega más aserciones según sea necesario
        }
        [TestMethod]
        public void ReadFile_LeeCorrectamenteElArchivo()
        {
            // Arrange
            string filePath = AppDomain.CurrentDomain.BaseDirectory.Insert(AppDomain.CurrentDomain.BaseDirectory.Length, $"\\{RutaInicial}\\{NombreArchivo}");

            // Act
            string fileContent = Reutilizables.ReadFile(filePath);

            // Assert
            Assert.IsNotNull(fileContent);
            // Agrega más aserciones según sea necesario para verificar la lectura correcta del archivo
        }

        [TestMethod]
        public void GuardarArchivoDeBytes_WhenGivenValidInput_ShouldReturnOkResponseAndCorrectFilePath()
        {
            // Arrange
            // Ruta de la imagen (ajusta la ruta según la ubicación de tu imagen)
            string rutaImagen = AppDomain.CurrentDomain.BaseDirectory.Insert(AppDomain.CurrentDomain.BaseDirectory.Length, $"\\{RutaInicial}\\{NombreArchivo}");

            // Lee la imagen como un arreglo de bytes
            byte[] archivoBytes = File.ReadAllBytes(rutaImagen);
            // Act
            Respuesta respuesta = Reutilizables.GuardarArchivoDeBytes(archivoBytes, RutaInicial, RutaModuloTipoDocumento, NombreArchivo);

            // Assert
            Assert.IsTrue(respuesta.Estado);
            Assert.IsNotNull(respuesta.Data);
            // Agrega más aserciones según sea necesario
        }

        [TestMethod]
        public void EliminarArchivo_WhenGivenValidPaths_ShouldReturnOkResponse()
        {
            // Arrange
            var respuestaEsperada = new Respuesta { Estado = true, Mensaje = "Se ha eliminado correctamente el archivo." };

            string filePath = AppDomain.CurrentDomain.BaseDirectory.Insert(AppDomain.CurrentDomain.BaseDirectory.Length, $"\\{RutaInicial}");

            // Act
            var resultado = Reutilizables.EliminarArchivo(filePath, "Titulo.html");

            // Assert
            Assert.AreEqual(respuestaEsperada.Estado, resultado.Estado);
            Assert.AreEqual(respuestaEsperada.Mensaje, resultado.Mensaje);
            Assert.AreEqual((int)HttpStatusCode.OK, (int)resultado.StatusCode);
        }

        [TestMethod]
        [DataRow("1990-01-01", "33", "2023-12-11")] // Ajusta según necesidad
        [DataRow("1998-11-15", "25", "2023-12-11")] // Otro conjunto de datos
        public void CalcularEdad_CalculaEdadCorrecta(string fechaNacimientoStr, string edadEsperadaStr, string fechaActualStr)
        {
            // Arrange
            DateTime fechaNacimiento = DateTime.Parse(fechaNacimientoStr);
            DateTime fechaActual = DateTime.Parse(fechaActualStr);
            short edadEsperada = short.Parse(edadEsperadaStr);

            // Act
            short edadCalculada = Reutilizables.CalcularEdad(fechaNacimiento, fechaActual);

            // Assert
            Assert.AreEqual(edadEsperada, edadCalculada);
        }

        [TestMethod]
        public void FormatDatesByRange_FormateaFechasCorrectamente()
        {
            // Arrange
            DateTime fechaInicial = new DateTime(2023, 1, 1, 12, 30, 0);
            DateTime fechaFinal = new DateTime(2023, 1, 2, 18, 45, 0);

            // Act
            var resultado = Reutilizables.FormatDatesByRange(fechaInicial, fechaFinal);

            // Assert
            Assert.AreEqual(new DateTime(2023, 1, 1, 0, 0, 0, 0), resultado.DateInitial);
            Assert.AreEqual(new DateTime(2023, 1, 2, 23, 59, 59), resultado.DateEnd);
        }
    }
    class MyObject
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public string Property3 { get; set; }
        public DateTime Property4 { get; set; }
        // Otros campos...
    }
}
