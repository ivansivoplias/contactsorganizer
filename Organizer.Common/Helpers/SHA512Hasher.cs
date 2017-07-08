using System;
using System.Security.Cryptography;
using System.Text;

namespace GameStore.Common.Hasher
{
    public class Sha512Hasher
    {
        private const int MinSaltSize = 4;
        private const int MaxSaltSize = 8;

        private const int HashSizeInBits = 512;
        private const int HashSizeInBytes = 64;

        private static Sha512Hasher _instance;

        private Sha512Hasher()
        {
        }

        public static Sha512Hasher GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Sha512Hasher();
            }
            return _instance;
        }

        public string ComputeHash(string plainText, byte[] saltBytes)
        {
            if (saltBytes == null)
            {
                // Generate a random number for the size of the salt.
                var random = new Random();
                int saltSize = random.Next(MinSaltSize, MaxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                var rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA512Managed();

            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            return Convert.ToBase64String(hashWithSaltBytes);
        }

        public byte[] GetSalt(string base64text)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(base64text);

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < HashSizeInBytes)
                throw new Exception("Length of encoded text are not enough.");

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        HashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[HashSizeInBytes + i];

            return saltBytes;
        }

        public bool VerifyHash(string plainText, string hashValue)
        {
            byte[] saltBytes = GetSalt(hashValue);

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, saltBytes);

            return (hashValue == expectedHashString);
        }
    }
}