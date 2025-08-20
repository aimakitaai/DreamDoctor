using UnityEngine;
using System.Collections;

public class NPCGridMovement : MonoBehaviour
{
    public float moveDistance = 1.0f; // 1回の移動距離（1マス）
    public float moveSpeed = 5.0f; // 移動速度
    public float waitTime = 1.0f; // 次の移動までの待ち時間

    public Vector2 minBounds; // 移動範囲の最小座標
    public Vector2 maxBounds; // 移動範囲の最大座標

    public LayerMask playerLayer; // プレイヤーのレイヤー

    private Vector2 currentDirection; // 今の移動方向
    private bool isMoving = false;
    public bool isTalking = false; // 会話中かどうか

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left; // 初期方向をランダムに
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (isTalking) // 会話中は動かない
            {
                isMoving = false;
                yield return null;
                continue;
            }

            if (!isMoving)
            {
                // 進行方向にプレイヤーがいる場合は待つ
                if (IsPlayerInPath())
                {
                    yield return new WaitForSeconds(0.2f); // 少し待って再チェック
                    continue;
                }

                isMoving = true;
                Vector2 targetPosition = (Vector2)transform.position + currentDirection * moveDistance;

                // 範囲外なら反転
                if (targetPosition.x < minBounds.x || targetPosition.x > maxBounds.x)
                {
                    ReverseDirection();
                    targetPosition = (Vector2)transform.position + currentDirection * moveDistance;
                }

                SetAnimation();

                yield return StartCoroutine(MoveToPosition(targetPosition));

                yield return new WaitForSeconds(waitTime);
                isMoving = false;
            }
            yield return null;
        }
    }

    IEnumerator MoveToPosition(Vector2 target)
    {
        while ((Vector2)transform.position != target)
        {
            if (isTalking) yield break; // 会話中なら即終了

            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void ReverseDirection()
    {
        currentDirection *= -1; // 左右反転
    }

    void SetAnimation()
    {
        if (isTalking) // 会話中ならアニメーションを止める
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", false);
            return;
        }

        if (currentDirection == Vector2.right)
        {
            animator.SetBool("isMovingRight", true);
            animator.SetBool("isMovingLeft", false);
        }
        else
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", true);
        }
    }

    bool IsPlayerInPath()
    {
        if (isTalking) return false; // 会話中ならチェック不要

        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, moveDistance, playerLayer);
        return hit.collider != null;
    }
}
