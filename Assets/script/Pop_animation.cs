using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTriggerAnimation : MonoBehaviour
{
    private Animator anim;
    private bool isPlayerNearby = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーが近づいたら
        {
            anim.SetBool("Activate", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーが離れたら
        {
            anim.SetBool("Activate", false);
        }
    }
}
