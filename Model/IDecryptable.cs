namespace CryptDemo.Model
{
    public interface IDecryptable
    {
        /// <summary>
        /// 復号する
        /// </summary>
        /// <param name="encryptedMessage">暗号文</param>
        /// <returns></returns>
        public string Decrypt(string encryptedMessage);
    }
}
