using System.Text;

namespace CryptDemo.Model
{
    /// <summary>
    /// シーザー暗号
    /// </summary>
    public class CaesarCipher : IDecryptable
    {
        /// <summary>
        /// アルファベット版
        /// </summary>
        protected string rotor;
        /// <summary>
        /// シフト数
        /// </summary>
        private int shift;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal CaesarCipher()
        {
            this.rotor = Alphabet.AlphabetUpperString;
        }

        /// <summary>
        /// キーフレーズ付のシーザー暗号用コンストラクタ
        /// </summary>
        /// <param name="keyPhrase"></param>
        internal CaesarCipher(int shift, string keyPhrase)
        {
            this.shift = shift;
            this.UpdateRotor(keyPhrase);
        }

        /// <summary>
        /// キーフレーズにしたがってアルファベット列を変更する
        /// </summary>
        /// <param name="keyPhrase"></param>
        private void UpdateRotor(string keyPhrase)
        {
            string restructedKeyPhrase = "";
            foreach (char c in keyPhrase.ToUpper())
            {
                if (!restructedKeyPhrase.Contains(c))
                {
                    restructedKeyPhrase += c;
                }
            }
            this.rotor = restructedKeyPhrase;
            foreach (char c in Alphabet.AlphabetUpperString)
            {
                if (!this.rotor.Contains(c))
                {
                    this.rotor += c;
                }
            }
        }

        /// <summary>
        /// 暗号化する
        /// </summary>
        /// <param name="text">平文</param>
        /// <returns>暗号文</returns>
        internal string Encrypt(string text)
        {
            return this.Encrypt(text, this.shift);
        }

        /// <summary>
        /// 暗号化する
        /// </summary>
        /// <param name="text">平文</param>
        /// <param name="shift">鍵</param>
        /// <returns>平文</returns>
        private string Encrypt(string text, int shift)
        {
            StringBuilder encryptedText = new StringBuilder();
            foreach (char c in text)
            {
                encryptedText.Append(this.Encrypt(c, shift));
            }
            return encryptedText.ToString();
        }

        /// <summary>
        /// 文字を暗号化する
        /// </summary>
        /// <param name="c">文字</param>
        /// <param name="shift">鍵（シフト量）</param>
        /// <returns>暗号化後の文字</returns>
        protected char Encrypt(char c, int shift)
        {
            char encryptedChar = c;
            // 大文字の場合
            if (this.rotor.Contains(c))
            {
                int index = (this.rotor.IndexOf(c) + shift) %
                    Alphabet.AlphabetStringLength;
                encryptedChar = this.rotor[index];
            }
            // 小文字の場合
            else if (this.rotor.ToLower().Contains(c))
            {
                int index = (this.rotor.ToLower().IndexOf(c) + shift) %
                    Alphabet.AlphabetStringLength;
                encryptedChar = this.rotor.ToLower()[index];
            }
            return encryptedChar;
        }

        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="text">暗号文</param>
        /// <returns>復号文</returns>
        public virtual string Decrypt(string text)
        {
            // shift 戻ることと (26 - shift) 進むことは同じ
            return this.Encrypt(text, Alphabet.AlphabetStringLength - this.shift);
        }

        /// <summary>
        /// 文字を復号化する
        /// </summary>
        /// <param name="c">文字</param>
        /// <param name="shift">鍵（シフト量）</param>
        /// <returns>復号化後の文字</returns>
        protected char Decrypt(char c, int shift)
        {
            return this.Encrypt(c, Alphabet.AlphabetStringLength - shift);
        }
    }
}
