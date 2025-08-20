using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 5f;
    public float chaseSpeed = 3f;       // 追いかけるときの速さ
    public float returnSpeed = 1.5f;    // 元の位置に戻るときの速さ
    public float stopChaseRange = 7f;

    private Vector2 moveDirection;
    private Vector3 startPosition;
    private bool isCollidingWithPlayer = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (player == null || isCollidingWithPlayer) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < chaseRange)
        {
            ChasePlayer();
        }
        else if (distance > stopChaseRange)
        {
            ReturnToStart();
        }
    }

    private void ChasePlayer()
    {
        Vector2 diff = player.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            moveDirection = new Vector2(Mathf.Sign(diff.x), 0);
        else
            moveDirection = new Vector2(0, Mathf.Sign(diff.y));

        transform.position += (Vector3)moveDirection * chaseSpeed * Time.deltaTime;
    }

    private void ReturnToStart()
    {
        Vector2 diff = startPosition - transform.position;

        if (diff.magnitude > 0.1f)
        {
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                moveDirection = new Vector2(Mathf.Sign(diff.x), 0);
            else
                moveDirection = new Vector2(0, Mathf.Sign(diff.y));

            transform.position += (Vector3)moveDirection * returnSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = startPosition;
            moveDirection = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }
}


