using DIMARCore.Utilities.Seguridad;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenteMarCore.Tests.Utilties.Seguridad
{
    [TestClass]
    public class SecurityEncryptTests
    {
        [TestMethod]
        public void Desencrypt_ShouldBe_Equals()
        {
            string valueEncrypt = "jbtyxgjxvG5EJt6GotiKmEsNCpdKt//atyPGLZ1yFOu5QoVQ3feLomEz2w+Jgh9Z0CPCrpng4L+krqRo0oDLIYxzpl5Dor2BfIl6yckvFsQ=";
            string decript = SecurityEncrypt.GenerateDecrypt(valueEncrypt);
            string encript = SecurityEncrypt.GenerateEncrypt(decript);
            Assert.AreEqual(valueEncrypt, encript);
        }
        [TestMethod]
        public void Encrypt_ShouldBe_Equals()
        {
            string valuedesencrypt = "DEVELOPERS GDM DIEGO/CAMILO";
            string encript = SecurityEncrypt.GenerateEncrypt(valuedesencrypt);
            string decript = SecurityEncrypt.GenerateDecrypt(encript);
            Assert.AreEqual(valuedesencrypt, decript);
        }
    }
}
