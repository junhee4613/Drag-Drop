using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;
public class Managers : MonoBehaviour           //����� �� �� �Ű������� ���� �Ҵ�ƴ����� ���� ���θ� ������ �����1�� ȣ�⽺�� â�� ������� �ߴ����� ���� �� �� �ڵ带 �Ҵ� ���Ѿߵ�
{
    public bool developer_mode = false;
    public bool invincibility = false;
    static Managers _instance;
    public static Managers instance { get { Init(); return _instance; } }
    private void Awake()
    {
        Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if(count == totalCount)
            {
                Debug.Log(Resource._resources.Count);

                UI_jun.Init();
                Sound.mixer = Resource.Load<AudioMixer>("Sound_option.mixer");
                SceneManager.sceneLoaded += GameManager.Next_sceneLoaded;
                SceneManager.sceneLoaded += Sound.OnSceneLoaded;
                SceneManager.sceneLoaded += UI_jun.UI_on_scene_loaded;
                Sound.bgSound = gameObject.GetOrAddComponent<AudioSource>();
                GameManager.scene_name = SceneManager.GetActiveScene().name;
            }
            //������� �ϸ� �� 
            /*Debug.Log("key : " + key + " Count : " + count + " totalCount : " + totalCount);
            if (count == totalCount)
            {
                Managers.Data.Init();
                Managers.Game.Init();
                Init();
            }*/
        });

        GameManager.gameover += Game_system_stop;


    }
    private void Start()
    {
        
    }
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

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();
        }
    }
    public void Update()        
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.scene_name != "Lobby_screen")       //FIX : ���߿� ���� �����ؾߵ�
        {
            if (UI_jun.UI_window_on["Option"].activeSelf)
            {
                GameManager.game_stop = false;
                UI_jun.UI_window_off.Peek().SetActive(false);
                if(GameManager.scene_name != "Main_screen")
                {
                    Sound.bgSound.pitch = 1;
                }
                Time.timeScale = 1;
            }
            else
            {
                GameManager.game_stop = true;
                UI_jun.UI_window_on["Option"].SetActive(true);
                Time.timeScale = 0;
                if (GameManager.scene_name != "Main_screen")
                {
                    Sound.bgSound.pitch = 0;
                }
                UI_jun.UI_window_off.Push(UI_jun.UI_window_on["Option"]);

            }
            //StartCoroutine(Option_window());
        }
    }
    /*IEnumerator Option_window()
    {

        yield return null;
    }*/
    public void Game_system_stop()
    {
        Time.timeScale = 0;
        Sound.bgSound.pitch = 0;
    }

    
    public static SoundManager Sound { get { return instance?._sound; } }
    public static UIManager_jun UI_jun { get { return instance?._ui; } }
    //public static UIManager UI_base { get { return instance?._ui_base; } }
    public static GameManager GameManager { get { return instance?._game; } }
    public static ResourceManager Resource { get { return instance?._resources; } }
    public static PoolManager Pool { get { return instance?._pool; } }

    SoundManager _sound = new SoundManager();
    UIManager_jun _ui = new UIManager_jun();
    //UIManager _ui_base = new UIManager();
    ResourceManager _resources = new ResourceManager();
    GameManager _game = new GameManager();
    PoolManager _pool = new PoolManager();
}
