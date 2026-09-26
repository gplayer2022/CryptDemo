namespace CryptDemo.Model
{
    public class VigenereCipher : CaesarCipher,  IDecryptable
    {
        /// <summary>
        /// キー
        /// </summary>
        private string key;
        /// <summary>
        /// シフト量たち
        /// </summary>
        int[] shifts;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">キー</param>
        internal VigenereCipher(string key) : base()
        {
            this.key = key;
            this.CalcShifts();
        }

        /// <summary>
        /// ヴィジュネル暗号で暗号化する
        /// </summary>
        /// <param name="text">平文</param>
        /// <param name="key">鍵</param>
        /// <returns>暗号化文</returns>
        internal string Encrypt(string text)
        {
            string encryptedText = "";
            int index = 0;
            foreach (char c in text)
            {
                encryptedText += base.Encrypt(c, this.shifts[index]);
                index = ++index % this.shifts.Length;
            }
            return encryptedText;
        }

        /// <summary>
        /// ヴィジュネル暗号を復号化する
        /// </summary>
        /// <param name="text">暗号文</param>
        /// <returns>復号化された平文</returns>
        public override Task<string> DecryptAsync(string text)
        {
            string decryptedText = "";
            int index = 0;
            foreach (char c in text)
            {
                decryptedText += base.Decrypt(c, this.shifts[index]);
                index = ++index % this.shifts.Length;
            }
            return Task.FromResult(decryptedText);
        }

        /// <summary>
        /// シフト量を集計する
        /// </summary>
        private void CalcShifts()
        {
            this.shifts = new int[this.key.Length];
            for (int i = 0; i < this.key.Length; i++)
            {
                this.shifts[i] = base.rotor.IndexOf(char.ToUpper(this.key[i]));
            }
        }
    }
}
