using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorStartInit
{
    static EditorStartInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path; // 씬 번호를 넣어주자.
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}