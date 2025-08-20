using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshProの名前空間

public class TextAnimation : MonoBehaviour
{
    public TextMeshProUGUI text;    // UIのTextMeshProUGUI
    public float typingSpeed = 0.1f; // 文字の表示速度（秒）

    private string fullText = "こんにちは！これが一文字ずつ表示されるテキストです。";

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        text.text = ""; // 最初は空文字にして、テキストをリセット

        foreach (char letter in fullText) // 全ての文字を一文字ずつ表示
        {
            text.text += letter; // 1文字ずつ追加
            yield return new WaitForSeconds(typingSpeed); // 指定した時間だけ待機
        }
    }
}

