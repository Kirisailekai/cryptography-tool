using System.Security.Cryptography;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  encrypt --input <path> --output <path> --key <key>");
            Console.WriteLine("  decrypt --input <path> --output <path> --key <key>");
            Console.WriteLine("  generatekey --output <path> --length <length>");
            return;
        }

        var command = args[0];
        var arguments = ParseArguments(args.Skip(1).ToArray());

        switch (command)
        {
            case "encrypt":
                EncryptFile(arguments["--input"], arguments["--output"], arguments["--key"]);
                break;
            case "decrypt":
                DecryptFile(arguments["--input"], arguments["--output"], arguments["--key"]);
                break;
            case "generatekey":
                GenerateKey(arguments["--output"], int.Parse(arguments["--length"]));
                break;
            default:
                Console.WriteLine($"Unknown: {command}");
                break;
        }
    }

    public static void EncryptFile(string inputPath, string outputPath, string key)
    {
        var data = File.ReadAllBytes(inputPath);
        var keyBytes = ParseHexKey(key);
        ProcessXor(data, keyBytes);
        File.WriteAllBytes(outputPath, data);
    }

    public static void DecryptFile(string inputPath, string outputPath, string key)
    {
        EncryptFile(inputPath, outputPath, key);
    }

    public static void GenerateKey(string outputPath, int length)
    {
        var key = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        File.WriteAllText(outputPath, BitConverter.ToString(key).Replace("-", ""));
    }

    private static Dictionary<string, string> ParseArguments(IEnumerable<string> args)
    {
        var result = new Dictionary<string, string>();
        var currentKey = "";
        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                currentKey = arg.Substring(2);
                result[currentKey] = "";
            }
            else if (!string.IsNullOrEmpty(currentKey))
            {
                result[currentKey] = arg;
            }
        }
        return result;
    }

    private static byte[] ParseHexKey(string key)
    {
        return Enumerable.Range(0, key.Length / 2)
            .Select(x => Convert.ToByte(key.Substring(x * 2, 2), 16))
            .ToArray();
    }

    private static void ProcessXor(byte[] data, byte[] key)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= key[i % key.Length];
        }
    }
}