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
    public Fruit_Barrage fruit_barrage;
    public Cactus_thorn cactus_thorn;

    // Start is called before the first frame update
    void Start()
    {
        fruit_barrage.cactus = Managers.Resource.Load<GameObject>("Fruit_cactus");
        Debug.Log(fruit_barrage.cactus.name);
        for (int i = 0; i < fruit_barrage.cactus.transform.childCount; i++)
        {
            for (int j = 0; j < fruit_barrage.cactus.transform.GetChild(i).childCount; j++)
            {
                fruit_barrage.fruit_spawner_pos[] = fruit_barrage.cactus.transform.GetChild(i).GetChild(j).gameObject;
                Debug.Log(fruit_barrage.cactus.transform.GetChild(i).GetChild(j).gameObject.name);
            }
        }
    }

    // Update is called once per frame
    
    public override void Pattern_processing()
    {
        Pattern_function(ref cactus_climb_up.pattern_data, ref cactus_climb_up.pattern_ending, ref cactus_climb_up.duration,ref cactus_climb_up.pattern_count, Cactus_climb_up_pattern);
        Pattern_function(ref fruit_barrage.pattern_data, ref fruit_barrage.pattern_ending, ref fruit_barrage.duration,ref fruit_barrage.pattern_count, Fruit_barrage_pattern);
        if(fruit_barrage.fruit == null/*���⿡ ���Ű� �� �ڸ��� �� ���ڸ��� �ִ��� Ȯ��*/)
        {
            fruit_barrage.fruit = Managers.Resource.Load<GameObject>("������ ����");
        }
        fruit_barrage.fruit.transform.position = fruit_barrage.fruit_spawner_pos[Random.Range(0, fruit_barrage.fruit_spawner_pos.Length)].transform.position;
        Managers.Pool.Pop(fruit_barrage.fruit);
    }
    void Cactus_climb_up_pattern()
    {
        switch (cactus_climb_up.pattern_data[cactus_climb_up.pattern_count].action_num)
        {
            case 0:             //������ �ϳ� ���� �� �� ����
                cactus_climb_up.index_num = (sbyte)Random.Range(1, 4);
                general_warning_obj[general_warning_obj.Count] = General_warning_box(new Vector3(cactus_climb_up.cactus_size[cactus_climb_up.index_num], 0.1f, 0), new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0), warning_color);
                break;
            case 1:
                //������ ������ �ϳ� ��ȯ�ؼ� ���� �̵���Ű��
                general_warning_obj[general_warning_obj.Count].SetActive(false);
                general_warning_obj.RemoveAt(0);
                GameObject cactus_obj = Managers.Resource.Load<GameObject>("���⿡ ���߿� ������");
                cactus_obj.transform.position = new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0);
                cactus_obj.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
                {
                    cactus_obj.transform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                    {
                        //������ �� �κ� dotween���� ȿ�� �� �� �� ȿ���� ������ List �ȿ� �ֱ�
                        cactus_obj.transform.DOMoveY(8, cactus_climb_up.speed).SetEase(Ease.Linear).OnComplete(() => 
                        {
                            cactus_obj.transform.localScale = Vector2.zero;
                            //���⿡ �ȵ� ������ �� �������ִ� ���� �־��ֱ�
                            Managers.Pool.Push(cactus_obj);
                        });
                    });
                });
                break;
        }
    }
    void Fruit_barrage_pattern()
    {
        switch (fruit_barrage.pattern_data[fruit_barrage.pattern_count].action_num)
        {
            case 0:             //������ ����

                fruit_barrage.cactus = Managers.Resource.Load<GameObject>("ź�� ������");
                fruit_barrage.cactus.transform.position = new Vector3(0, 0, 0);
                fruit_barrage.cactus.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                Managers.Pool.Pop(fruit_barrage.cactus);
                fruit_barrage.cactus.transform.DOScaleY(10, 0).OnComplete(() =>
                {
                    fruit_barrage.cactus.transform.DOScaleY(10, 0);
                });
                break;
            case 1:     //���� ����Ʈ��
                fruit_barrage.random_num = Random.Range(0, fruit_barrage.fully_grown.Count);

                if (fruit_barrage.fully_grown[fruit_barrage.random_num].TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.gravityScale = 1;
                    fruit_barrage.fully_grown.Remove(rb.gameObject);
                }
                break;
            case 2:
                // ���� ����
                Managers.Pool.Push(fruit_barrage.fully_grown[fruit_barrage.random_num]);
                fruit_barrage.fully_grown.RemoveAt(fruit_barrage.random_num);
                //���⿡ ź�� �����ϴ� ���� �߰�
                break;
            case 3:     //������ �����
                fruit_barrage.cactus.transform.DOScaleY(10, 0).OnComplete(() =>
                {
                    fruit_barrage.cactus.transform.DOScaleY(10, 0);
                });
                break;
        }
    }
    void Cactus_thorn_pattern()
    {

    }
    [Serializable]
    public class Cactus_climb_up : Pattern_base_data
    {
        public float speed;         //������ �����̴� �ӵ�
        public float[] cactus_size;     //�����̴� ������ �������
        public int appearance_dir = 0;      //������ �����ϴ� ����
        public int appearance_height = 0;      //������ �����ϴ� ����
        public sbyte index_num;             //���° �������� �����ð��� �� ����
    }
    [Serializable]
    public class Fruit_Barrage : Pattern_base_data 
    {
        public List<GameObject> fruit_spawner_pos = new List<GameObject>();
        public float barrage_bullet_num;
        public GameObject cactus;
        public List<GameObject> fully_grown = new List<GameObject>();
        public int random_num;
        public GameObject fruit;
    }
    [Serializable]
    public class Cactus_thorn : Pattern_base_data
    {

    }

}
