using DIMARCore.Utilities.CorreoSMTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace GenteMarCore.Tests.Utilties.CorreoSMTP
{
    [TestClass]
    public class EmailServiceTests
    {
        private EMailService _emailService;

        [TestInitialize]
        public void Setup()
        {
            _emailService = new EMailService(from: "serviciodimar@dimar.mil.co", password: "Wug08640", host: "smtp.office365.com", 587);
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

            // Act
            try
            {
                await _emailService.SendMail(correosDestino, mensaje, body, title, footer);
            }
            catch (Exception ex)
            {
                // If an exception is thrown, fail the test
                Assert.Fail($"Sending email failed: {ex.Message}");
            }
        }
    }
}
