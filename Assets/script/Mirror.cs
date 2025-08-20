using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteOnPlayerNear : MonoBehaviour
{
    [Header("変化後のスプライト")]
    public Sprite highlightedSprite;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite; // 元のスプライトを保存
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = highlightedSprite;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}
