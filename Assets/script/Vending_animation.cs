using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimationTrigger : MonoBehaviour
{
    private Animator animator;
    private bool isPlayerNearby = false;
    private bool isActive = false; // アニメーションのON/OFFを管理

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Z))
        {
            isActive = !isActive; // ON/OFFを切り替える
            animator.SetBool("isActive", isActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
