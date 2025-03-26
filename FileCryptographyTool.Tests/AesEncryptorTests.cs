using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

[TestClass]
public class AesEncryptorTests
{
    private const string TestFile = "test.txt";
    private const string Password = "SecureP@ssw0rd!";

    [TestInitialize]
    public void Setup()
    {
        File.WriteAllText(TestFile, "This is a secret message!");
    }

    [TestMethod]
    public void EncryptDecrypt_ValidPassword_ReturnsOriginalData()
    {
        AesEncryptor.EncryptFile(TestFile, "encrypted.aes", Password);

        AesEncryptor.DecryptFile("encrypted.aes", "decrypted.txt", Password);

        string original = File.ReadAllText(TestFile);
        string decrypted = File.ReadAllText("decrypted.txt");
        Assert.AreEqual(original, decrypted);
    }

    [TestMethod]
    [ExpectedException(typeof(CryptographicException))]
    public void Decrypt_WrongPassword_ThrowsException()
    {
        AesEncryptor.EncryptFile(TestFile, "encrypted.aes", Password);
        AesEncryptor.DecryptFile("encrypted.aes", "decrypted.txt", "WrongPassword!"); // Должен упасть
    }

    [TestCleanup]
    public void Cleanup()
    {
        File.Delete(TestFile);
        if (File.Exists("encrypted.aes")) File.Delete("encrypted.aes");
        if (File.Exists("decrypted.txt")) File.Delete("decrypted.txt");
    }
}
