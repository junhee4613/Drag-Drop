using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Chapter2_general_stage1 : BossController
{
    public Cactus_climb_up cactus_climb_up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Pattern_processing()
    {
        Pattern_function(ref cactus_climb_up.pattern_data, ref cactus_climb_up.pattern_ending, ref cactus_climb_up.duration,ref cactus_climb_up.pattern_count, Cactus_climb_up_pattern);
    }
    public void Cactus_climb_up_pattern()
    {
        switch (cactus_climb_up.pattern_data[cactus_climb_up.pattern_count].action_num)
        {
            case 0:             //������ �ϳ� ����
                Managers.Pool.Pop(Managers.Resource.Load<GameObject>("���⿡ ���߿� ������")).transform;
                break;
            case 1:             //������ �����ϸ鼭 ������
                break;
            case 2:
                break;          //������ �����
        }
    }
    [Serializable]
    public class Cactus_climb_up : Pattern_base_data
    {
        public float speed;         //������ �����̴� �ӵ�
        public float[] cactus_size;     //�����̴� ������ �������
        public int appearance_dir = 0;      //������ �����ϴ� ����
        public GameObject[] moving_cactus;  //�����̴� ������� �־�δ� ��
        public sbyte[] array_num;

    }
}
