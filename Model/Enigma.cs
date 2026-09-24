using System.Text;

namespace CryptDemo.Model
{
    public class Enigma : IDecryptable
    {
        /// <summary>
        /// ロータの文字列と切り欠き位置たち
        /// ロータは 5 つ中 3 つまで使用可能
        /// </summary>
        private readonly List<Rotor> fiveRotors = new List<Rotor> {
            new Rotor("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q'),
            new Rotor("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E'),
            new Rotor("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V'),
            new Rotor("ESOVPZJAYQUIRHXLNFTGKDCMWB", 'J'),
            new Rotor("VZBRGITYUPSDNHLXAWMJQOFECK", 'Z'),
        };
        /// <summary>
        /// リフレクタたち
        /// </summary>
        private readonly string[] reflectors = new string[]
        {
            "EJMZALYXVBWFCRQUONTSPIKHGD",
            "YRUHQSLDPXNGOKMIEBFZCWVJAT",
            "FVPJIAOYEDRZXWGCTKUQSBNMHL",
        };
        /// <summary>
        /// リフレクターの文字列
        /// </summary>
        private readonly string reflector;
        /// <summary>
        /// ロータの枚数
        /// </summary>
        private readonly static int routorsCount = 3;

        /// <summary>
        /// 本体前面のプラグボード
        /// </summary>
        private Plugboard plugboard;
        /// <summary>
        /// 設定されたロータたち
        /// </summary>
        private List<Rotor> rotors = new List<Rotor>();
        /// <summary>
        /// ロータの初期位置たち
        /// </summary>
        private int[] rotorInitialPositions = new int[Enigma.routorsCount];
        /// <summary>
        /// ロータの現在位置たち
        /// </summary>
        private int[] rotorPositions = new int[Enigma.routorsCount];

        /// <summary>
        /// ロータたち
        /// </summary>
        internal string[] Rotors
        {
            get
            {
                string[] rotors = new string[this.fiveRotors.Count];
                int i = 0;
                foreach (Rotor rotor in this.fiveRotors)
                {
                    rotors[i++] = rotor.Dial;
                }
                return rotors;
            }
        }
        /// <summary>
        /// リフレクタたち
        /// </summary>
        internal string[] Reflectors
        {
            get
            {
                return this.reflectors;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// （リフレクタやロータのみ取得するための空のコンストラクタ）
        /// </summary>
        internal Enigma()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// （エラー処理は省略）
        /// （ロータの数は 3 つに固定）
        /// （ポジションの各要素は 0 以上から 25 以下の範囲で指定）
        /// </summary>
        /// <param name="rotorNumbers">設置するロータたち</param>
        /// <param name="rotorPositions">ロータの初期位置</param>
        /// <param name="reflectorNumber">リフレクタ</param>
        internal Enigma(int[] rotorNumbers, int[] rotorPositions, int reflectorNumber)
        {
            this.SetRotors(rotorNumbers);
            this.rotorInitialPositions = (int[])rotorPositions.Clone();
            this.rotorPositions = (int[])rotorPositions.Clone();
            this.plugboard = new Plugboard();
            this.reflector = this.reflectors[reflectorNumber];
        }

        /// <summary>
        /// プラグボードにプラグを追加する
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        internal void AddPlugboardPlug(char a, char b)
        {
            this.plugboard.AddPlug(a, b);
        }

        /// <summary>
        /// 暗号化する
        /// </summary>
        /// <param name="text">平文</param>
        /// <returns>暗号化された文</returns>
        internal string Encrypt(string text)
        {
            StringBuilder encryptedText = new StringBuilder();
            foreach (char c in text)
            {
                encryptedText.Append(this.Encrypt(c));
            }
            return encryptedText.ToString();
        }

        /// <summary>
        /// 復号化する
        /// </summary>
        /// <param name="text">暗号文</param>
        /// <returns>復号化された平文</returns>
        public string Decrypt(string text)
        {
            this.rotorPositions = (int[])this.rotorInitialPositions.Clone();
            // Enigma の暗号化と復号化は同じ処理を行うため、Encrypt メソッドを呼び出す
            return this.Encrypt(text);
        }

        /// <summary>
        /// 文字を暗号化する
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>暗号化された文字</returns>
        private char Encrypt(char c)
        {
            // ロータの位置を回転（修正：このメソッド末での回転から、メソッド開始時に変更）
            this.RotateRotors();
            char currentChar = c;
            int index;
            AlphabetCase alphabetCase = this.CheckAlphabetCase(c, out index);
            // アルファベット以外の文字はそのまま返す
            if (alphabetCase == AlphabetCase.Other)
            {
                return currentChar;
            }
            // プラグボードを通過
            currentChar = this.plugboard.PassThrough(currentChar);
            index = Alphabet.AlphabetUpperString.IndexOf(char.ToUpper(currentChar));
            // ロータを通過
            for (int i = 0; i < this.rotors.Count; i++)
            {
                index = (index + this.rotorPositions[i]) % Alphabet.AlphabetStringLength;
                currentChar = this.rotors[i].Dial[index];
                index = Alphabet.AlphabetUpperString.IndexOf(currentChar);
                index = (index - this.rotorPositions[i] + Alphabet.AlphabetStringLength) % Alphabet.AlphabetStringLength;
            }
            // リフレクターを通過
            currentChar = this.reflector[index];
            index = Alphabet.AlphabetUpperString.IndexOf(currentChar);
            // ロータを逆に通過
            for (int i = this.rotors.Count - 1; 0 <= i; i--)
            {
                char shiftedChar = Alphabet.AlphabetUpperString[
                    (index + this.rotorPositions[i]) % Alphabet.AlphabetStringLength];
                index = (this.rotors[i].Dial.IndexOf(shiftedChar) - this.rotorPositions[i]
                    + Alphabet.AlphabetStringLength) % Alphabet.AlphabetStringLength;
            }
            currentChar = Alphabet.AlphabetUpperString[index];
            // プラグボードを逆に通過
            currentChar = this.plugboard.PassThrough(currentChar);
            index = Alphabet.AlphabetUpperString.IndexOf(char.ToUpper(currentChar));
            currentChar = this.GetAlphabetChar(alphabetCase, index);
            return currentChar;
        }

        /// <summary>
        /// アルファベットが大文字か小文字かをチェックする
        /// </summary>
        /// <param name="c">文字</param>
        /// <param name="index">アルファベット順の位置</param>
        /// <returns>大文字か小文字かそれ以外か</returns>
        private AlphabetCase CheckAlphabetCase(char c, out int index)
        {
            AlphabetCase alphabetCase = AlphabetCase.Other;
            index = -1;
            // アルファベットが大文字の場合
            if (Alphabet.AlphabetUpperString.Contains(c))
            {
                alphabetCase = AlphabetCase.Upper;
                index = Alphabet.AlphabetUpperString.IndexOf(c);
            }
            // アルファベットが小文字の場合
            else if (Alphabet.AlphabetLowerString.Contains(c))
            {
                alphabetCase = AlphabetCase.Lower;
                index = Alphabet.AlphabetLowerString.IndexOf(c);
            }
            return alphabetCase;
        }

        /// <summary>
        /// 大文字か小文字かを指定してアルファベットの文字を取得する
        /// </summary>
        /// <param name="alphabetCase">大文字か小文字か</param>
        /// <param name="index">文字の場所</param>
        /// <returns></returns>
        private char GetAlphabetChar(AlphabetCase alphabetCase, int index)
        {
            char c = Alphabet.AlphabetUpperString[index];
            // 小文字の場合
            if (alphabetCase == AlphabetCase.Lower)
            {
                c = char.ToLower(c);
            }
            return c;
        }

        /// <summary>
        /// ロータをセットする
        /// </summary>
        /// <param name="rotorNumbers">セットするロータの番号たち</param>
        private void SetRotors(int[] rotorNumbers)
        {
            // ロータ数が正しいか確認
            if (rotorNumbers.Length != Enigma.routorsCount)
            {
                throw new ArgumentException(
                    $"ロータの数は {Enigma.routorsCount} 個固定です。");
            }
            // ロータをセット
            for (int i = 0; i < rotorNumbers.Length; i++)
            {
                // 参照渡しを避けるためにクローンで追加
                this.rotors.Add(fiveRotors[rotorNumbers[i]].Clone());
            }
        }

        /// <summary>
        /// ロータを回転させる
        /// </summary>
        private void RotateRotors()
        {
            bool isMiddleAtNotch = this.rotorPositions[1] == this.rotors[1].Dial.IndexOf(this.rotors[1].Notch);
            bool isRightAtNotch = this.rotorPositions[0] == this.rotors[0].Dial.IndexOf(this.rotors[0].Notch);
            // 右ロータは常に回転する
            this.rotorPositions[0] = ++this.rotorPositions[0] % Alphabet.AlphabetStringLength;
            // 中央ロータのみ右ロータか中央ロータがノッチ位置にある場合に回転する（ダブルステップ）
            if (isRightAtNotch || isMiddleAtNotch)
            {
                this.rotorPositions[1] = ++this.rotorPositions[1] % Alphabet.AlphabetStringLength;
            }
            // 左ロータは中央ロータがノッチ位置にある場合に回転する
            if (isMiddleAtNotch)
            {
                this.rotorPositions[2] = ++this.rotorPositions[2] % Alphabet.AlphabetStringLength;
            }
        }
    }
}
