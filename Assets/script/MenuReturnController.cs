using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuReturnController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            string previousScene = SceneHistory.GetPreviousScene();
            if (!string.IsNullOrEmpty(previousScene))
            {
                StartCoroutine(ReturnToPreviousScene());
            }
            else
            {
                Debug.LogWarning("戻り先のシーンが登録されていません！");
            }
        }
    }

    private System.Collections.IEnumerator ReturnToPreviousScene()
    {
        string sceneName = SceneHistory.GetPreviousScene();
        Vector3 pos = SceneHistory.GetPreviousPosition();

        // シーンをロード
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            yield return null;
        }

        // プレイヤーを探して位置復元
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = pos;
        }
        else
        {
            Debug.LogWarning("プレイヤーが見つかりませんでした。Tagが 'Player' になっているか確認してください。");
        }
    }
}
