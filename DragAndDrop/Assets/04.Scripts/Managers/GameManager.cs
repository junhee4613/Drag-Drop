using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour        //���⼭ ��Ʈ�� ����
{
    PlayerController player;
    public PlayerController Player 
    { 
        get { 
            if (player == null)
            {
                player = GameObject.FindObjectOfType<PlayerController>();
                if(player == null)
                {
                    Debug.LogError("�÷��̾ ����");
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
    public float audio_clip_length;
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

}
