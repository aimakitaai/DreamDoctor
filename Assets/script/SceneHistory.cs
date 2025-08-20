using UnityEngine;

public static class SceneHistory
{
    private static string previousScene = ""; // 直前のシーン
    private static Vector3 previousPosition;  // プレイヤーの位置

    public static void SetPreviousScene(string sceneName, Vector3 position)
    {
        previousScene = sceneName;
        previousPosition = position;
    }

    public static string GetPreviousScene()
    {
        return previousScene;
    }

    public static Vector3 GetPreviousPosition()
    {
        return previousPosition;
    }
}
