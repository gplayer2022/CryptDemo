namespace CryptDemo.Model
{
    /// <summary>
    /// プラグ接続用のクラス
    /// </summary>
    public class PlugPair
    {
        /// <summary>
        /// 左
        /// </summary>
        internal char? Left { get; set; }
        /// <summary>
        /// 右
        /// </summary>
        internal char? Right { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="left">左</param>
        /// <param name="right">右</param>
        internal PlugPair(char left, char right)
        {
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PlugPair()
        {

        }
    }
}
