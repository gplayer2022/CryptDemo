// wwwroot/js/aesCrypto.js
export async function encrypt(keyBytes, plainText) {
    const key = await crypto.subtle.importKey(
        "raw", new Uint8Array(keyBytes), { name: "AES-CBC" }, false, ["encrypt"]);
    const iv = crypto.getRandomValues(new Uint8Array(16));
    const plainBytes = new TextEncoder().encode(plainText);
    const cipherBuffer = await crypto.subtle.encrypt({ name: "AES-CBC", iv }, key, plainBytes);

    const combined = new Uint8Array(iv.length + cipherBuffer.byteLength);
    combined.set(iv, 0);
    combined.set(new Uint8Array(cipherBuffer), iv.length);
    return btoa(String.fromCharCode(...combined));
}

export async function decrypt(keyBytes, base64CipherText) {
    const combined = Uint8Array.from(atob(base64CipherText), c => c.charCodeAt(0));
    const iv = combined.slice(0, 16);
    const cipherBytes = combined.slice(16);
    const key = await crypto.subtle.importKey(
        "raw", new Uint8Array(keyBytes), { name: "AES-CBC" }, false, ["decrypt"]);
    const plainBuffer = await crypto.subtle.decrypt({ name: "AES-CBC", iv }, key, cipherBytes);
    return new TextDecoder().decode(plainBuffer);
}