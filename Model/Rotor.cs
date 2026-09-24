namespace CryptDemo.Model
{
    /// <summary>
    /// ロータ（エニグマ用）
    /// </summary>
    public class Rotor
    {
        /// <summary>
        /// 文字盤
        /// </summary>
        internal string Dial
        {
            get; private set;
        }

        /// <summary>
        /// 切り欠き位置の文字
        /// </summary>
        internal char Notch
        {
            get; private set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dial">文字盤</param>
        /// <param name="notch">切り欠き位置の文字</param>
        internal Rotor(string dial, char notch)
        {
            this.Dial = dial;
            this.Notch = notch;
        }

        /// <summary>
        /// クローンする
        /// </summary>
        /// <returns>Rotor</returns>
        internal Rotor Clone()
        {
            return new Rotor(this.Dial, this.Notch);
        }
    }
}
