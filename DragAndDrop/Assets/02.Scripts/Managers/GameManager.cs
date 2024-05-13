using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager
{
    public List<Pattern_state> pattern_data = new List<Pattern_state>();
    PlayerController player;
    Transform player_character;
    GameObject beat_box;
    public GameObject Beat_box 
    { 
        get
        { 
            if(beat_box == null)
            {
                beat_box = GameObject.FindGameObjectWithTag("Beat_box");
            }
            return beat_box; 
        } 
    }
    public bool splash = false;             //���÷��� ȭ���� �� ���� �Ұ�
    public string scene_name;
    public bool load_end = false;
    public bool option_window_on = false;
    public Dictionary<string, bool> stage_clear = new Dictionary<string, bool>() 
    { {"Chapter1_boss_stage", false}, { "Chapter2_boss_stage", false }, { "Chapter2_general_stage1", false }, { "Stage4", false } };

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

    public GameObject boss;
    public bool boss_die = false;
    public bool player_die = false;
    public Action gameover;
    public float beat;
    public float bgm_length;        //���� ���� �ð�
    public bool game_start = false;
    public sbyte pattern_num;
    public bool game_stop = false;
    public void Init()
    {
    }
    // Start is called before the first frame update
    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip temp = Managers.Resource.Load<AudioClip>(scene.name);
        scene_name = scene.name;
        if (temp != null)
        {
            if(scene.name == "Main_screen")
            {
                Managers.Sound.BGMSound(temp, true);
            }
            else
            {
                Managers.Sound.BGMSound(temp, false);
            }
        }
        Managers.Pool.Clear();
        /*switch (scene_name)
        {
            case "Stage1":
                Stage1();
                break;
            default:
                break;
        }
        if (scene_name.Contains("Stage"))
        {
        }*/
        Sound_init(scene, mode);
        Managers.UI_jun.option_window_on = false;           //FIX : �̰� �� �ִ��� �𸣰��� 
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
    public void Stage1()
    {
        //FIX : ���߿� ���⿡ �÷��̾� �ڽ� ������ �����ϴ� ���� �ֱ�
    }
    public void Sound_init(Scene arg0, LoadSceneMode arg1)
    {
        
    }
}
