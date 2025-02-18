using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using Newtonsoft.Json;

public class Chapter2_general_stage1 : BossController
{
    public Cactus_climb_up cactus_climb_up;
    public List<GameObject> general_warning_obj = new List<GameObject>();
    public Color warning_color;
    public Fruit_Barrage fruit_barrage;
    public Cactus_thorn cactus_thorn;

    // Start is called before the first frame update
    void Start()
    {
        cactus_climb_up.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Chapter2_general1_cactus_climb_up_data").text);
        fruit_barrage.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Chapter2_general1_fruit_cactus_data").text);
        cactus_thorn.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Chapter2_general1_cactus_thorn_data").text);
        Managers.GameManager.game_start = true;
    }
    // Update is called once per frame

    public override void Pattern_processing()
    {
        Pattern_function(ref cactus_climb_up.pattern_data, ref cactus_climb_up.pattern_ending, ref cactus_climb_up.duration,ref cactus_climb_up.pattern_count, Cactus_climb_up_pattern);
        Pattern_function(ref fruit_barrage.pattern_data, ref fruit_barrage.pattern_ending, ref fruit_barrage.duration,ref fruit_barrage.pattern_count, Fruit_barrage_pattern);
        if (!fruit_barrage.pattern_ending && fruit_barrage.fruit_spawner_pos.Count != 0 && fruit_barrage.cactus_grow_end)
        {
            Fruit_spawner();
        }
        Pattern_function(ref cactus_thorn.pattern_data, ref cactus_thorn.pattern_ending, ref cactus_thorn.duration, ref cactus_thorn.pattern_count, Cactus_thorn_pattern);
    }

