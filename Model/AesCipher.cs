using System.Security.Cryptography;
using System.Text;

namespace CryptDemo.Model
{
    public class AesCipher : IDecryptable
    {
        private byte[] keyBytes;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">鍵</param>
        public AesCipher(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length != 32)
            {
                throw new ArgumentException("鍵は 32 バイト（ 256 ビット）である必要があります。");
            }
            this.keyBytes = keyBytes;
        }

        /// <summary>
        /// AES 暗号で暗号化する
        /// </summary>
        /// <param name="text">平文</param>
        /// <returns>暗号化後に Base64 で変換された文字列</returns>
        internal string Encrypt(string text)
        {
            //byte[] plainBytes = Encoding.UTF8.GetBytes(text);
            //byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize]; // 12 バイト
            //RandomNumberGenerator.Fill(nonce);

            //byte[] cipherBytes = new byte[plainBytes.Length];
            //byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize]; // 16 バイト

            //using (var aesGcm = new AesGcm(this.keyBytes))
            //{
            //    aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);
            //}

            //// 結合: [Nonce (12B)] + [Tag (16B)] + [Ciphertext]
            //byte[] result = new byte[nonce.Length + tag.Length + cipherBytes.Length];
            //Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
            //Buffer.BlockCopy(tag, 0, result, nonce.Length, tag.Length);
            //Buffer.BlockCopy(cipherBytes, 0, result, nonce.Length + tag.Length, cipherBytes.Length);

            //return Convert.ToBase64String(result);
            return "";
        }

        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="encryptedMessage">暗号文</param>
        /// <returns>復号文</returns>
        public string Decrypt(string encryptedMessage)
        {
            //// Base64 文字列をバイト配列に変換
            //byte[] connectedBytes = Convert.FromBase64String(encryptedMessage);
            //// 暗号化されたバイト配列から IV と暗号化された部分を分離する
            //// IV の長さは 16 バイトの固定長
            //byte[] iv = new byte[16];
            //Array.Copy(connectedBytes, 0,
            //    iv, 0,
            //    iv.Length);
            //// 暗号化された部分のバイト配列を作成
            //byte[] encryptedBytes = new byte[connectedBytes.Length - iv.Length];
            //Array.Copy(connectedBytes, iv.Length,
            //    encryptedBytes, 0,
            //    encryptedBytes.Length);
            //using (Aes aes = Aes.Create())
            //{
            //    // 鍵を設定する
            //    aes.Key = this.keyBytes;
            //    aes.IV = iv;
            //    using (ICryptoTransform decryptor = aes.CreateDecryptor())
            //    {
            //        // 復号化
            //        byte[] plainTextBytes = decryptor.TransformFinalBlock(
            //            encryptedBytes, 0, encryptedBytes.Length);
            //        // バイト配列を文字列に変換して返す
            //        return Encoding.UTF8.GetString(plainTextBytes);
            //    }
            //}
            return "";
        }


    }
}
