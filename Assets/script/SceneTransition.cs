using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image fadePanel; // フェード用のUI画像
    public float fadeSpeed = 1f; // フェード速度
    private bool isFading = false; // フェード中フラグ

    void OnEnable()
    {
        fadePanel.color = new Color(0, 0, 0, 1); // 最初は真っ黒
        StartCoroutine(FadeIn()); // シーン開始時にフェードイン
    }

    public void ChangeScene(string sceneName)
    {
        if (!isFading) StartCoroutine(FadeOut(sceneName)); // フェードアウトを1回だけ実行
    }

    IEnumerator FadeIn()
    {
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        isFading = true;
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
