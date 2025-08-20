using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTextOnInteract : MonoBehaviour
{
    public TextMeshProUGUI interactText; // UIのテキスト
    private bool isNearObject = false;   // オブジェクトに近づいているか
    private bool hasInteracted = false;  // Zキーが押されたか
    private Animator objectAnimator;     // オブジェクトのアニメーター

    void Start()
    {
        interactText.gameObject.SetActive(false); // 最初は非表示
        objectAnimator = GetComponent<Animator>(); // アニメーターを取得
    }

    void Update()
    {
        // プレイヤーがオブジェクトに近づいてZキーが押されたらテキスト表示
        if (isNearObject && Input.GetKeyDown(KeyCode.Z))
        {
            if (hasInteracted)
            {
                interactText.gameObject.SetActive(false); // すでに表示されていたら非表示にする
                hasInteracted = false; // フラグリセット
            }
            else
            {
                interactText.gameObject.SetActive(true);  // テキスト表示
                hasInteracted = true; // フラグ更新

                // フェードアウトアニメーションを再生
                objectAnimator.SetTrigger("kirakira_destroy");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーがオブジェクトに近づいたら
        {
            isNearObject = true; // プレイヤーがオブジェクトに近づいた
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーがオブジェクトから離れたら
        {
            isNearObject = false; // プレイヤーがオブジェクトから離れた
            interactText.gameObject.SetActive(false); // テキストを非表示にする
            hasInteracted = false; // フラグリセット
        }
    }
}


