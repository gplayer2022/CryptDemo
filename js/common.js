// Unit8Array(byte[]に相当)をバイナリ文字列（Base64変換用）に変換する
// バイト列をブラウザ制約にかからないように展開する
export function bytesToBinaryString(bytes) {
    // 一度に処理するバイト数: 32768バイト
    // 32768。上限(65536)の半分程度に安全マージンを取る
    const chunkSize = 0x8000;
    let binary = "";
    for (let i = 0; i < bytes.length; i += chunkSize) {
        const chunk = bytes.subarray(i, i + chunkSize);
        // 各バイトを1文字として扱うことで、バイナリ文字列を作る
        binary += String.fromCharCode(...chunk);
    }
    return binary;
}
