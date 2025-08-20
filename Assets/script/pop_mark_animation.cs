using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private bool isNearObject = false;   // オブジェクトに近づいているか
    private bool hasInteracted = false;  // 既にアニメーションが切り替わったか
    private Animator objectAnimator;     // オブジェクトのアニメーター

    void Start()
    {
        objectAnimator = GetComponent<Animator>(); // アニメーターを取得
    }

    void Update()
    {
        if (isNearObject && Input.GetKeyDown(KeyCode.Z) && !hasInteracted)
        {
            // Zキーが押されたらアニメーションを切り替え
            objectAnimator.SetTrigger("Destroy"); // アニメーションのトリガーを設定

            hasInteracted = true; // アニメーションが切り替わったフラグを立てる
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
        }
    }
}
