using Microsoft.JSInterop;
using System.Text;

namespace CryptDemo.Model
{
    /// <summary>
    /// RSA 暗号
    /// </summary>
    public class RsaCipher : IDecryptable
    {
        /// <summary>
        /// JavaScript モジュールの参照。JS 関数の呼び出しに使用する。
        /// </summary>
        /// <remarks>IJSRuntime でインポートして取得する。不要になったら DisposeAsync 等で解放すること。</remarks>
        private readonly IJSObjectReference module;
        /// <summary>
        /// 公開鍵
        /// </summary>
        private string publicKey = "";
        /// <summary>
        /// 秘密鍵
        /// </summary>
        private string secretKey = "";

        /// <summary>
        /// 公開鍵
        /// </summary>
        internal string PublicKey
        {
            get
            {
                return this.publicKey;
            }
        }

        /// <summary>
        /// 秘密鍵
        /// </summary>
        internal string SecretKey
        {
            get
            {
                return this.secretKey;
            }
            set
            {
                this.secretKey = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private RsaCipher(IJSObjectReference module)
        {
            this.module = module;
        }

        /// <summary>
        /// 鍵ペアを生成し、初期化済みのインスタンスを作る
        /// </summary>
        internal static async Task<RsaCipher> CreateAsync(IJSObjectReference module)
        {
            RsaCipher rsaCipher = new RsaCipher(module);
            KeyPairResult keyPair = await module.InvokeAsync<KeyPairResult>("generateKeyPair");
            rsaCipher.publicKey = keyPair.PublicKey;
            rsaCipher.secretKey = keyPair.PrivateKey;
            return rsaCipher;
        }

        /// <summary>
        /// 公開鍵で暗号化する
        /// </summary>
        /// <param name="message">平文</param>
        /// <returns>暗号文</returns>
        internal async Task<string> Encrypt(string message)
        {
            return await this.module.InvokeAsync<string>("encrypt", this.publicKey, message);
        }

        /// <summary>
        /// 秘密鍵で復号する
        /// </summary>
        /// <param name="encryptedMessage">暗号文</param>
        /// <returns>復号した平文</returns>
        public async Task<string> DecryptAsync(string encryptedMessage)
        {
            return await this.module.InvokeAsync<string>("decrypt", this.secretKey, encryptedMessage);
        }
    }
}
