using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorStartInit
{
    static EditorStartInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path; // �� ��ȣ�� �־�����.
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}