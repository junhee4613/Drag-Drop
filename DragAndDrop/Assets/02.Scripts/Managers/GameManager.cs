using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager
{
    PlayerController player;
    Transform player_character;
    Transform playerTr;
    GameObject beat_box;
    Transform portals;

    Portal[] portalsObj;

    public string last_stage = "Chapter2_boss_stage";
    public bool splash = false;             //todo : 스플레시 화면일 때 쓰는 불값 - 2024.11.18 현재 로딩된 scene이 어떤 scene인지 런타임에 알 수 있게 되어 이건 필요 없어짐
    public string scene_name;               //todo : 여기 enum타입으로 변경
    public SceneName sceneName = SceneName.Lobby;
    public bool load_end = false;
    public bool option_window_on = false;
    public Dictionary<string, bool> stage_clear = new Dictionary<string, bool>()
    { { "Tutorial_stage", false },  {"Chapter1_boss_stage", false}, { "Chapter2_boss_stage", false }, { "Chapter2_general_stage1", false }};
    #region 프로퍼티 함수들

    public Portal[] PortalsObj
    {
        get
        {
            portalsObj = GameObject.FindObjectsOfType<Portal>();

            return portalsObj;
        }
    }
    public PlayerController Player 
    { 
        get { 
            if (player == null)
            {
                player = GameObject.FindObjectOfType<PlayerController>();
            } 
            return player; 
        }
    }
    public Transform PlayerTr
    {
        get
        {
            playerTr = GameObject.FindObjectOfType<PlayerController>().transform;

            return playerTr;
        }
    }
    public Transform Player_character
    {
        get
        {
            if(player_character == null)
            {
                player_character = GameObject.FindGameObjectWithTag("Player_character").transform;
            }
            return player_character;
        }
    }
    public GameObject Beat_box
    {
        get
        {
            if (beat_box == null)
            {
                beat_box = GameObject.FindGameObjectWithTag("Beat_box");
            }
            return beat_box;
        }
    }
    public Transform Portals
    {
        get
        {
            if (portals == null)
            {
                portals = GameObject.Find("Portals").gameObject.transform;
            }
            return portals;
        }
    }
    #endregion
    public bool player_die = false;
    public Action gameover;
    public bool game_start = false;
    public bool game_stop = false;
    public bool tutorial = false;
    public bool tutorial_hit = false;
    public Vector3 InitPos = new Vector3(-18, -2, 0);
    //List<Transform> portals = new List<Transform>();
    public int clear_stage_count = 0;              //스테이지 클리어 시 1이 증가, 0번째 게임 오브젝트부터 건들기 위해 -1부터 시작

    public bool operate = false;
    public bool player_move = false;
    public bool base_tutorial_end = false;
    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene_name = scene.name;
        if (!string.IsNullOrEmpty(scene_name))
        {
            Sound_init(scene_name);
        }
        if (scene_name == "Tutorial_stage")
        {
            operate = false;
        }
        else
        {
            operate = true;
        }

        if (sceneName == SceneName.Main)
        {
            PlayerTr.position = InitPos;
            Managers.Main_camera.Main_camera.transform.position = Managers.Main_camera.camera_pos;
            foreach (var portal in PortalsObj)
            {
                portal.Setting();
            }
            
        }

        UI_init();
    }
    public void Sound_init(string scene_name)
    {
        if (scene_name == "Main_screen")
        {
            Managers.Sound.BGMSound(Managers.Resource.Load<AudioClip>(scene_name), true);

        }
        else if (scene_name != "Tutorial_stage")
        {
            Managers.Sound.BGMSound(Managers.Resource.Load<AudioClip>(scene_name), false);
        }
        else
        {
            Managers.Sound.bgSound.Stop();
        }
    }
    public void UI_init()
    {
        Managers.UI_jun.option_window_on = false;           //FIX : 이거 왜 있는지 모르겠음 
        if (Managers.UI_jun.UI_window_on["Game_over"].activeSelf)
        {
            Managers.UI_jun.UI_window_on["Game_over"].SetActive(false);
        }
        if (Managers.UI_jun.UI_window_off.Count != 0)
        {
            for (int i = 0; i < Managers.UI_jun.UI_window_off.Count; i++)
            {
                Managers.UI_jun.UI_window_off.Peek().SetActive(false);
            }
        }
    }

    public void stage_clear_init()
    {
        InitGameMode();
        Managers.GameManager.InitPos = new Vector3(-18, -2, 0);
        Managers.Main_camera.camera_pos= new Vector3(-18, 0, -10);

        Managers.UI_jun.SetButtonStatus();

    }

    public void InitGameMode()
    {
        if (!Managers.instance.tutorial_skip)
        {
            Managers.GameManager.stage_clear["Tutorial_stage"] = false;
            Managers.GameManager.operate = false;
            Managers.GameManager.player_move = false;
            Managers.GameManager.base_tutorial_end = false;
        }
        Managers.GameManager.stage_clear["Chapter1_boss_stage"] = false;
        Managers.GameManager.stage_clear["Chapter2_boss_stage"] = false;
        Managers.GameManager.stage_clear["Chapter2_general_stage1"] = false;

        clear_stage_count = 0;

        splash = false;
    }
    public void Setting_main_stage()
    {
        if (Managers.GameManager.clear_stage_count >= 1)
            return;

        Player.transform.position = InitPos;
        Managers.Main_camera.Main_camera.transform.position = Managers.Main_camera.camera_pos;
    }
}
