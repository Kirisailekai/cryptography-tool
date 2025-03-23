# Сryptography-tool

FileCryptographyTool is a console application for encrypting and decrypting files using AES-256 and XOR cipher. It supports key generation, password-based encryption, and batch processing.

Encryption & Decryption:
AES-256
XOR

Key Generation:
Cryptographically secure keys.
Password-based key derivation (PBKDF2).

Batch Processing:
Encrypt all files in a folder.

Installation
Build from Source
Install .NET 8 SDK.

Clone the repository:
git clone https://github.com/your-username/FileCryptographyTool.git
cd FileCryptographyTool
Build the project:
dotnet build
Download Release
Go to Releases and download .zip or .exe.

Usage
Basic Commands
Encrypt File (AES)
FileCryptographyTool encrypt --algo AES --input file.txt --output encrypted.bin --password "password"
Decrypt File (AES)
FileCryptographyTool decrypt --algo AES --input encrypted.bin --output decrypted.txt --password ""
Encrypt File (XOR)
FileCryptographyTool encrypt --algo XOR --input file.txt --output encrypted.bin --key "key" (w/o quotes)
Decrypt File (XOR)
FileCryptographyTool decrypt --algo XOR --input encrypted.bin --output decrypted.txt --key ""
Generate AES Key
FileCryptographyTool generatekey --output aes_key.bin

Tilek Sakyev COM22A 210104044

No video yet
