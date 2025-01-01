using UnityEditor;
using UnityEditor.SceneManagement;

# if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class EditorStartInit
{
# if UNITY_EDITOR
    static EditorStartInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path; // �� ��ȣ�� �־�����.
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
#endif
}