namespace CryptDemo.Model
{
    /// <summary>
    /// RSA で鍵ペアを格納するだけのクラス
    /// </summary>
    public class KeyPairResult
    {
        public string PublicKey
        {
            get; set;
        } = "";

        public string PrivateKey
        {
            get; set;
        } = "";
    }
}
