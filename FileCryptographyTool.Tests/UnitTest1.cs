using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

[TestClass]
public class CryptoTest
{
    private const string TestFile = "test.txt";
    private const string Key = "A1B2";

    [TestInitialize]
    public void Setup()
    {
        File.WriteAllText(TestFile, "Hello World");
    }

    [TestMethod]
    public void EncryptDecrypt_ReturnsOriginalData()
    {
        // Шифрование
        Program.EncryptFile(TestFile, "encrypted.bin", Key);
        // Дешифрование
        Program.DecryptFile("encrypted.bin", "decrypted.txt", Key);
        // Проверка
        Assert.AreEqual(File.ReadAllText(TestFile), File.ReadAllText("decrypted.txt"));
    }

    [TestCleanup]
    public void Cleanup()
    {
        File.Delete(TestFile);
        File.Delete("encrypted.bin");
        File.Delete("decrypted.txt");
    }
}