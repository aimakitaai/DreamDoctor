using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI参照")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public GameObject choiceBox;
    public TextMeshProUGUI choiceYesText;
    public TextMeshProUGUI choiceNoText;

    [Header("会話テキスト")]
    public string[] startMessages;
    public string[] yesBranchMessages;
    public string[] noBranchMessages;

    [Header("設定")]
    public bool hasChoices = true; // 選択肢モードかどうか
    public string yesSceneName;    // YESで移動するシーン名
    public GameObject illustrationObject;

    private string[] currentMessages;
    private bool isPlayerNearby = false;
    private bool isDialogueOpen = false;
    private bool isTyping = false;
    private bool isIllustrationPhase = false;
    private bool isChoicePhase = false;
    private bool hasChosen = false;

    private int messageIndex = 0;
    private bool choiceIsYes = true;

    private PlayerMovement playerMovement;
    private NPCGridMovement npcMovement;
    private MenuController menuController;

    void Start()
    {
        dialogueBox.SetActive(false);
        choiceBox.SetActive(false);
        if (illustrationObject != null)
            illustrationObject.SetActive(false);

        menuController = FindObjectOfType<MenuController>();
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Z) && (menuController == null || !menuController.IsMenuOpen()))
        {
            if (!isDialogueOpen)
            {
                OpenDialogue();
            }
            else if (isTyping)
            {
                SkipTyping();
            }
            else if (isChoicePhase)
            {
                ConfirmChoice();
            }
            else if (isIllustrationPhase)
            {
                EndDialogue();
            }
            else
            {
                ShowNextMessage();
            }
        }

        if (isChoicePhase && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            ToggleChoice();
        }
    }

    private void OpenDialogue()
    {
        dialogueBox.SetActive(true);
        currentMessages = startMessages;
        messageIndex = 0;
        isDialogueOpen = true;
        isIllustrationPhase = false;
        isChoicePhase = false;
        hasChosen = false;

        StartCoroutine(TypeText(currentMessages[messageIndex]));

        if (playerMovement != null) playerMovement.isTalking = true;
        if (npcMovement != null) npcMovement.isTalking = true;
    }

    private void ShowNextMessage()
    {
        messageIndex++;
        if (messageIndex < currentMessages.Length)
        {
            StartCoroutine(TypeText(currentMessages[messageIndex]));
        }
        else
        {
            if (hasChoices && currentMessages == startMessages && !hasChosen)
            {
                // 選択肢あり → 選択フェーズへ
                StartChoicePhase();
            }
            else
            {
                // 選択肢なし or 分岐後 → イラストまたは終了
                StartIllustrationPhase();
            }
        }
    }

    private void StartChoicePhase()
    {
        isChoicePhase = true;
        choiceIsYes = true;
        choiceBox.SetActive(true);
        UpdateChoiceUI();
    }

    private void ToggleChoice()
    {
        choiceIsYes = !choiceIsYes;
        UpdateChoiceUI();
    }

    private void UpdateChoiceUI()
    {
        choiceYesText.color = choiceIsYes ? Color.yellow : Color.white;
        choiceNoText.color = !choiceIsYes ? Color.yellow : Color.white;
    }

    private void ConfirmChoice()
    {
        isChoicePhase = false;
        choiceBox.SetActive(false);
        hasChosen = true;
        messageIndex = 0;

        if (choiceIsYes)
        {
            if (!string.IsNullOrEmpty(yesSceneName))
            {
                SceneManager.LoadScene(yesSceneName);
                return;
            }
            else if (yesBranchMessages.Length > 0)
            {
                currentMessages = yesBranchMessages;
            }
            else
            {
                StartIllustrationPhase();
                return;
            }
        }
        else
        {
            if (noBranchMessages.Length > 0)
            {
                currentMessages = noBranchMessages;
            }
            else
            {
                StartIllustrationPhase();
                return;
            }
        }

        StartCoroutine(TypeText(currentMessages[messageIndex]));
    }

    private void StartIllustrationPhase()
    {
        dialogueText.text = "";

        if (illustrationObject != null)
        {
            illustrationObject.SetActive(true);
            isIllustrationPhase = true;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        if (illustrationObject != null)
            illustrationObject.SetActive(false);

        dialogueBox.SetActive(false);
        dialogueText.text = "";
        isDialogueOpen = false;
        isIllustrationPhase = false;
        isChoicePhase = false;

        if (playerMovement != null) playerMovement.isTalking = false;
        if (npcMovement != null) npcMovement.isTalking = false;
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
        dialogueText.text = currentMessages[messageIndex];
        isTyping = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerMovement = other.GetComponent<PlayerMovement>();
            npcMovement = GetComponent<NPCGridMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (!isDialogueOpen)
                EndDialogue();
        }
    }

    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}
