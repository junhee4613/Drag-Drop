using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cha_in_gureumi;
using Newtonsoft.Json;


public class ChaInGureumi : BossController          //��Ʈ�� 80dlek
{
    public Cha_in_gureumi_simple_patterns simple_pattern;
    public Cha_in_gureumi_hard_patterns hard_pattern;
    /*[Header("�� ����")]
    public Rain_drop_pattern rain_drop;*/
    [Header("��ٶ� ����")]
    public Rain_storm_pattern rain_storm;
    [Header("�ҳ��� ����")]
    public Shower_pattern shower;
    [Header("���� ����")]
    public Rush_pattern rush;
    [Header("���� ���� ����")]
    public Lightning_pattern lightning;
    public GameObject camera_obj;
    protected override void Awake()
    {
        base.Awake();
        rain_storm.pattern_data = JsonConvert.DeserializeObject<List<Pattern_state_date>>(Managers.Resource.Load<TextAsset>("Rain_storm_data").text);
        rush.pattern_data = JsonConvert.DeserializeObject<List<Pattern_state_date>>(Managers.Resource.Load<TextAsset>("Cloud_rush_data").text);
        shower.pattern_data = JsonConvert.DeserializeObject<List<Pattern_state_date>>(Managers.Resource.Load<TextAsset>("Shower_data").text);

    }
    public void Start()
    {
        StartCoroutine(Temp_method());
        Anim_state_machin("idle");
    }
    IEnumerator Temp_method()
    {
        while (camera_obj.transform.position.y != 0)
        {
            camera_obj.transform.position = new Vector3(camera_obj.transform.position.x, Mathf.Clamp(camera_obj.transform.position.y + Time.deltaTime * 5, -11, 0), camera_obj.transform.position.z);
            yield return null;
        }
        Managers.GameManager.game_start = true;
    }
    public override void Pattern_processing()
    {
        base.Pattern_processing();
        if (!rain_storm.pattern_ending)
        {
            if ((rain_storm.pattern_data[rain_storm.pattern_count].time <= Managers.Sound.bgSound.time || rain_storm.duration != 0))
            {
                if (rain_storm.duration == 0)
                {
                    rain_storm.duration = rain_storm.pattern_data[rain_storm.pattern_count].duration;
                }
                rain_storm.time -= Time.fixedDeltaTime;
                if (rain_storm.time <= 0)          //���߿� ������ �ٲ�� ������ ���� ����ϱ�
                {
                    rain_storm.time += 0.375f;
                    Rain_storm();
                }
                Rain_storm_rotation();
                rain_storm.duration = Mathf.Clamp(rain_storm.duration - Time.fixedDeltaTime, 0, rain_storm.pattern_data[rain_storm.pattern_count].duration);
                if (rain_storm.duration == 0)
                {
                    rain_storm.pattern_count++;
                    if (rain_storm.pattern_data.Count == rain_storm.pattern_count)
                    {
                        rain_storm.pattern_ending = true;
                    }
                }
            } 
        }
        if (!shower.pattern_ending)
        {
            if ((shower.pattern_data[shower.pattern_count].time <= Managers.Sound.bgSound.time || shower.duration != 0))
            {
                if (shower.duration == 0)
                {
                    shower.duration = shower.pattern_data[shower.pattern_count].duration;
                }
                Shower();
                shower.duration = Mathf.Clamp(shower.duration - Time.fixedDeltaTime, 0, shower.pattern_data[shower.pattern_count].duration);
                if (shower.duration == 0)
                {
                    shower.pattern_count++;
                    if (shower.pattern_data.Count == shower.pattern_count)
                    {
                        Anim_state_machin("idle");
                        //���̵� �ִϸ��̼�
                        shower.shower_obj.SetActive(false);
                        shower.pattern_ending = true;
                        //boss_image.flipX = false;
                    }
                }
            } 
        }
        if (!rush.pattern_ending)
        {
            if(rush.pattern_data[rush.pattern_count].time <= Managers.Sound.bgSound.time || rush.duration != 0)
            {
                if (rush.duration == 0)
                {
                    rush.duration = rush.pattern_data[rush.pattern_count].duration;
                }
                Rush();
                rush.duration = Mathf.Clamp(rush.duration - Time.fixedDeltaTime, 0, rush.pattern_data[rush.pattern_count].duration);
            }
        }
        if (!lightning.pattern_ending)
        {

        }
        

        /*if (lightning.pattenr_data[lightning.pattern_count].time >= Managers.Sound.bgSound.time)
        {

        }
        if (lightning.pattenr_data[lightning.pattern_count].time >= Managers.Sound.bgSound.time)
        {

        }
        if (lightning.pattenr_data[lightning.pattern_count].time >= Managers.Sound.bgSound.time)
        {

        }
        if (lightning.pattenr_data[lightning.pattern_count].time >= Managers.Sound.bgSound.time)
        {

        }*/
    }
    public void Rain_storm_rotation()
    {
        switch (rain_storm.pattern_data[rain_storm.pattern_count].action_num)
        {
            case 0:         //���(�ٶ� ���Ƶ��̴� �ִϸ��̼�)
                if (rain_storm.pos_x_critaria != 0)
                {
                    rain_storm.pos_x_critaria = 0;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 0)
                {
                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.identity, rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            case 1:         //����(���� �������� �δ� �ִϸ��̼�)
                if (rain_storm.pos_x_critaria != rain_storm.pos_x_criteria_option)
                {
                    rain_storm.pos_x_critaria = -rain_storm.pos_x_criteria_option;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 315)
                {

                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.Euler(0, 0, 315), rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            case 2:     //������(���������� �δ� �ִϸ��̼�)
                if (rain_storm.pos_x_critaria != rain_storm.pos_x_criteria_option)
                {
                    rain_storm.pos_x_critaria = rain_storm.pos_x_criteria_option;
                }
                if (rain_storm.rain_trans[rain_storm.rain_trans.Count - 1].rotation.eulerAngles.z != 45)
                {
                    rain_storm.criteria = Quaternion.RotateTowards(rain_storm.criteria, Quaternion.Euler(0, 0, 45), rain_storm.rotation_speed * Time.fixedDeltaTime);
                    foreach (var item in rain_storm.rain_trans)
                    {
                        item.rotation = rain_storm.criteria;
                    }
                }
                break;
            default:
                break;
        }
    }
   
    public void Idle()
    {
        Anim_state_machin(anim_state["simple_pattern0"]);
    }
    public void Rain_storm()
    {
        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Rain_drop"));
        temp.transform.rotation = rain_storm.criteria;
        switch (rain_storm.pattern_data[rain_storm.pattern_count].action_num)
        {
            case 0:         //���(�ٶ� ���Ƶ��̴� �ִϸ��̼�)
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 1:         //����(���� �������� �δ� �ִϸ��̼�)
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria + 2, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 2:     //������(���������� �δ� �ִϸ��̼�)
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria - 2, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            default:
                break;
        }
        if(rain_storm.pos_x_count == rain_storm.pos_x.Length - 1)
        {
            rain_storm.pos_x_count = 0;
        }
        else
        {
            rain_storm.pos_x_count++;
        }
    }
    public void Shower()
    {
        //2~-4.5
        switch (shower.pattern_data[shower.pattern_count].action_num)
        {
            case 0:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(4.75f, transform.position.y), 2 * Time.fixedDeltaTime);
                Anim_state_machin("simple_pattern1");
                if (!shower.shower_obj.activeSelf)
                {
                    shower.shower_obj.SetActive(true);
                }
                break;
            case 1:
                //��� �ִϸ��̼�
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1.75f, transform.position.y), 12 * Time.fixedDeltaTime);
                if(!boss_image.flipX)
                {
                    boss_image.flipX = true;
                }
                break;
            default:
                break;
        }
    }
    public void Rush()
    {
        switch (rush.pattern_data[rush.pattern_count].action_num)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                break;
        }
        /*switch ((rush_pattern.pattenr_data[rush_pattern.pattern_count].action_num))
        {
            case 1:
                for (int i = 0; i < rush_pattern.rush_pos_deciding_count[rush_pattern.rush_pattern_num]; i++)
                {
                    Debug.Log(i);
                    rush_pattern.rush_height.Enqueue(rush_pattern.pos_y[rush_pattern.pos_y_count]);
                    if (rush_pattern.pos_y_count != rush_pattern.pos_y.Length - 1)
                    {
                        rush_pattern.pos_y_count++;
                    }
                }
                if (rush_pattern.rush_pos_deciding_count.Length - 1 != rush_pattern.rush_pattern_num)
                {
                    rush_pattern.rush_pattern_num++;
                }
                rush_pattern.pattern_count++;
                if (rush_pattern.pattenr_data.Count == rush_pattern.pattern_count)
                {
                    rush_pattern.pattern_ending = true;
                    rush_pattern.pattern_starting = false;
                }
                break;
            case 2:
                Debug.Log(rush_pattern.rush_height.Count);
                transform.position = new Vector3( rush_pattern.pos_x * rush_pattern.pos_x_dir[rush_pattern.rush_count], rush_pattern.rush_height.Dequeue(), 0);
                rush_pattern.rush_speed = rush_pattern.rush_speed_option * rush_pattern.pos_x_dir[rush_pattern.rush_count];
                break;
            default:
                break;




                *//*case 0:
                    transform.Translate(new Vector3(0, rush_pattern.up_move_speed * Time.fixedDeltaTime, 0));
                    break;
                case 1:
                    for (int i = 0; i < rush_pattern.rush_pos_deciding_count[rush_pattern.rush_pattern_num]; i++)
                    {
                        rush_pattern.rush_height.Enqueue(rush_pattern.pos_y[rush_pattern.pos_y_count]);
                        if(rush_pattern.pos_y_count != rush_pattern.pos_y.Length - 1)
                        {
                            rush_pattern.pos_y_count++;
                        }
                    }
                    if(rush_pattern.rush_pos_deciding_count.Length - 1 != rush_pattern.rush_pattern_num)
                    {
                        rush_pattern.rush_pattern_num++;
                    }
                    rush_pattern.rush_start = false;
                    break;
                case 2:
                    if (!rush_pattern.rush_start)
                    {
                        rush_pattern.rush_start = true;
                        transform.position = new Vector3(rush_pattern.pos_x_dir[rush_pattern.rush_count], rush_pattern.rush_height.Dequeue(), 0);
                        rush_pattern.rush_speed = rush_pattern.rush_speed_option * rush_pattern.pos_x_dir[rush_pattern.rush_count];
                        if (rush_pattern.rush_count != rush_pattern.pos_x_dir.Length - 1)
                        {
                            rush_pattern.rush_count++;
                        }
                    }
                    transform.Translate(new Vector3(0,rush_pattern.rush_speed * Time.fixedDeltaTime, 0 ));
                    break;
                default:
                    break;*//*
        }*/
    }
    public void Broad_lightning()
    {

    }
    public void Lightning_ball()
    {

    }
    public void Lightning()
    {
       
    }
    [Serializable]
    public class Lightning_pattern : Pattern_base_data
    {
        
    }
    
    [Serializable]
    public class Rain_storm_pattern : Pattern_base_data
    {
        [Header("�����Ǵ� ����")]
        public float pos_y;
        [Header("�����Ǵ� x�� ��ġ��")]
        public float[] pos_x;
        [Header("ȸ���ϴ� �ӵ�")]
        public float rotation_speed;
        [Header("�ٶ� ������ �ٲ� �� �����ؾߵǴ� ��ġ�� �������� ���� ���̳� �������ߵǴ��� �Ҵ�")]
        public float pos_x_criteria_option;
        [HideInInspector]
        public sbyte pos_x_count;
        [HideInInspector]
        public List<Transform> rain_trans = new List<Transform>();
        [HideInInspector]
        public HashSet<string> rain_hash = new HashSet<string>();
        [HideInInspector]
        public Quaternion criteria;
        [HideInInspector]
        public float pos_x_critaria;
        [HideInInspector]
        public float time;
        
    }
    [Serializable]
    public class Shower_pattern : Pattern_base_data
    {
        [Header("�̵� �ӵ�")]
        public float speed;
        [Header("��� ������Ʈ")]
        public GameObject umbrella;
        [Header("��� ������Ʈ ������ �ð�")]
        public float umbrella_on_time;
        [Header("��� ���ӽð�")]
        public float umbrella_duration_time;
        [Header("�ҳ��� ������Ʈ")]
        public GameObject shower_obj;
        
    }
    [Serializable]
    public class Rush_pattern : Pattern_base_data
    {
        [HideInInspector]
        public Queue<float> rush_height = new Queue<float>();
        [Header("ó�� ���� �̵��ϴ� �ӵ�")]
        public float up_move_speed;
        [Header("�����ϴ� ���̵� ���� �� ����")]
        public float[] pos_y;
        [Header("���� ������ �ѹ� ������ �� �������� ��ġ ���� Ƚ��")]
        public sbyte[] rush_pos_deciding_count;
        [Header("�����ϴ� ����(������ = -1, ������ = 1)")]
        public sbyte[] pos_x_dir;
        [Header("���� ��� ��(����� ���� ��)")]
        public float pos_x;
        [Header("���� �ӵ�")]
        public float rush_speed_option = 47.94f;
        [HideInInspector]
        public float rush_speed;
        [HideInInspector]
        public sbyte pos_y_count;
        [HideInInspector]
        public sbyte rush_pattern_num;      //���������� �ѹ� ���� ������ �����ϴ� ����
        [HideInInspector]
        public sbyte rush_count;              //���������� ������ Ƚ��
        public float time;
        /*[HideInInspector]
        public bool rush_start = false;*/

    }
}
