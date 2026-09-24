namespace CryptDemo.Model
{
    internal static class FrequencyAnalyser
    {
        /// <summary>
        /// 文字列の頻度を分析する
        /// </summary>
        /// <param name="text">分析する文字列</param>
        /// <returns>文字の頻度を表す辞書</returns>
        internal static Dictionary<char, int> Analyse(string text)
        {
            Dictionary<char, int> frequencies = FrequencyAnalyser.PrepareFrequencies();
            string allAlphabet = Alphabet.AlphabetUpperString + Alphabet.AlphabetLowerString;
            foreach (char c in text)
            {
                if (allAlphabet.Contains(c))
                {
                    char upperC = char.ToUpper(c);
                    frequencies[upperC]++;
                }
            }
            return frequencies;
        }

        /// <summary>
        /// 辞書を準備する
        /// </summary>
        /// <returns>初期化された辞書</returns>
        private static Dictionary<char, int> PrepareFrequencies()
        {
            Dictionary<char, int> frequencies = new Dictionary<char, int>();
            foreach (char c in Alphabet.AlphabetUpperString)
            {
                frequencies[c] = 0;
            }
            return frequencies;
        }
    }
}
