using DIMARCore.UIEntities.Requests;
using DIMARCore.Utilities.Seguridad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace GenteMarCore.Tests.Utilities.Seguridad
{
    [TestClass]
    public class EncryptDecryptRequestServiceTest
    {
        private const string Key = "AJ9Bo365^5Na3Cg!";
        private const string IV = "hS4Zi*@m2Tk7A@CU";
        private const string PlainText = "Hello, World!";
        private EncryptDecryptRequestService _service;
        [TestInitialize]
        public void TestInitialize()
        {
            _service = new EncryptDecryptRequestService(Key, IV);
        }

        [TestMethod]
        public void Encrypt_ShouldEncryptJson()
        {
            // Arrange
            LoginRequest loginRequest = new LoginRequest
            {
                UserName = "dparramol",
                Password = "undertaker",
                IpAddress = "190.71.155.26",
                Aplicacion = "MVC_GDM"
            };
            string json = JsonConvert.SerializeObject(loginRequest);
            // Act
            byte[] encrypted = _service.EncryptReturnBytes(json);

            // Assert
            Assert.IsNotNull(encrypted);
            Assert.IsTrue(encrypted.Length > 0);
        }

        [TestMethod]
        public void DecryptJson_ShouldDecryptEncryptedJson()
        {
            // Arrange
            LoginRequest loginRequest = new LoginRequest
            {
                UserName = "dparramol",
                Password = "undertaker",
                IpAddress = "190.71.155.26",
                Aplicacion = "MVC_GDM"
            };
            string json = JsonConvert.SerializeObject(loginRequest);
            // Act
            string encrypted = _service.EncryptReturnString(json);
            string decryptedText = _service.DecryptString(encrypted);

            var jsonDecrypted = JsonConvert.DeserializeObject<LoginRequest>(decryptedText);
            // Assert
            Assert.IsNotNull(decryptedText);
            Assert.IsTrue(encrypted.Length > 0);
            Assert.AreEqual(loginRequest.UserName, jsonDecrypted.UserName);
            Assert.AreEqual(loginRequest.Password, jsonDecrypted.Password);
            Assert.AreEqual(loginRequest.IpAddress, jsonDecrypted.IpAddress);
            Assert.AreEqual(loginRequest.Aplicacion, jsonDecrypted.Aplicacion);
        }

        [TestMethod]
        public void DecryptString_ShouldDecryptEncryptedString()
        {
            // Arrange
            byte[] plainTextBytes = _service.EncryptReturnBytes(PlainText);
            // Act
            string decryptedText = _service.DecryptBytes(plainTextBytes);
            Assert.AreEqual(PlainText, decryptedText);
        }
    }
}
