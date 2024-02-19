using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cha_in_gureumi;
using Newtonsoft.Json;
using DG.Tweening;


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
    [Header("2������ ����")]
    public Phase_2_pattern phase_2;
    [Header("���� ��ü")]
    public Lightning_ball_controller lightning_ball_controller;
    [Header("���� źȯ")]
    public Electric_bullet_controller electric_bullet_controller;
    protected override void Awake()
    {
        base.Awake();
        rain_storm.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Rain_storm_data").text);
        rush.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Cloud_rush_data").text);
        shower.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Shower_data").text);
        lightning.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Lightning_data").text);
        phase_2.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage_1_2_phase_start").text);
        lightning_ball_controller.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Lightning_ball_controller_data").text);
        electric_bullet_controller.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Electric_bullet_data").text);
    }
    public void Start()
    {
        Anim_state_machin("1_phase_idle");
        Managers.GameManager.game_start = true;
    }
    /* IEnumerator Temp_method()
     {
         while (Managers.Main_camera.transform.position.y != 0)
         {
             Managers.Main_camera.transform.position = new Vector3(Managers.Main_camera.transform.position.x, Mathf.Clamp(Managers.Main_camera.transform.position.y + Time.deltaTime * 5, -11, 0), Managers.Main_camera.transform.position.z);
             yield return null;
         }
        Managers.GameManager.game_start = true;
     } */
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
                        Anim_state_machin("1_phase_idle");
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
            if(lightning.pattern_data[lightning.pattern_count].time <= Managers.Sound.bgSound.time)
            {
                Anim_state_machin("2_phase_idle");
                Lightning();
            }
        }
        if (!phase_2.pattern_ending)
        {
            if ((phase_2.pattern_data[phase_2.pattern_count].time <= Managers.Sound.bgSound.time || phase_2.duration != 0))
            {
                if (phase_2.duration == 0)
                {
                    phase_2.duration = phase_2.pattern_data[phase_2.pattern_count].duration;
                }
                Phase_2();
                phase_2.duration = Mathf.Clamp(phase_2.duration - Time.fixedDeltaTime, 0, phase_2.pattern_data[phase_2.pattern_count].duration);
                if (phase_2.duration == 0)
                {
                    phase_2.pattern_count++;
                    if (phase_2.pattern_data.Count == phase_2.pattern_count)
                    {
                        //���̵� �ִϸ��̼�
                        phase_2.pattern_ending = true;
                    }
                }
            }
                
        }
        if (!lightning_ball_controller.pattern_ending)
        {
            if ((lightning_ball_controller.pattern_data[lightning_ball_controller.pattern_count].time <= Managers.Sound.bgSound.time || lightning_ball_controller.duration != 0))
            {
                if (lightning_ball_controller.duration == 0)
                {
                    lightning_ball_controller.duration = lightning_ball_controller.pattern_data[lightning_ball_controller.pattern_count].duration;
                }
                Lightning_ball();
                lightning_ball_controller.duration = Mathf.Clamp(lightning_ball_controller.duration - Time.fixedDeltaTime, 0, lightning_ball_controller.pattern_data[lightning_ball_controller.pattern_count].duration);
                if (lightning_ball_controller.duration == 0)
                {
                    lightning_ball_controller.pattern_count++;
                    if (lightning_ball_controller.pattern_data.Count == lightning_ball_controller.pattern_count)
                    {
                        //���̵� �ִϸ��̼�
                        lightning_ball_controller.pattern_ending = true;
                    }
                }
            }

        }
        if (!electric_bullet_controller.pattern_ending)
        {
            if ((electric_bullet_controller.pattern_data[electric_bullet_controller.pattern_count].time <= Managers.Sound.bgSound.time))
            {

                Electric_bullet();
                electric_bullet_controller.pattern_count++;
                if (electric_bullet_controller.pattern_data.Count == electric_bullet_controller.pattern_count)
                {
                    electric_bullet_controller.pattern_ending = true;
                }
            }

        }
        
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
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria + 4f, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 2:     //������(���������� �δ� �ִϸ��̼�)
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria - 4f, rain_storm.pos_y, 0);
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
    }
    public void Lightning_ball()
    {
        switch (lightning_ball_controller.pattern_data[lightning_ball_controller.pattern_count].action_num)
        {
            case 0:
                lightning_ball_controller.lightning_ball.SetActive(true);
                Sequence sequence = DOTween.Sequence();
                sequence.Append(lightning_ball_controller.lightning_ball.transform.DOScale(Vector3.one * 1.5f, 0.5f));
                sequence.Append(lightning_ball_controller.lightning_ball.transform.DOScale(Vector3.one, 0.5f));
                break;
            case 1:
                lightning_ball_controller.lightning_ball.transform.Rotate(0, 0, lightning_ball_controller.electric_bullet_rotation_speed *Time.fixedDeltaTime);
                break;
            case 2:
                lightning_ball_controller.lightning_ball.transform.Rotate(0, 0, lightning_ball_controller.electric_bullet_reverse_rotation_speed * Time.fixedDeltaTime);
                break;
            case 3:
                //���ӽð� 0.5��
                break;
            default:
                break;
        }
    }
    public void Lightning()
    {
        switch (lightning.pattern_data[lightning.pattern_count].action_num)
        {
            case 0:
                GameObject single_lightning = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Single_lightning"));
                single_lightning.transform.position = new Vector3(lightning.single_pos_x[lightning.single_count], 0.5f, 0);
                if(lightning.single_count != lightning.single_pos_x.Length - 1)
                {
                    lightning.single_count++;
                }
                break;
            case 1:
                GameObject board_lightning = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Broad_lightning"));
                board_lightning.transform.position = new Vector3(lightning.board_pos_x[lightning.board_count], 0.5f, 0);
                if (lightning.board_count != lightning.board_pos_x.Length - 1)
                {
                    lightning.board_count++;
                }
                break;
            default:
                break;
        }
        lightning.pattern_count++;
        if(lightning.pattern_data.Count == lightning.pattern_count)
        {
            lightning.pattern_ending = true;
        }
    }
    public void Phase_2()
    {
        switch (phase_2.pattern_data[phase_2.pattern_count].action_num)
        {
            case 0:
                transform.position = new Vector3(1.5f, 8, 0);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - Time.fixedDeltaTime * phase_2.speed, 2.7f, 8), 0);
                break;
            case 2:
                Anim_state_machin("hard_pattern1");
                break;
            case 3:
                Anim_state_machin("2_phase_idle");
                break;
            default:
                break;
        }
    }
    public void Electric_bullet()
    {
        foreach (var item in electric_bullet_controller.muzzles)
        {
            GameObject bullet = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Electric_bullet"));
            bullet.GetOrAddComponent<Electric_bullet_obj>().bullet_push_time = electric_bullet_controller.push_time;
            bullet.GetOrAddComponent<Electric_bullet_obj>().bullet_speed = electric_bullet_controller.speed;
            bullet.transform.position = item.position;
            bullet.transform.rotation = item.rotation;
        }
    }
    [Serializable]
    public class Lightning_pattern : Pattern_base_data
    {
        [Header("���� ���� x �� ������� �Ҵ�")]
        public float[] single_pos_x;
        [Header("������ ���� x �� ������� �Ҵ�")]
        public float[] board_pos_x;
        [HideInInspector]
        public sbyte single_count;
        [HideInInspector]
        public sbyte board_count;
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
    [Serializable]
    public class Phase_2_pattern : Pattern_base_data
    {
        public float speed = 2;
    }
    [Serializable]
    public class Lightning_ball_controller : Pattern_base_data
    {
        public GameObject lightning_ball;
        [Header("����ź ���� ���� �ð���� �ӵ�")]
        public float electric_bullet_rotation_speed;
        [Header("����ź ���� ���� �ݽð���� �ӵ�")]
        public float electric_bullet_reverse_rotation_speed;
        [Header("�������� �� ���ư��� ȸ�� �ӵ�")]
        public float electric_line_rotation_speed;
    }
    [Serializable]
    public class Electric_bullet_controller : Pattern_base_data
    {
        public Transform[] muzzles;
        public float speed;
        public float push_time;
    }
    [Serializable]
    public class Electric_line_controller : Pattern_base_data
    {

    }
}
