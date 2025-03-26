using System.Security.Cryptography;
using System.Text;

public static class AesEncryptor
{
    private const int KeySize = 256; // Размер ключа AES-256 (32 байта)
    private const int SaltSize = 16; // Размер соли (16 байт)
    private const int Iterations = 100_000; // Количество итераций PBKDF2

    public static void EncryptFile(string inputFile, string outputFile, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] key = GenerateKeyFromPassword(password, salt);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV(); 

            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
            {
                outputStream.Write(salt, 0, salt.Length);
                outputStream.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream cryptoStream = new CryptoStream(
                    outputStream,
                    aes.CreateEncryptor(),
                    CryptoStreamMode.Write))
                {
                    using (FileStream inputStream = File.OpenRead(inputFile))
                    {
                        inputStream.CopyTo(cryptoStream);
                    }
                }
            }
        }
    }

    public static void DecryptFile(string inputFile, string outputFile, string password)
    {
        using (FileStream inputStream = File.OpenRead(inputFile))
        {
            byte[] salt = new byte[SaltSize];
            byte[] iv = new byte[16]; // AES IV всегда 16 байт
            inputStream.Read(salt, 0, salt.Length);
            inputStream.Read(iv, 0, iv.Length);

            byte[] key = GenerateKeyFromPassword(password, salt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (CryptoStream cryptoStream = new CryptoStream(
                    inputStream,
                    aes.CreateDecryptor(),
                    CryptoStreamMode.Read))
                {
                    using (FileStream outputStream = File.OpenWrite(outputFile))
                    {
                        cryptoStream.CopyTo(outputStream);
                    }
                }
            }
        }
    }
    private static byte[] GenerateKeyFromPassword(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256))
        {
            return pbkdf2.GetBytes(KeySize / 8); 
        }
    }
}
