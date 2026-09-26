namespace CryptDemo.Model
{
    public interface IDecryptable
    {
        /// <summary>
        /// 復号する
        /// </summary>
        /// <param name="encryptedMessage">暗号文</param>
        /// <returns>復号文</returns>
        public Task<string> DecryptAsync(string encryptedMessage);
    }
}
