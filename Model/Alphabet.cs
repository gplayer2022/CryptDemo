namespace CryptDemo.Model
{
    public class Alphabet
    {
        /// <summary>
        /// アルファベット大文字
        /// </summary>
        private static readonly string alphabetUpperString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// アルファベット小文字
        /// </summary>
        private static readonly string alphabetLowerString = Alphabet.alphabetUpperString.ToLower();

        /// <summary>
        /// 大文字のアルファベット
        /// </summary>
        internal static string AlphabetUpperString
        {
            get
            {
                return Alphabet.alphabetUpperString;
            }
        }

        /// <summary>
        /// 小文字のアルファベット
        /// </summary>
        internal static string AlphabetLowerString
        {
            get
            {
                return Alphabet.alphabetLowerString;
            }
        }

        /// <summary>
        /// アルファベットの文字数
        /// </summary>
        internal static int AlphabetStringLength
        {
            get
            {
                return Alphabet.alphabetUpperString.Length;
            }
        }
    }
}
