using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  encrypt --algo <AES|XOR> --input <path> --output <path> (--key <key> | --password <password>)");
            Console.WriteLine("  decrypt --algo <AES|XOR> --input <path> --output <path> (--key <key> | --password <password>)");
            Console.WriteLine("  generatekey --output <path> --length <length> [--algorithm <AES|XOR>]");
            return;
        }

        var command = args[0];
        var arguments = ParseArguments(args.Skip(1).ToArray());

        try
        {
            switch (command.ToLower())
            {
                case "encrypt":
                    HandleEncryption(arguments);
                    break;

                case "decrypt":
                    HandleDecryption(arguments);
                    break;

                case "generatekey":
                    HandleKeyGeneration(arguments);
                    break;

                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void HandleEncryption(Dictionary<string, string> args)
    {
        ValidateArgs(args, new[] { "--input", "--output" });

        if (args["--algo"].ToUpper() == "AES")
        {
            ValidateArgs(args, new[] { "--password" });
            AesEncryptor.EncryptFile(args["--input"], args["--output"], args["--password"]);
        }
        else
        {
            ValidateArgs(args, new[] { "--key" });
            XorEncrypt(args["--input"], args["--output"], args["--key"]);
        }
    }

    private static void HandleDecryption(Dictionary<string, string> args)
    {
        ValidateArgs(args, new[] { "--input", "--output" });

        if (args["--algo"].ToUpper() == "AES")
        {
            ValidateArgs(args, new[] { "--password" });
            AesEncryptor.DecryptFile(args["--input"], args["--output"], args["--password"]);
        }
        else
        {
            ValidateArgs(args, new[] { "--key" });
            XorEncrypt(args["--input"], args["--output"], args["--key"]);
        }
    }

    private static void HandleKeyGeneration(Dictionary<string, string> args)
    {
        ValidateArgs(args, new[] { "--output", "--length" });
        
        var algorithm = args.ContainsKey("--algorithm") 
            ? args["--algorithm"].ToUpper() 
            : "XOR";

        if (algorithm == "AES")
        {
            var length = int.Parse(args["--length"]);
            if (length != 128 && length != 192 && length != 256)
                throw new ArgumentException("Invalid key size for AES. Use 128, 192 or 256");
            
            AesEncryptor.GenerateKey(args["--output"], length);
        }
        else
        {
            var length = int.Parse(args["--length"]);
            GenerateXorKey(args["--output"], length);
        }
    }

    private static void ValidateArgs(Dictionary<string, string> args, string[] required)
    {
        foreach (var arg in required)
        {
            if (!args.ContainsKey(arg) || string.IsNullOrEmpty(args[arg]))
                throw new ArgumentException($"Missing required argument: {arg}");
        }
    }

    private static void XorEncrypt(string inputPath, string outputPath, string key)
    {
        var data = File.ReadAllBytes(inputPath);
        var keyBytes = ParseHexKey(key);
        ProcessXor(data, keyBytes);
        File.WriteAllBytes(outputPath, data);
    }

    private static void GenerateXorKey(string outputPath, int length)
    {
        var key = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        File.WriteAllText(outputPath, BitConverter.ToString(key).Replace("-", ""));
    }
    private static Dictionary<string, string> ParseArguments(IEnumerable<string> args) { ... }
    private static byte[] ParseHexKey(string key) { ... }
    private static void ProcessXor(byte[] data, byte[] key) { ... }
}
