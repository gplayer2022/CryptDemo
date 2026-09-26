using System.Security.Cryptography;
using System.Text;
using Microsoft.JSInterop;

namespace CryptDemo.Model
{
    public class AesCipher : IDecryptable
    {
        /// <summary>
        /// JavaScript 参照用モジュール
        /// </summary>
        private readonly IJSObjectReference module;
        /// <summary>
        /// キーのハッシュ値をバイト配列として保持するフィールド
        /// </summary>
        /// <remarks>
        /// コンストラクターで初期化され、変更されない読み取り専用の内部データ
        /// </remarks>
        private readonly byte[] keyHashBytes;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">鍵</param>
        public AesCipher(IJSObjectReference module, string key)
        {
            this.module = module;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(key);
                this.keyHashBytes = sha256.ComputeHash(textBytes);
            }
        }

        /// <summary>
        /// AES 暗号で暗号化する
        /// </summary>
        /// <param name="text">平文</param>
        /// <returns>暗号化後に Base64 で変換された文字列</returns>
        internal async Task<string> EncryptAsync(string text)
        {
            string encryptedText = await this.module.InvokeAsync<string>(
                "encrypt", this.keyHashBytes, text);
            return encryptedText;
        }

        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="encryptedText">暗号文</param>
        /// <returns>復号文</returns>
        public async Task<string> DecryptAsync(string encryptedText)
        {
            string text = await this.module.InvokeAsync<string>(
                "decrypt", this.keyHashBytes, encryptedText);
            return text;
        }
    }
}
