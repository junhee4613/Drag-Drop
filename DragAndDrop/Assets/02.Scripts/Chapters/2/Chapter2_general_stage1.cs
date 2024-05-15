using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class Chapter2_general_stage1 : BossController
{
    public Cactus_climb_up cactus_climb_up;
    List<GameObject> general_warning_obj = new List<GameObject>();
    public Color warning_color;

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
        if(cactus_climb_up.moving_cactus.Count != 0 && cactus_climb_up.pattern_ending)
        {
            for (int i = 0; i < cactus_climb_up.moving_cactus.Count; i++)
            {
                cactus_climb_up.moving_cactus[i].transform.position =
                    new Vector3(cactus_climb_up.moving_cactus[i].transform.position.x, cactus_climb_up.moving_cactus[i].transform.position.y + (Time.deltaTime * cactus_climb_up.speed), 0);
                if(Mathf.Abs(cactus_climb_up.moving_cactus[i].transform.position.y) == 15)
                {
                    Managers.Pool.Push(cactus_climb_up.moving_cactus[i]);
                    cactus_climb_up.moving_cactus.Remove(cactus_climb_up.moving_cactus[i]);
                }
            }
        }
    }
    public void Cactus_climb_up_pattern()
    {
        switch (cactus_climb_up.pattern_data[cactus_climb_up.pattern_count].action_num)
        {
            case 0:             //������ �ϳ� ���� �� �� ����
                //���� ��� ���� �����ѵ�
                cactus_climb_up.index_num = (sbyte)Random.Range(1, 4);
                general_warning_obj[general_warning_obj.Count] = General_warning_box(new Vector3(cactus_climb_up.cactus_size[cactus_climb_up.index_num], 0.1f, 0), new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0), warning_color);
                break;
            case 1:
                general_warning_obj[general_warning_obj.Count].SetActive(false);
                general_warning_obj.RemoveAt(0);
                GameObject cactus_obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("���⿡ ���߿� ������"));
                cactus_obj.transform.position = new Vector3(4 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0);
                cactus_obj.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
                {
                    cactus_obj.transform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                    {
                        //������ �� �κ� dotween���� ȿ�� �� �� �� ȿ���� ������ List �ȿ� �ֱ�
                        if (!cactus_climb_up.moving_cactus.Contains(cactus_obj))
                        {
                            cactus_climb_up.moving_cactus.Add(cactus_obj);
                        }
                    });
                });
                break;
        }
    }
    [Serializable]
    public class Cactus_climb_up : Pattern_base_data
    {
        public float speed;         //������ �����̴� �ӵ�
        public float[] cactus_size;     //�����̴� ������ �������
        public int appearance_dir = 0;      //������ �����ϴ� ����
        public int appearance_height = 0;      //������ �����ϴ� ����
        public List<GameObject> moving_cactus = new List<GameObject>();  //�����̴� ������� �־�δ� ��
        public sbyte index_num;             //���° �������� �����ð��� �� ����

    }
}
