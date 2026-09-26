// AESで暗号化
export async function encrypt(keyBytes, plainText) {
    // crypto.subtle: ブラウザ標準の Crypto API
    // importKey(): 生のバイト列を暗号処理で利用可能な CryptoKey オブジェクトに変換する
    //   "row": 鍵の形式。未加工のバイト列。他に "pkcs8" や "jwk"（JSON形式）
    //   new Unit8Array(keyBytes): Blazor からは `number[]` のような形で渡されがち。`byte[]` 相当のものに変換
    //   { name: "AES-CBC", }: アルゴリズムの指定
    //   false: 鍵を後から取り出せるようにするかどうか
    //   ["encrypt", ]: この鍵で許可する陽とのリスト。ここでは暗号化のみ
    const cryptoKey = await crypto.subtle.importKey(
        "raw", new Uint8Array(keyBytes), { name: "AES-CBC", }, false, ["encrypt",]);
    // 初期化ベクトル作成
    // crypto.getRandomValues(): 暗号強度のある乱数生成器
    // AESのブロックサイズと同じ16バイト
    const iv = crypto.getRandomValues(new Uint8Array(16));
    // 文字列をUTF-8のバイト列に変換
    const plainTextBytes = new TextEncoder().encode(plainText);
    // 実際の暗号化（ArrayBuffer型）
    const cipherBuffer = await crypto.subtle.encrypt(
        { name: "AES-CBC", iv: iv, }, cryptoKey, plainTextBytes);
    // IVと暗号文を連結するための領域をUnit8Array型として確保
    const combined = new Uint8Array(iv.length + cipherBuffer.byteLength);
    // IVの先頭（0）からIVの16バイトをコピー
    combined.set(iv, 0);
    // IVの直後から暗号文のバイト列をコピー
    combined.set(new Uint8Array(cipherBuffer), iv.length);
    // Base64に変換して返す
    return btoa(bytesToBinaryString(combined));
}

// バイト列をブラウザ制約にかからないように展開する
function bytesToBinaryString(bytes) {
    // 32768。上限(65536)の半分程度に安全マージンを取る
    const chunkSize = 0x8000;
    let binary = "";
    for (let i = 0; i < bytes.length; i += chunkSize) {
        const chunk = bytes.subarray(i, i + chunkSize);
        binary += String.fromCharCode(...chunk);
    }
    return binary;
}

// AESで復号化
export async function decrypt(keyBytes, base64CipherText) {
    // Uint8Array.from(): 文字列からUnit8Arrayを組み立てる
    //   atob(): Base64文字列をバイナリ文字列に変換する
    //   c => c.charCodeAt(0): 各文字を文字コード（0~255）に変換する
    const combined = Uint8Array.from(atob(base64CipherText), c => c.charCodeAt(0));
    const iv = combined.slice(0, 16);
    const cipherBytes = combined.slice(16);
    const cryptoKey = await crypto.subtle.importKey(
        "raw", new Uint8Array(keyBytes), { name: "AES-CBC", }, false, ["decrypt", ]);
    const plainBuffer = await crypto.subtle.decrypt(
        { name: "AES-CBC", iv: iv, }, cryptoKey, cipherBytes);
    // UTF-8のバイト列を文字列（UTF-16）に戻す
    // new TextDecoder().decode(): new TextEncoder().encode() の逆
    return new TextDecoder().decode(plainBuffer);
}