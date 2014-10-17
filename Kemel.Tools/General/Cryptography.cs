using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Tools.General
{
    using Resources;
    using Text;
    using System.Security.Cryptography;

    public static class Cryptography
    {
        static readonly byte[] _key = null;
        static SymmetricAlgorithmOptions _symmetricAlgorithm = SymmetricAlgorithmOptions.DES;
        static HashAlgorithmOptions _hashAlgorithm = HashAlgorithmOptions.MD5;

        static Cryptography()
        {
            _key = AppEncoding.GetBytes(GlobalResources.CryptoKey);
        }

        public static SymmetricAlgorithmOptions SymmetricAlgorithm
        {
            get { return _symmetricAlgorithm; }
            set { _symmetricAlgorithm = value; }
        }

        public static HashAlgorithmOptions HashAlgorithm
        {
            get { return _hashAlgorithm; }
            set { _hashAlgorithm = value; }
        }

        private static SymmetricAlgorithm GetSymmetricAlgorithm()
        {
            switch (Cryptography.SymmetricAlgorithm)
            {
                default:
                case SymmetricAlgorithmOptions.DES:
                    return DES.Create();
                case SymmetricAlgorithmOptions.TripleDES:
                    return TripleDES.Create();
                case SymmetricAlgorithmOptions.Rijndael:
                    return Rijndael.Create();
                case SymmetricAlgorithmOptions.RC2:
                    return RC2.Create();
            }
        }

        private static HashAlgorithm GetHashAlgorithm()
        {
            switch (Cryptography.HashAlgorithm)
            {
                default:
                case HashAlgorithmOptions.MD5:
                    return MD5.Create();
                case HashAlgorithmOptions.RIPEMD160:
                    return RIPEMD160.Create();
                case HashAlgorithmOptions.SHA1:
                    return SHA1.Create();
                case HashAlgorithmOptions.SHA256:
                    return SHA256.Create();
                case HashAlgorithmOptions.SHA384:
                    return SHA384.Create();
                case HashAlgorithmOptions.SHA512:
                    return SHA512.Create();
            }
        }

        public static byte[] Encrypt(byte[] buffer)
        {
            if (ReferenceEquals(buffer, null))
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Empty Array", "buffer");
            }

            SymmetricAlgorithm algorithm = GetSymmetricAlgorithm();

            byte[] key = new byte[algorithm.Key.Length];
            byte[] iv = new byte[algorithm.IV.Length];

            Buffer.BlockCopy(_key, 0, key, 0, key.Length);
            Buffer.BlockCopy(_key, _key.Length - iv.Length, iv, 0, iv.Length);

            using (ICryptoTransform transform = algorithm.CreateEncryptor(key, iv))
            {
                return transform.TransformFinalBlock(buffer, 0, buffer.Length);
            }
        }

        public static byte[] Encrypt(byte[] buffer, string criptoKey)
        {
            byte[] btCriptoKey = AppEncoding.GetBytes(criptoKey);
            if (ReferenceEquals(buffer, null))
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Empty Array", "buffer");
            }

            SymmetricAlgorithm algorithm = GetSymmetricAlgorithm();

            byte[] key = new byte[algorithm.Key.Length];
            byte[] iv = new byte[algorithm.IV.Length];

            Buffer.BlockCopy(btCriptoKey, 0, key, 0, key.Length);
            Buffer.BlockCopy(btCriptoKey, btCriptoKey.Length - iv.Length, iv, 0, iv.Length);

            using (ICryptoTransform transform = algorithm.CreateEncryptor(key, iv))
            {
                return transform.TransformFinalBlock(buffer, 0, buffer.Length);
            }
        }

        public static byte[] Decrypt(byte[] buffer)
        {
            if (ReferenceEquals(buffer, null))
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Empty Array", "buffer");
            }

            SymmetricAlgorithm algorithm = GetSymmetricAlgorithm();

            byte[] key = new byte[algorithm.Key.Length];
            byte[] iv = new byte[algorithm.IV.Length];

            Buffer.BlockCopy(_key, 0, key, 0, key.Length);
            Buffer.BlockCopy(_key, _key.Length - iv.Length, iv, 0, iv.Length);

            using (ICryptoTransform transform = algorithm.CreateDecryptor(key, iv))
            {
                return transform.TransformFinalBlock(buffer, 0, buffer.Length);
            }
        }

        public static byte[] Decrypt(byte[] buffer, string criptoKey)
        {
            byte[] btCriptoKey = AppEncoding.GetBytes(criptoKey);
            if (ReferenceEquals(buffer, null))
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Empty Array", "buffer");
            }

            SymmetricAlgorithm algorithm = GetSymmetricAlgorithm();

            byte[] key = new byte[algorithm.Key.Length];
            byte[] iv = new byte[algorithm.IV.Length];

            Buffer.BlockCopy(btCriptoKey, 0, key, 0, key.Length);
            Buffer.BlockCopy(btCriptoKey, btCriptoKey.Length - iv.Length, iv, 0, iv.Length);

            using (ICryptoTransform transform = algorithm.CreateDecryptor(key, iv))
            {
                return transform.TransformFinalBlock(buffer, 0, buffer.Length);
            }
        }

        public static string EncryptText(string text)
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            if (text.Length == 0) { return string.Empty; }

            return Convert.ToBase64String(Encrypt(AppEncoding.GetBytes(text)));
        }

        public static string DecryptText(string text)
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            if (text.Length == 0) { return string.Empty; }

            return AppEncoding.GetString(Decrypt(Convert.FromBase64String(text)));
        }

        public static byte[] ComputeHash(byte[] buffer)
        {
            if (ReferenceEquals(buffer, null))
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Empty Array", "buffer");
            }

            HashAlgorithm algorithm = GetHashAlgorithm();
            return algorithm.ComputeHash(buffer);
        }

        public static string ComputeHashText(string text)
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            HashAlgorithm algorithm = GetHashAlgorithm();
            return Convert.ToBase64String(algorithm.ComputeHash(AppEncoding.GetBytes(text)));
        }

        public static string ComputeHashHexText(string text, int length)
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            HashAlgorithm algorithm = GetHashAlgorithm();
            byte[] buffer = algorithm.ComputeHash(AppEncoding.GetBytes(text));
            StringBuilder result = new StringBuilder();

            foreach (byte b in buffer)
            {
                if (length != 0 && result.Length >= length)
                {
                    break;
                }

                result.AppendFormat("{0:x2}", b);
            }

            while (result.Length < length)
            {
                result.Append("0");
            }

            return result.ToString();
        }

        public static string ComputeHashHexText(string text)
        {
            return ComputeHashHexText(text, 0);
        }

        internal static string EncryptText(string text, string criptoKey)
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            if (text.Length == 0) { return string.Empty; }

            return Convert.ToBase64String(Encrypt(AppEncoding.GetBytes(text), criptoKey));
        }

        internal static string DecryptText(string text, string criptoKey)
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            if (text.Length == 0) { return string.Empty; }

            return AppEncoding.GetString(Decrypt(Convert.FromBase64String(text), criptoKey));
        }
    }

    public enum SymmetricAlgorithmOptions : byte
    {
        DES = 0,
        TripleDES = 1,
        RC2 = 2,
        Rijndael = 3
    }

    public enum HashAlgorithmOptions : byte
    {
        MD5 = 0,
        RIPEMD160 = 1,
        SHA1 = 2,
        SHA256 = 3,
        SHA384 = 4,
        SHA512 = 5
    }
}
