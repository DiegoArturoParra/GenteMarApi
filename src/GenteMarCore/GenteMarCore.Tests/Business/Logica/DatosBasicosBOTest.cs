using DIMARCore.Business.Logica;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Middleware;
using GenteMarCore.Entities.Models;
using GenteMarCore.Tests.Global;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GenteMarCore.Tests.Business.Logica
{
    [TestClass]
    public class DatosBasicosBOTest
    {
        private TestSetup _setup;
        private DatosBasicosRepository _datosBasicosRepository;
        private EstupefacienteDatosBasicosRepository _estupefacienteDatosBasicosRepository;
        private TituloRepository _tituloRepository;
        private LicenciaRepository _licenciaRepository;
        private ObservacionesDatosBasicosRepository _observacionesDatosBasicosRepository;
        private DatosBasicosBO _datosBasicosBO;
        private readonly string _documentoIdentificacionCreate = "87.654.321";

        [TestInitialize]
        public void Setup()
        {
            _setup = new TestSetup();
            _datosBasicosRepository = new DatosBasicosRepository(_setup.Context);
            _estupefacienteDatosBasicosRepository = new EstupefacienteDatosBasicosRepository(_setup.Context);
            _tituloRepository = new TituloRepository(_setup.Context);
            _licenciaRepository = new LicenciaRepository(_setup.Context);
            _observacionesDatosBasicosRepository = new ObservacionesDatosBasicosRepository(_setup.Context);
            _datosBasicosBO = new DatosBasicosBO(_datosBasicosRepository, _estupefacienteDatosBasicosRepository, _licenciaRepository, _tituloRepository);
        }

        // Se ejecuta una sola vez antes de que se ejecute la clase de pruebas
        [TestCleanup]
        public void Cleanup()
        {
            var data = _datosBasicosRepository.GetWithCondition(x => x.documento_identificacion == _documentoIdentificacionCreate);
            if (data != null)
            {
                var observacion = _observacionesDatosBasicosRepository.GetWithCondition(x => x.id_gentemar == data.id_gentemar);
                _observacionesDatosBasicosRepository.Delete(observacion).Wait();
                _datosBasicosRepository.Delete(data).Wait();
            }
            _setup.Dispose();
        }
        [TestMethod]
        [DataRow("1.073.788.202", "18/07/2029", true)]
        [DataRow("1.073.788.202", "19/07/2029", true)]
        [DataRow("1.073.788.202", "20/07/2029", false)]
        [DataRow("1.118.811.838", "09/08/2023", true)]
        [DataRow("1.118.811.838", "10/08/2023", true)]
        [DataRow("1.118.811.838", "29/08/2024", false)]

        public async Task GetDatosBasicosValidacionVcitePersona_ValidInput_Returns(string identificacion, string fecha, bool esValido)
        {
            // Act
            try
            {
                DateTime dateTime = DateTime.Parse(fecha);
                await _datosBasicosBO.ValidarVcitePersona(identificacion, dateTime);
                // Assert
                // Assert: Si no lanza excepción, debe ser un caso válido
                Assert.IsTrue(esValido, "No se esperaba excepción para un caso válido.");
            }
            catch (HttpStatusCodeException ex)
            {
                // Assert: Validar que las excepciones se lanzan correctamente en casos inválidos
                Assert.IsFalse(esValido, "Se esperaba excepción para un caso inválido.");
                Assert.AreEqual(HttpStatusCode.Conflict, ex.StatusCode);
            }
        }

        [TestMethod]
        public async Task GetlicenciaTituloVigentesPorDocumentoUsuario_ReturnsExpectedData()
        {
            // Arrange
            string documento = "1007692833";
            DateTime fechaVencimiento = DateTime.Parse("10/08/2023");

            // Act
            var result = await _datosBasicosBO.GetlicenciaTituloVigentesPorDocumentoUsuario(documento, fechaVencimiento);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Licencias.Count);
            Assert.AreEqual(1, result.Titulos.Count);
        }

        [TestMethod]
        public async Task GetlicenciaTituloVigentesPorDocumentoUsuario_InvalidDocument_ReturnsNull()
        {
            // Arrange
            string invalidDocumento = "123"; // Simula un documento inválido

            // Act
            var result = await _datosBasicosBO.GetlicenciaTituloVigentesPorDocumentoUsuario(invalidDocumento, DateTime.Now);

            // Assert
            Assert.AreEqual(0, result.Licencias.Count);
            Assert.AreEqual(0, result.Titulos.Count);
        }


        [TestMethod]
        public async Task CrearAsync_SuccessfulCreation_ReturnsCreatedResponse()
        {
            // Arrange
            var entidad = new GENTEMAR_DATOSBASICOS
            {
                documento_identificacion = _documentoIdentificacionCreate,
                nombres = "Ana",
                apellidos = "Gómez",
                telefono = "123456",
                numero_movil = "987654",
                correo_electronico = "ana@hotmail.com",
                fecha_nacimiento = DateTime.Now,
                id_tipo_documento = 1,
                id_genero = 1,
                direccion = "calle 7a bis",
                id_estado = 1,
                fecha_expedicion = DateTime.Now,
                fecha_vencimiento = DateTime.Now,
                id_pais_residencia = "160",
                id_formacion_grado = 2,
                id_municipio_residencia = 1,
                id_pais_nacimiento = "160",
                IncludePhoto = true,
                LoginCreacionId = 1,
                FechaCreacion = DateTime.Now,
                LoginModificacionId = 1,
                FechaModificacion = DateTime.Now
            };

            // Act
            var respuesta = await _datosBasicosBO.CrearAsync(entidad, string.Empty);

            // Assert
            Assert.IsTrue(respuesta.Estado);
            Assert.AreEqual(201, (int)respuesta.StatusCode);

            // Verificar creación en la base de datos
            var usuarioGuardado = await _datosBasicosRepository.GetWithConditionAsync(x => x.documento_identificacion == entidad.documento_identificacion);

            Assert.IsNotNull(usuarioGuardado);
            Assert.AreEqual(_documentoIdentificacionCreate, usuarioGuardado.documento_identificacion);
        }


        [TestMethod]
        public async Task CrearAsync_BadRequestPhotoRequired_ThrowsException()
        {
            // Arrange
            var entidadNueva = new GENTEMAR_DATOSBASICOS
            {
                documento_identificacion = "123",
                nombres = "Carlos",
                apellidos = "López",
                IncludePhoto = false,
            };

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<HttpStatusCodeException>(() =>
                _datosBasicosBO.CrearAsync(entidadNueva, "C:\\RutaPrueba"));
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.AreEqual(
                $"Debe adjuntar una foto de la persona.",
                exception.Message
            );
        }

        [TestMethod]
        public async Task CrearAsync_UserAlreadyExists_ThrowsException()
        {
            // Arrange

            var entidadNueva = new GENTEMAR_DATOSBASICOS
            {
                documento_identificacion = "1.007.692.833",
                nombres = "Carlos",
                apellidos = "López",
                IncludePhoto = true
            };

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<HttpStatusCodeException>(() =>
                _datosBasicosBO.CrearAsync(entidadNueva, "C:\\RutaPrueba"));
            Assert.AreEqual(HttpStatusCode.Conflict, exception.StatusCode);
            Assert.AreEqual(
                $"El usuario con # de identificación: {entidadNueva.documento_identificacion} ya se encuentra registrado.",
                exception.Message
            );
        }

        [TestMethod]
        public async Task GetDatosBasicosIdAsync_ReturnsDatosBasicosDTO()
        {
            // Arrange
            var entidad = new GENTEMAR_DATOSBASICOS
            {
                documento_identificacion = "1.030.922",
                nombres = "Ana",
                apellidos = "Gómez",
                telefono = "123456",
                numero_movil = "987654",
                correo_electronico = "ana@hotmail.com",
                fecha_nacimiento = DateTime.Now,
                id_tipo_documento = 1,
                id_genero = 2,
                direccion = "calle 7a bis",
                id_estado = 1,
                fecha_expedicion = DateTime.Now,
                fecha_vencimiento = DateTime.Now,
                id_pais_residencia = "160",
                id_formacion_grado = 2,
                id_municipio_residencia = 1,
                id_pais_nacimiento = "160",
                IncludePhoto = true,
                LoginCreacionId = 1,
                FechaCreacion = DateTime.Now,
                LoginModificacionId = 1,
                FechaModificacion = DateTime.Now
            };
            await _datosBasicosRepository.Create(entidad);

            var rutaInicial = @"C:\Pruebas";


            // Act
            var result = await _datosBasicosBO.GetDatosBasicosIdAsync(entidad.id_gentemar, rutaInicial);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Juan Perez", result.DocumentoIdentificacion);
            Assert.AreEqual("foto.jpg", result.UrlArchivo);
            await _datosBasicosRepository.Delete(entidad);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpStatusCodeException))]
        public async Task GetDatosBasicosIdAsync_ThrowsNotFoundException()
        {
            // Arrange
            var id = 99L;
            var rutaInicial = @"C:\Pruebas";

            // Act
            await _datosBasicosBO.GetDatosBasicosIdAsync(id, rutaInicial);

            // Assert (handled by ExpectedException)
        }
    }
}
