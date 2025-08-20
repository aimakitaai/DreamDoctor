using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public Vector2 minLimit; // カメラの最小範囲
    public Vector2 maxLimit; // カメラの最大範囲
    public float smoothing = 0.1f; // 追従のスムージング値

    private Vector3 offset; // カメラのオフセット

    void Start()
    {
        // プレイヤーとのオフセットを計算
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // プレイヤーの位置にオフセットを加えた新しいカメラ位置を計算
        Vector3 targetPosition = player.position + offset;

        // X, Y の移動範囲を制限
        float clampedX = Mathf.Clamp(targetPosition.x, minLimit.x, maxLimit.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minLimit.y, maxLimit.y);

        // 制限された位置にカメラをスムーズに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, transform.position.z), smoothing);
        transform.position = smoothedPosition;
    }
}
