using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NovelController : MonoBehaviour
{
    [Header("キャラ表示")]
    public CanvasGroup characterGroup;   // キャラのCanvasGroup
    public float fadeDuration = 1.0f;

    [Header("テキスト表示")]
    public GameObject dialogueBox;       // テキストボックス（UIのパネル）
    public TextMeshProUGUI dialogueText; // テキスト本体
    [TextArea(2, 5)]
    public string[] messages;            // 会話文リスト
    public float typingSpeed = 0.05f;    // 文字送りの速さ

    private bool isCharacterShown = false;
    private bool isDialogueStarted = false;
    private int messageIndex = 0;
    private bool isTyping = false;       // 今打ち込み中かどうか
    private Coroutine typingCoroutine;

    void Start()
    {
        if (characterGroup != null) characterGroup.alpha = 0f;
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isCharacterShown)
            {
                // キャラをフェードイン
                StartCoroutine(FadeInCharacter());
            }
            else if (!isDialogueStarted)
            {
                // テキストボックスを表示開始
                StartDialogue();
            }
            else
            {
                if (isTyping)
                {
                    // 打ち込み中なら全文表示に切り替え
                    StopCoroutine(typingCoroutine);
                    dialogueText.text = messages[messageIndex];
                    isTyping = false;
                }
                else
                {
                    // 次の文章へ
                    ShowNextMessage();
                }
            }
        }
    }

    private IEnumerator FadeInCharacter()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            characterGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        isCharacterShown = true;
    }

    private void StartDialogue()
    {
        dialogueBox.SetActive(true);
        messageIndex = 0;
        typingCoroutine = StartCoroutine(TypeText(messages[messageIndex]));
        isDialogueStarted = true;
    }

    private void ShowNextMessage()
    {
        messageIndex++;
        if (messageIndex < messages.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(messages[messageIndex]));
        }
        else
        {
            // 会話終了
            dialogueBox.SetActive(false);
            isDialogueStarted = false;
        }
    }

    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in message)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}
