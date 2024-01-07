using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    static Managers instance { get { Init(); return _instance; } }
    public static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);  //Scene �� ����ǵ� �ı� ���� �ʰ� 
            _instance = go.GetComponent<Managers>();
        }
    }
    public static GameManager GameManager { get { return instance?._game; } }

    public GameManager _game = new GameManager();
}
