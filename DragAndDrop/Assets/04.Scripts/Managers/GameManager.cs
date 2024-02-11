using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour        //여기서 비트를 관리
{
    public List<Pattern_state> pattern_data = new List<Pattern_state>();
    PlayerController player;
    public string scene_name;
    public GameObject player_obj;
    public GameObject player_box;
    public Dictionary<string, bool> stage_clear = new Dictionary<string, bool>() 
    { {"Stage1", true}, { "Stage2", false }, { "Stage3", false }, { "Stage4", false } };

    public PlayerController Player 
    { 
        get { 
            if (player == null)
            {
                player = GameObject.FindObjectOfType<PlayerController>();
                if(player == null)
                {
                    Managers.Resource.Load<GameObject>("Player");
                }
            } 
            return player; 
        }
    }
    public GameObject boss;
    public bool boss_die = false;
    public bool player_die = false;
    public Action gameover;
    public float beat;
    public float bgm_length;        //음악 진행 시간
    public bool game_start = false;
    public sbyte pattern_num;
    public bool game_stop = false;
    public void Init()
    {
        player_obj = Managers.Resource.Load<GameObject>("Player");
        player_box = Managers.Resource.Load<GameObject>("Player_box");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {

    }
    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene_name = scene.name;
        Managers.Pool.Clear();
        switch (scene_name)
        {
            case "Stage1":
                Stage1();
                break;
            default:
                break;
        }
        if (scene_name.Contains("Stage"))
        {
        }
        Managers.UI_jun.option_window_on = false;           //FIX : 이거 왜 있는지 모르겠음 
    }
    public void Stage1()
    {
        
    }
}
