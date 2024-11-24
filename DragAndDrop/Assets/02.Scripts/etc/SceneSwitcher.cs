using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
public class SceneSwitcher : MonoBehaviour
{
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
}
