using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // メニュー画面のUI
    public PlayerMovement playerMovement; // プレイヤーの移動スクリプト
    public TextMeshProUGUI[] menuItems; // メニュー項目のリスト
    public Color selectedColor = Color.yellow; // 選択中の項目の色
    public Color defaultColor = Color.white; // 通常時の色

    private bool isMenuOpen = false;
    private int selectedIndex = 0;

    void Start()
    {
        // メニュー項目がちゃんとセットされているか確認
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i] == null)
            {
                Debug.LogError($"menuItems[{i}] がセットされていません！");
            }
        }
    }


    void Update()
    {
        // 会話中ならメニューを開けない
        if (FindObjectOfType<NPCDialogue>()?.IsDialogueOpen() == true)
        {
            return;
        }

        // Xキーを押したらメニューの開閉
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleMenu();
        }

        if (isMenuOpen)
        {
            HandleMenuNavigation();
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            // メニューが開いたらプレイヤーの移動を止める
            playerMovement.enabled = false;
            Time.timeScale = 0f; // ゲーム時間を止める
        }
        else
        {
            // メニューを閉じたらプレイヤーの移動を復活
            playerMovement.enabled = true;
            Time.timeScale = 1f; // ゲーム時間を再開
        }
    }

    void HandleMenuNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
            UpdateMenuSelection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % menuItems.Length;
            UpdateMenuSelection();
        }
        else if (Input.GetKeyDown(KeyCode.Z)) // Zキーで決定
        {
            SelectMenuItem();
        }
    }

    void UpdateMenuSelection()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].color = (i == selectedIndex) ? selectedColor : defaultColor;
        }
    }

    void SelectMenuItem()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("シラベノートを開く");
                var player = GameObject.FindGameObjectWithTag("Player"); // プレイヤー取得
                SceneHistory.SetPreviousScene(
                    SceneManager.GetActiveScene().name,
                    player.transform.position // 現在位置
                );
                Time.timeScale = 1f;
                SceneManager.LoadScene("ShirabeNote");
                break;

            case 1:
                Debug.Log("アイテム画面を開く");
                break;
        }
    }

    public bool IsMenuOpen()
    {
        return isMenuOpen;
    }
}
