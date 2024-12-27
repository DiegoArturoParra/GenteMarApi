using DIMARCore.Utilities.Seguridad;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenteMarCore.Tests.Utilities.Seguridad
{
    [TestClass]
    public class EncryptDecryptServiceTest
    {
        [TestMethod]
        public void Desencrypt_ShouldBe_Equals()
        {
            string valueEncrypt = "NHH18iwVZ8BHdWGNF+bpevdDNR+5XwFyj2LPaAaV/5SOrGiunjLRntxQyeDDxbNXPPQE/VZajqwE3Icfg8AkEw==";
            string decript = EncryptDecryptService.GenerateDecrypt(valueEncrypt);
            string encript = EncryptDecryptService.GenerateEncrypt(decript);
            Assert.AreEqual(valueEncrypt, encript);
        }
        [TestMethod]
        public void Encrypt_ShouldBe_Equals()
        {
            string valuedesencrypt = "DEVELOPERS GDM DIEGO/CAMILO";
            string encript = EncryptDecryptService.GenerateEncrypt(valuedesencrypt);
            string decript = EncryptDecryptService.GenerateDecrypt(encript);
            Assert.AreEqual(valuedesencrypt, decript);
        }
    }
}
