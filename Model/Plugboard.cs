namespace CryptDemo.Model
{
    /// <summary>
    /// プラグボード（エニグマ用）
    /// </summary>
    public class Plugboard
    {
        private List<char> availablePlugs = Alphabet.AlphabetUpperString.ToList();
        private List<char[]> plugs = new List<char[]>();

        /// <summary>
        /// プラグを追加する
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <exception cref="ArgumentException"></exception>
        internal void AddPlug(char a, char b)
        {
            if (this.availablePlugs.Contains(char.ToUpper(a)) &&
                this.availablePlugs.Contains(char.ToUpper(b)))
            {
                this.plugs.Add(new char[] { a, b });
                this.availablePlugs.Remove(a);
                this.availablePlugs.Remove(b);
            }
            else
            {
                throw new ArgumentException(
                    "Plugboard に設定できない文字が指定されました。");
            }
        }

        /// <summary>
        /// プラグを通過させる
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>通過後の文字</returns>
        internal char PassThrough(char c)
        {
            char upperC = c.ToString().ToUpper()[0];
            char passedC = c;
            List<char[]> findedPlug = this.plugs.Where(delegate (char[] plus)
            {
                return plus.Contains(upperC);
            }).ToList();
            if (0 < findedPlug.Count)
            {
                char[] plug = findedPlug[0];
                // 大文字の場合
                if (0 <= Alphabet.AlphabetUpperString.IndexOf(c))
                {
                    if (plug[0] == upperC)
                    {
                        passedC = plug[1];
                    }
                    else
                    {
                        passedC = plug[0];
                    }
                }
                // 小文字の場合
                else
                {
                    if (plug[0] == upperC)
                    {
                        passedC = char.ToLower(plug[1]);
                    }
                    else
                    {
                        passedC = char.ToLower(plug[0]);
                    }
                }
            }
            return passedC;
        }
    }
}
