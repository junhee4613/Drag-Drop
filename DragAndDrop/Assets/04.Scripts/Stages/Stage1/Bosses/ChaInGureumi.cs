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
    public Transform boss_trans;
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
    [Header("���� ��ü ����")]
    public Electric_ball_pattern electric_ball_pattern;
    [Header("���� ��ü ȸ��")]
    public Electric_ball_rotation_pattern electric_ball_rotation_pattern;
    
    protected override void Awake()
    {
        base.Awake();
        rain_storm.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_rain_storm_data").text);
        rush.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Cloud_rush_data").text);
        shower.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_shower_data").text);
        lightning.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_lightning_data").text);
        phase_2.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage_1_2_phase_start_data").text);
        electric_ball_rotation_pattern.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_electric_ball_rotation_data").text);
        electric_ball_pattern.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage1_Electric_ball_pattern_data").text);
        
    }
    public void Start()
    {
        Anim_state_machin(anim_state["1_phase_idle"]);
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
                if (rain_storm.time <= 0)
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
                        Anim_state_machin(anim_state["1_phase_idle"]);
                        shower.shower_obj.SetActive(false);
                        shower.pattern_ending = true;
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
                Anim_state_machin(anim_state["2_phase_idle"]);
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
        if (!electric_ball_pattern.pattern_ending)
        {
            if ((electric_ball_pattern.pattern_data[electric_ball_pattern.pattern_count].time <= Managers.Sound.bgSound.time || electric_ball_pattern.duration != 0))
            {
                if (electric_ball_pattern.duration == 0)
                {
                    electric_ball_pattern.duration = electric_ball_pattern.pattern_data[electric_ball_pattern.pattern_count].duration;
                }
                Electric_ball();
                electric_ball_pattern.duration = Mathf.Clamp(electric_ball_pattern.duration - Time.fixedDeltaTime, 0, electric_ball_pattern.pattern_data[electric_ball_pattern.pattern_count].duration);
                if (electric_ball_pattern.duration == 0)
                {
                    electric_ball_pattern.pattern_count++;
                    if (electric_ball_pattern.pattern_data.Count == electric_ball_pattern.pattern_count)
                    {
                        //���̵� �ִϸ��̼�
                        electric_ball_pattern.pattern_ending = true;
                    }
                }
            }

        }
        if (!electric_ball_rotation_pattern.pattern_ending)
        {
            if ((electric_ball_rotation_pattern.pattern_data[electric_ball_rotation_pattern.pattern_count].time <= Managers.Sound.bgSound.time || electric_ball_rotation_pattern.duration != 0))
            {
                if (electric_ball_rotation_pattern.duration == 0)
                {
                    electric_ball_rotation_pattern.duration = electric_ball_rotation_pattern.pattern_data[electric_ball_rotation_pattern.pattern_count].duration;
                }
                Electric_ball_rotation();
                electric_ball_rotation_pattern.duration = Mathf.Clamp(electric_ball_rotation_pattern.duration - Time.fixedDeltaTime, 0, electric_ball_rotation_pattern.pattern_data[electric_ball_rotation_pattern.pattern_count].duration);
                if (electric_ball_rotation_pattern.duration == 0)
                {
                    electric_ball_rotation_pattern.pattern_count++;
                    if (electric_ball_rotation_pattern.pattern_data.Count == electric_ball_rotation_pattern.pattern_count)
                    {
                        //���̵� �ִϸ��̼�
                        electric_ball_rotation_pattern.pattern_ending = true;
                    }
                }
            }

        }

    }
    public void Rain_storm_rotation()
    {
        switch (rain_storm.pattern_data[rain_storm.pattern_count].action_num)
        {
            case 0:         //���(�ٶ� ���Ƶ��̴� �ִϸ��̼�)
                Anim_state_machin(anim_state["simple_pattern2"]);
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
                Anim_state_machin(anim_state["simple_pattern3"]);
                if (boss_trans.transform.localScale.x != -1)
                {
                    boss_trans.transform.localScale = new Vector3(-1f, 1f, 0f);
                }
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
                Anim_state_machin(anim_state["simple_pattern3"]);
                if (boss_trans.transform.localScale.x != 1)
                {
                    boss_trans.transform.localScale = new Vector3(1f, 1f, 0f);
                }
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
            case 3:
                Anim_state_machin(anim_state["simple_pattern3"]);
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
            case 4:
                Anim_state_machin(anim_state["2_phase_idle"]);
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
            case 3:
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria, rain_storm.pos_y, 0);
                if (!rain_storm.rain_hash.Contains(temp.GetInstanceID().ToString()))
                {
                    rain_storm.rain_hash.Add(temp.GetInstanceID().ToString());
                    rain_storm.rain_trans.Add(temp.transform);
                }
                break;
            case 4:
                temp.transform.position = new Vector3(rain_storm.pos_x[rain_storm.pos_x_count] + rain_storm.pos_x_critaria, rain_storm.pos_y, 0);
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
    public void Warning_box(Vector3 size, Vector3 pos, sbyte count, float minute)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = size;
        warning_box.GetComponent<SpriteRenderer>().DOFade(0, minute).SetLoops(count, LoopType.Yoyo);
    }
    public void Shower()
    {
        //2~-4.5
        switch (shower.pattern_data[shower.pattern_count].action_num)
        {
            case 0:
                //���� �ڽ� ����
                Anim_state_machin(anim_state["simple_pattern1"]);
                Warning_box(new Vector3(7, 4, 0), new Vector3(0.5f, -2, 0), 3, 0.25f);
                break;
            case 1:
                //��鼭 �÷��̾� �ڽ� �������� �̵�
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(3.9f, transform.position.y), 2.5f * Time.fixedDeltaTime);
                Anim_state_machin(anim_state["simple_pattern1"]);
                if (!shower.shower_obj.activeSelf && transform.position.x == 3.9f)
                {
                    shower.shower_obj.SetActive(true);
                }
                break;
            case 2:
                Anim_state_machin(anim_state["simple_pattern0"]);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1.75f, transform.position.y), 12 * Time.fixedDeltaTime);
                if (!boss_image.flipX)
                {
                    boss_image.flipX = true;
                }
                //������ ��ġ���� ���� �� �ҳ��� �����
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
    public void Electric_ball_rotation()
    {
        switch (electric_ball_rotation_pattern.pattern_data[electric_ball_rotation_pattern.pattern_count].action_num)
        {
            case 0:
                electric_ball_rotation_pattern.lightning_ball.Rotate(0, 0, electric_ball_rotation_pattern.electric_bullet_rotation_speed * Time.fixedDeltaTime);
                break;
            case 1:
                electric_ball_rotation_pattern.lightning_ball.Rotate(0, 0, electric_ball_rotation_pattern.electric_bullet_reverse_rotation_speed * Time.fixedDeltaTime);
                break;
            case 2:
                electric_ball_rotation_pattern.lightning_ball.Rotate(0, 0, -electric_ball_rotation_pattern.electric_bullet_rotation_speed * Time.fixedDeltaTime);
                break;
            default:
                break;
        }
    }
    public void Electric_ball()
    {
        

        switch (electric_ball_pattern.pattern_data[electric_ball_pattern.pattern_count].action_num)
        {
            case 0:
                Sequence sequence = DOTween.Sequence();
                electric_ball_pattern.electric_ball_obj.SetActive(true);
                sequence.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one * 1.5f, 0.5f));
                sequence.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one, 0.5f));
                break;
            case 1:
                Sequence sequence1 = DOTween.Sequence();
                sequence1.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one * 0.5f, 0.1f));
                sequence1.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one, 0.1f)).OnComplete(() => 
                {
                    electric_ball_pattern.muzzle.SetActive(true);
                });
                break;
            case 2:
                foreach (var item in electric_ball_pattern.muzzles_trans)
                {
                    GameObject bullet = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Electric_bullet"));
                    Electric_bullet_obj electric_bullet = bullet.GetComponent<Electric_bullet_obj>();
                    if (electric_bullet == null)
                    {
                        electric_bullet = bullet.AddComponent<Electric_bullet_obj>();
                        electric_bullet.bullet_speed = electric_ball_pattern.speed;
                        electric_bullet.bullet_push_time = electric_ball_pattern.push_time;

                    }
                    bullet.transform.position = item.position;
                    bullet.transform.rotation = item.rotation;
                }
                break;
            case 3:
                Sequence sequence2 = DOTween.Sequence();

                sequence2.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one * 0.5f, 0.1f)).OnComplete(() =>
                {
                    electric_ball_pattern.muzzle.SetActive(false);
                }); ;
                sequence2.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one, 0.1f));
                //���ӽð� 0.5��
                break;
            case 4:
                electric_ball_pattern.electric_line.SetActive(true);

                /*sequence.Append(electric_ball_pattern.electric_line.transform.DOScale(Vector3.one * 0.5f, 0.1f)).OnComplete(() =>
                {
                }); ;
                sequence.Append(electric_ball_pattern.electric_ball_obj.transform.DOScale(Vector3.one, 0.1f));*/
                break;
            case 5:
                electric_ball_pattern.ball.DOScale(transform.localScale + Vector3.one * 0.1f, 0.2f);
                break;
            case 6:
                electric_ball_pattern.electric_line.SetActive(false);
                break;
            case 7:
                electric_ball_pattern.electric_ball_obj.SetActive(false);
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
            Anim_state_machin(anim_state["hard_pattern1"]);
                break;
            case 3:
            Anim_state_machin(anim_state["2_phase_idle"]);
                Managers.Main_camera.Moving();
                break;
            default:
                break;
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
    public class Electric_ball_pattern : Pattern_base_data
    {
        public GameObject electric_ball_obj;
        public Transform ball;
        public GameObject muzzle;
        public GameObject electric_line;
        public Transform[] muzzles_trans;
        public float speed;
        public float push_time;
    }
    [Serializable]
    public class Electric_ball_rotation_pattern : Pattern_base_data
    {
        
        public Transform lightning_ball;
        [Header("����ź ���� ���� �ð���� �ӵ�")]
        public float electric_bullet_rotation_speed;
        [Header("����ź ���� ���� �ݽð���� �ӵ�")]
        public float electric_bullet_reverse_rotation_speed;
    }
    [Serializable]
    public class Electric_line_controller : Pattern_base_data
    {

    }
}
