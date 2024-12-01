using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEditor;
public class SceneSwitcher : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Scenes/Load Lobby")]
    static void LobbyScene()
    {
        LoadScene("Assets/01.Scenes/Base_scene/Lobby_screen.unity");
    }

    static void LoadScene(string scenePath)
    {
        // �� ���� ���� Ȯ�� �� ���
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // ������ �� �ε�
            EditorSceneManager.OpenScene(scenePath);
        }
    }
#endif
}
