import { bytesToBinaryString, } from './common.js';

// 鍵ペアを生成する
export async function generateKeyPair() {
    // 鍵ペアを作成する
    //   RSA、鍵長 2024bit 、公開指数 65537 、ハッシュアルゴリズム SHA-256
    //   new Uint8Array([1, 0, 1, ]) : byte[] に相当。公開鍵指数 e を指定（ 0x010001: 65537 ）
    //   第2引数: true => exportKey() で鍵を取り出せるようにする
    //   第3引数: 鍵ペアに ["encrypt", "decrypt", ] のそれぞれ役割を与える
    //       keyPair.publicKey => encrypt
    //       keyPair.privateKey => decrypt
    const keyPair = await crypto.subtle.generateKey(
        { name: "RSA-OAEP", modulusLength: 2048, publicExponent: new Uint8Array([1, 0, 1, ]), hash: "SHA-256", },
        true,  
        ["encrypt", "decrypt", ]
    );
    // JWK(JSON Web Key)形式で書き出し、文字列化してC#側にそのまま渡せるようにする
    // jwk 以外に、公開鍵なら spki で、秘密鍵なら pkcs8 で取り出せる
    const publicKeyJwk = await crypto.subtle.exportKey("jwk", keyPair.publicKey);
    const privateKeyJwk = await crypto.subtle.exportKey("jwk", keyPair.privateKey);
    // JSON文字列として返す
    return {
        publicKey: JSON.stringify(publicKeyJwk),
        privateKey: JSON.stringify(privateKeyJwk),
    };
}

// 公開鍵で暗号化する
export async function encrypt(publicKeyJwkText, plainText) {
    // exportKey の逆
    //   第4引数 false: importした鍵はexport不可にする
    //   第5引数 [ "encrypt", ]: 鍵には暗号化用途のみ持たせる
    const key = await crypto.subtle.importKey(
        "jwk", JSON.parse(publicKeyJwkText),
        { name: "RSA-OAEP", hash: "SHA-256", }, false, ["encrypt",]);
    // 平文をバイト列にする
    const plainBytes = new TextEncoder().encode(plainText);
    // 暗号化
    const cipherBuffer = await crypto.subtle.encrypt({ name: "RSA-OAEP" }, key, plainBytes);
    // 暗号文をBase64にして返す
    return btoa(bytesToBinaryString(new Uint8Array(cipherBuffer)));
}

// 秘密鍵で復号する
export async function decrypt(privateKeyJwkText, base64CipherText) {
    // 秘密鍵をCryptoKeyに戻す
    const key = await crypto.subtle.importKey(
        "jwk", JSON.parse(privateKeyJwkText),
        { name: "RSA-OAEP", hash: "SHA-256", }, false, ["decrypt",]);
    // Base64の暗号文からUnit8Array(byte[]に相当)のバイト列に変換
    const cipherBytes = Uint8Array.from(atob(base64CipherText), c => c.charCodeAt(0));
    // ArrayBuffer型の平文に復号
    const plainBuffer = await crypto.subtle.decrypt({ name: "RSA-OAEP", }, key, cipherBytes);
    // UTF-8バイト列から文字列に変換して返す
    return new TextDecoder().decode(plainBuffer);
}