    void Cactus_climb_up_pattern()
    {
        switch (cactus_climb_up.pattern_data[cactus_climb_up.pattern_count].action_num)
        {
            case 0:
                cactus_climb_up.appearance_dir = Random.Range(0, 2) == 0 ? -1 : 1;
                cactus_climb_up.index_num = (sbyte)Random.Range(1, 8);
                general_warning_obj.Add(General_warning_box(new Vector3(cactus_climb_up.cactus_size[cactus_climb_up.index_num- 1], 0.1f, 0), new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0), warning_color));
                break;
            case 1:
                general_warning_obj[general_warning_obj.Count - 1].SetActive(false);
                GameObject cactus_obj = Managers.Pool.Pop(Managers.Resource.Load<GameObject>($"Cactus_{cactus_climb_up.index_num}"));
                cactus_obj.transform.position = new Vector3(9 * cactus_climb_up.appearance_dir, cactus_climb_up.appearance_height, 0);
                cactus_obj.transform.localScale = new Vector3(1, 1 * cactus_climb_up.appearance_dir, 1);
                cactus_obj.transform.DOScale(new Vector3(1,1 * cactus_climb_up.appearance_dir, 0) * 1.1f, 0.2f).OnComplete(() =>
                {
                    cactus_obj.transform.DOScale(new Vector3(1, 1 * cactus_climb_up.appearance_dir, 0), 0.2f).OnComplete(() =>
                    {
                        cactus_obj.transform.DOMoveY(8 * ( Random.Range(0, 2) == 0 ? -1 : 1), cactus_climb_up.speed).SetEase(Ease.Linear).OnComplete(() => 
                        {
                            cactus_obj.transform.localScale = new Vector3(1, 0, 0);
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
            case 0:             //선인장 등장
                fruit_barrage.cactus = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Fruit_cactus"));
                fruit_barrage.cactus.transform.position = new Vector3(0, 7, 0);
                fruit_barrage.cactus.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                fruit_barrage.cactus.transform.DOScaleY(1.2f, 0.3f).OnComplete(() =>
                {
                    fruit_barrage.cactus.transform.DOScaleY(1, 0.1f).OnComplete(() => 
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < fruit_barrage.cactus.transform.GetChild(i).childCount; j++)
                            {
                                fruit_barrage.fruit_spawner_pos.Add(fruit_barrage.cactus.transform.GetChild(i).GetChild(j).gameObject.transform.position, fruit_barrage.cactus.transform.GetChild(i).GetChild(j).gameObject.transform);
                                fruit_barrage.available_pos.Add(fruit_barrage.cactus.transform.GetChild(i).GetChild(j).gameObject.transform.position);
                            }
                        }
                        fruit_barrage.cactus_grow_end = true;
                    });
                });
                break;
            case 1:     //열매 떨어트림
                fruit_barrage.random_num = Random.Range(0, fruit_barrage.fully_grown.Count);
                if (fruit_barrage.fully_grown[fruit_barrage.random_num].TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    //위치를 다시 넣어줘야됨
                    fruit_barrage.available_pos.Add(fruit_barrage.fully_grown[fruit_barrage.random_num].transform.position);
                    fruit_barrage.dop_fruit = rb;
                    fruit_barrage.dop_fruit.gravityScale = 1;
                }
                break;
            case 2:
                // 열매 터짐
                GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Projectile_spawner"));
                temp.transform.position = fruit_barrage.fully_grown[fruit_barrage.random_num].transform.position;
                temp.transform.rotation = fruit_barrage.fully_grown[fruit_barrage.random_num].transform.rotation;
                temp.GetOrAddComponent<Projectile_spawner>().Init(fruit_barrage.barrage_bullet_num, 1, 0, fruit_barrage.projectile_speed, 15, warning_color,
                    Managers.Resource.Load<GameObject>("Circle"), Spawner_mode.REPEAT_END, Projectile_moving_mode.GENERAL, temp.transform.position, temp.transform.rotation.eulerAngles, Vector3.one * 0.7f, Vector3.zero);
                Managers.Main_camera.Shake_move();
                fruit_barrage.dop_fruit.gravityScale = 0;
                fruit_barrage.fully_grown[fruit_barrage.random_num].transform.localScale = Vector3.zero;
                Managers.Pool.Push(fruit_barrage.fully_grown[fruit_barrage.random_num]);
                fruit_barrage.fully_grown.RemoveAt(fruit_barrage.random_num);
                break;
            case 3:     //선인장 사라짐
                fruit_barrage.cactus.transform.DOScaleY(1.2f, 0.1f).OnComplete(() =>
                {
                    fruit_barrage.cactus.transform.DOScaleY(0, 0.2f).OnComplete(() =>
                    {
                        Managers.Pool.Push(fruit_barrage.cactus);
                    });
                });
                break;
        }
    }
    void Fruit_spawner()
    {
        if (fruit_barrage.time >= fruit_barrage.set_grow_time)
        {
            fruit_barrage.set_grow_time = Random.Range(1, 3);
            fruit_barrage.time = 0;
            for (int i = 0; i < Random.Range(2, 4); i++)
            {
                if (fruit_barrage.available_pos.Count != 0)
                {
                    fruit_barrage.random_pos_num = Random.Range(0, fruit_barrage.available_pos.Count);
                    GameObject fruit = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Fruit"));
                    if (fruit.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                    {
                        rb.gravityScale = 0;
                    }
                    fruit.transform.parent = fruit_barrage.fruit_spawner_pos[fruit_barrage.available_pos[fruit_barrage.random_pos_num]];
                    fruit.transform.localPosition = Vector3.zero;
                    fruit.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    fruit_barrage.available_pos.RemoveAt(fruit_barrage.random_pos_num);
                    fruit.transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
                    {
                        fruit_barrage.fully_grown.Add(fruit);
                    }); 
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            fruit_barrage.time += Time.deltaTime;
        }
    }
    void Cactus_thorn_pattern()
    {
        switch (cactus_thorn.pattern_data[cactus_thorn.pattern_count].action_num)
        {
            case 0:
                //선인장 생성 및 가시 생성
                cactus_thorn.cactus = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Cactus_thorn")).transform;
                cactus_thorn.cactus.position = cactus_thorn.cactus_pos;
                cactus_thorn.cactus.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
                {
                    cactus_thorn.cactus.DOScale(Vector3.one, 0.2f);
                });
                break;
            case 1:     //쏘고난 후 가시 생성 1
                if(cactus_thorn.thorn_barrage1_pos == null)
                {
                    cactus_thorn.thorn_barrage1_pos = cactus_thorn.cactus.transform.GetChild(1);
                    cactus_thorn.thorn_barrage1_pos.gameObject.SetActive(true);
                    for (int i = 0; i < cactus_thorn.thorn_barrage1_pos.childCount; i++)
                    {
                        cactus_thorn.thorn_barrage_1.Add(cactus_thorn.thorn_barrage1_pos.GetChild(i));
                    }
                }
                foreach (var item in cactus_thorn.thorn_barrage_1)
                {
                    item.GetChild(0).DOLocalMoveY(0.13f, 0.2f);
                }
                break;
            case 2: //쏘고 난 후 가시 생성 2
                if (cactus_thorn.thorn_barrage1_pos == null)
                {
                    cactus_thorn.thorn_barrage1_pos = cactus_thorn.cactus.transform.GetChild(1);
                    cactus_thorn.thorn_barrage1_pos.gameObject.SetActive(true);
                    for (int i = 0; i < cactus_thorn.thorn_barrage1_pos.childCount; i++)
                    {
                        cactus_thorn.thorn_barrage_1.Add(cactus_thorn.thorn_barrage1_pos.GetChild(i));
                    }
                }
                foreach (var item in cactus_thorn.thorn_barrage_1)
                {
                    item.GetChild(0).DOLocalMoveY(0.13f, 0.2f);
                }
                if (cactus_thorn.thorn_barrage2_pos == null)
                {
                    cactus_thorn.thorn_barrage2_pos = cactus_thorn.cactus.transform.GetChild(2);
                    cactus_thorn.thorn_barrage2_pos.gameObject.SetActive(true);
                    for (int i = 0; i < cactus_thorn.thorn_barrage2_pos.childCount; i++)
                    {
                        cactus_thorn.thorn_barrage_2.Add(cactus_thorn.thorn_barrage2_pos.GetChild(i));
                    }
                }
                foreach (var item in cactus_thorn.thorn_barrage_2)
                {
                    item.GetChild(0).DOLocalMoveY(0.13f, 0.2f);
                }
                break;
            case 3:     //가시 발사
                if(cactus_thorn.thorn_barrage_2.Count != 0)
                {
                    foreach (var item in cactus_thorn.thorn_barrage_2)
                    {
                        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Projectile_spawner"));
                        temp.transform.position = item.transform.position;
                        temp.transform.rotation = item.transform.rotation;
                        temp.GetOrAddComponent<Projectile_spawner>().Init(1, 1, 0, cactus_thorn.thorn_speed, 7, Color.white,
                            Managers.Resource.Load<GameObject>("Thorn_projectile"), Spawner_mode.REPEAT_END, Projectile_moving_mode.GUIDED_Y, temp.transform.position, temp.transform.rotation.eulerAngles, Vector3.one * 2, 30, temp.transform.eulerAngles);
                        item.GetChild(0).localPosition = new Vector3(0, -0.1f, 0);
                    }
                }
                if (cactus_thorn.thorn_barrage_1.Count != 0)
                {
                    foreach (var item in cactus_thorn.thorn_barrage_1)
                    {
                        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Projectile_spawner"));
                        temp.transform.position = item.transform.position;
                        temp.transform.rotation = item.transform.rotation;
                        temp.GetOrAddComponent<Projectile_spawner>().Init(1, 1, 0, cactus_thorn.thorn_speed, 7, Color.white,
                            Managers.Resource.Load<GameObject>("Thorn_projectile"), Spawner_mode.REPEAT_END, Projectile_moving_mode.GUIDED_Y, temp.transform.position, temp.transform.rotation.eulerAngles, Vector3.one * 2, 45, temp.transform.eulerAngles);
                        item.GetChild(0).localPosition = new Vector3(0, -0.1f, 0);
                    }
                }
                cactus_thorn.cactus.DOPunchScale(new Vector3(0.1f, 0.1f), 0.1f);
                break;
            case 4:     //가시 제거
                foreach (var item in cactus_thorn.thorn_barrage_1)
                {
                    item.GetChild(0).DOLocalMoveY(-0.1f, 0.2f);
                }
                foreach (var item in cactus_thorn.thorn_barrage_2)
                {
                    item.GetChild(0).DOLocalMoveY(-0.1f, 0.2f);
                }
                break;
            case 5:     //선인장 제거
                cactus_thorn.cactus.DOMoveY(-5f, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => 
                {
                    cactus_thorn.cactus.DOMoveY(-9.5f, 0.4f).SetEase(Ease.InQuad);
                });
                break;
        }
    }
    [Serializable]
    public class Cactus_climb_up : Pattern_base_data
    {
        public float speed;         //선인장 움직이는 속도
        public float[] cactus_size;     //움직이는 선인장 사이즈들
        public int appearance_dir = 0;      //선인장 등장하는 방향
        public int appearance_height = 0;      //선인장 등장하는 높이
        public sbyte index_num;             //몇번째 선인장을 가져올건지 고를 변수
        public Transform cactus;
    }
    [Serializable]
    public class Fruit_Barrage : Pattern_base_data 
    {
        public Dictionary<Vector3, Transform> fruit_spawner_pos = new Dictionary<Vector3, Transform>();
        public int barrage_bullet_num;
        public float projectile_speed;
        public GameObject cactus;
        public List<GameObject> fully_grown = new List<GameObject>();
        public int random_num;
        public int set_grow_time = 0;
        public float time = 0;
        public int random_pos_num;
        public Rigidbody2D dop_fruit;
        public bool cactus_grow_end = false;
        public List<Vector3> available_pos = new List<Vector3>();
    }
    [Serializable]
    public class Cactus_thorn : Pattern_base_data
    {
        public List<Transform> thorn_barrage_2 = new List<Transform>();
        public List<Transform> thorn_barrage_1 = new List<Transform>();
        public Transform thorn_barrage1_pos;
        public Transform thorn_barrage2_pos;
        public Transform cactus;
        public Vector3 cactus_pos;
        public float thorn_speed = 5;
    }
}
