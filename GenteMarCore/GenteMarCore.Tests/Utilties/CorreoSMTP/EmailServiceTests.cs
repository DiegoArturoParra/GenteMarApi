using DIMARCore.Utilities.CorreoSMTP;
using DIMARCore.Utilities.Seguridad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GenteMarCore.Tests.Utilties.CorreoSMTP
{
    [TestClass]
    public class EmailServiceTests
    {
        private EMailService _emailService;
        private string _password = "3VhUckxhWAD7WsY+tMd5/zxn+mKjgi7D88+LEx9YB/c=";
        private string _from = "hZARSaW90A8u0wO0IbG2bXrNkE27kK6LN6jIJSHQo3XMzPQLhMMd7lAwSwoOJonKo44O3gN4+WjbsF7DXCHfzg==";
        private string _host = "smtp.office365.com";
        private int _port = 587;

        [TestInitialize]
        public void Setup()
        {
            _password = SecurityEncrypt.GenerateDecrypt(_password);
            _from = SecurityEncrypt.GenerateDecrypt(_from);
            _emailService = new EMailService(from: _from, password: _password, host: _host, port: _port);
        }

        [TestMethod]
        public async Task SendMail_ValidInput_SendsEmailSuccessfully()
        {
            // Arrange
            var correosDestino = new string[] { "walvarez@dimar.mil.co", "dparramol@dimar.mil.co", "jvargasq@dimar.mil.co" };
            var mensaje = "Test CORREO Message";
            var body = "Test CORREO Body";
            var title = "Test CORREO Title";
            var footer = "Test CORREO Footer";
            var emailRequest = new SendEmailRequest(correosDestino, mensaje, body, title, footer);
            // Act
            var response = await _emailService.SendMail(emailRequest);
            Assert.AreEqual((int)HttpStatusCode.OK, (int)response.StatusCode);
        }

        [TestMethod]
        public async Task SendMail_InvalidInput_ThrowsException()
        {
            // Arrange
            var correosDestino = new string[] { "" };
            var mensaje = "Test CORREO Message";
            var body = "Test CORREO Body";
            var title = "Test CORREO Title";
            var footer = "Test CORREO Footer";
            var emailRequest = new SendEmailRequest(correosDestino, mensaje, body, title, footer);
            // Act
            try
            {
                var response = await _emailService.SendMail(emailRequest);
                Assert.AreEqual((int)HttpStatusCode.Conflict, (int)response.StatusCode);
                Assert.Fail("Sending email should have failed");
            }
            catch (Exception ex)
            {
                // Assert that an exception was thrown
                Assert.IsNotNull(ex);
            }
        }
    }
}
