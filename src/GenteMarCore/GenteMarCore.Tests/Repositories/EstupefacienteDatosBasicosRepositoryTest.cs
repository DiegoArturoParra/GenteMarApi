using DIMARCore.Repositories.Repository;
using GenteMarCore.Tests.Global;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace GenteMarCore.Tests.Repositories
{
    [TestClass]
    public class EstupefacienteDatosBasicosRepositoryTest
    {
        private TestSetup _setup;
        private EstupefacienteDatosBasicosRepository _repository;

        public EstupefacienteDatosBasicosRepositoryTest()
        {
            _setup = new TestSetup();
            _repository = new EstupefacienteDatosBasicosRepository(_setup.Context);
        }

        // Se ejecuta una sola vez antes de que se ejecute la clase de pruebas
        [TestCleanup]
        public void Cleanup()
        {
            // Asegúrate de que los recursos se liberen correctamente
            _setup.Dispose();
        }
        [TestMethod]
        [DataRow("1.073.788.202", "18/07/2029", true)]
        [DataRow("1.073.788.202", "19/07/2029", true)]
        [DataRow("1.073.788.202", "20/07/2029", false)]
        [DataRow("1.118.811.838", "09/08/2023", true)]
        [DataRow("1.118.811.838", "10/08/2023", true)]
        [DataRow("1.118.811.838", "29/08/2024", false)]
        public async Task ValidarEstupefacienteVigentePersona_ValidInput_Returns(string identificacion, string fechaComparar, bool expected)
        {
            // Arrange
            var fechaActual = DateTime.Parse(fechaComparar);
            // Act
            var vciteVigentes = await _repository.ValidarEstupefacienteVigentePersona(identificacion, fechaActual);
            // Assert
            Assert.AreEqual(expected, vciteVigentes);
        }
    }
}
