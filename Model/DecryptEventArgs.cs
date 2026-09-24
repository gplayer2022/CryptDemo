namespace CryptDemo.Model
{
    public class DecryptEventArgs : EventArgs
    {
        /// <summary>
        /// 暗号文
        /// </summary>
        internal string EncryptedMessage
        {
            get; private set;
        }

        /// <summary>
        /// 暗号器（復号専用）
        /// </summary>
        internal IDecryptable Cipher
        {
            get; private set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="encryptedMessage">暗号文</param>
        /// <param name="cipher">暗号器（復号専用）</param>
        internal DecryptEventArgs(string encryptedMessage, IDecryptable cipher)
        {
            this.EncryptedMessage = encryptedMessage;
            this.Cipher = cipher;
        }
    }
}
