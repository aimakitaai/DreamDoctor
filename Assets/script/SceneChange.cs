using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    [Header("移動先シーン名")]
    public string nextSceneName;

    [Header("この出入口のID（プレイヤー初期位置の判別に使用）")]
    public string entranceID;

    private SceneTransition sceneTransition;

    private void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransition>(); // フェードスクリプト取得
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // シーン間データを保存
            SceneTransitionData.lastScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            SceneTransitionData.entranceID = entranceID;

            // フェード付きでシーン移動
            sceneTransition.ChangeScene(nextSceneName);
        }
    }
}
