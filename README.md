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
AES-256

ct encrypt -a AES -i document.pdf -o secret.ct -p "password"
ct decrypt -a AES -i secret.ct -o document.pdf -p "pw"
XOR

ct encrypt -a XOR -i notes.txt -o notes.xor -k A1B2C3
ct decrypt -a XOR -i notes.xor -o notes.txt -k A1...

Generate random key

ct genkey -o mykey.bin -s 256

Derive key from password

ct genkey -o pbkdf2.key -p "pw" -i 100000

Tilek Sakyev COM22A 210104044

No video yet
