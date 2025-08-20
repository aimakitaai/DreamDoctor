using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyDialogue : MonoBehaviour
{
    public GameObject dialogueBox;         // UIのダイアログ枠
    public TextMeshProUGUI dialogueText;   // テキスト表示用
    public string[] messages;              // 表示するセリフ（複数行）

    private bool isDialogueOpen = false;
    private bool isTyping = false;
    private int messageIndex = 0;

    private PlayerMovement playerMovement; // プレイヤーの動きを制御する参照

    private void Start()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (isDialogueOpen && Input.GetKeyDown(KeyCode.Z))
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                ShowNextMessage();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isDialogueOpen)
        {
            playerMovement = collision.collider.GetComponent<PlayerMovement>();
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(true);

        messageIndex = 0;
        isDialogueOpen = true;
        StartCoroutine(TypeText(messages[messageIndex]));

        // プレイヤーを停止させる
        if (playerMovement != null)
            playerMovement.isTalking = true;
    }

    private void ShowNextMessage()
    {
        messageIndex++;
        if (messageIndex < messages.Length)
        {
            StartCoroutine(TypeText(messages[messageIndex]));
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        dialogueText.text = "";
        isDialogueOpen = false;

        // プレイヤーの動きを再開
        if (playerMovement != null)
            playerMovement.isTalking = false;

        // シーン遷移を呼び出す
        SceneTransition st = FindObjectOfType<SceneTransition>();
        if (st != null)
        {
            st.ChangeScene("Field"); // ←ここを行きたいシーン名に書き換え
        }
    }


    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    private void SkipTyping()
    {
        StopAllCoroutines();
        dialogueText.text = messages[messageIndex];
        isTyping = false;
    }
}
