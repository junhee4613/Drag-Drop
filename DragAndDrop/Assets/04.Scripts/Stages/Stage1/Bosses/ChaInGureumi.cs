using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cha_in_gureumi;


public class ChaInGureumi : BossController          //��Ʈ�� 80dlek
{
    public Cha_in_gureumi_simple_patterns simple_pattern;
    public Cha_in_gureumi_hard_patterns hard_pattern;
    /*[Header("�� ����")]
    public Rain_drop_pattern rain_drop;*/
    [Header("��ٶ� ����")]
    public Rain_storm_pattern rain_storm;
    [Header("���� ����")]
    public Rush_pattern rush_pattern;
    [Header("���� ���� ����")]
    public Lightning_pattern lightning;
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Pattern_processing()
    {
        base.Pattern_processing();
       /* if (rain_drop.pattenr_data[rain_drop.pattern_count].time >= Managers.Sound.bgSound.time && !rain_drop.pattern_ending)
        {
            Rain_drop();
            rain_drop.pattern_count++;
            if(rain_drop.pattenr_data.Count == rain_drop.pattern_count)
            {
                rain_drop.pattern_ending = true;
            }
        }*/
        if (rain_storm.pattenr_data[rain_storm.pattern_count].time >= Managers.Sound.bgSound.time && !rain_storm.pattern_ending)
        {
            Rain_storm();
            rain_storm.pattern_count++;
            if (rain_storm.pattenr_data.Count == rain_storm.pattern_count)
            {
                rain_storm.pattern_ending = true;
            }
        }
        if (lightning.pattenr_data[lightning.pattern_count].time >= Managers.Sound.bgSound.time && !lightning.pattern_ending)
        {
            lightning.pattern_count++;
            if (lightning.pattenr_data.Count == lightning.pattern_count)
            {
                lightning.pattern_ending = true;
            }
        }
        if (rush_pattern.pattenr_data[rush_pattern.pattern_count].time >= Managers.Sound.bgSound.time && !rush_pattern.pattern_ending)
        {
            Rush();
            rush_pattern.pattern_count++;
            if (rush_pattern.pattenr_data.Count == rush_pattern.pattern_count)
            {
                rush_pattern.pattern_ending = true;
            }
        }
        if (lightning.pattenr_data[lightning.pattern_count].time >= Managers.Sound.bgSound.time)
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

        }
    }
    /*public override void Simple_pattern()
    {
        base.Simple_pattern();
        switch (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].simple_pattern_type)
        {
            case (sbyte)Cha_in_gureumi_hard_patterns.IDLE:
                Idle();
                break;
            case (sbyte)Cha_in_gureumi_simple_patterns.RAINDROPS:
                Rain_drop();
                break;
            case (sbyte)Cha_in_gureumi_simple_patterns.RAIN_STORM:
                Rain_storm();
                break;
            case (sbyte)Cha_in_gureumi_simple_patterns.SHOWER:
                Shower();
                break;
            case (sbyte)Cha_in_gureumi_simple_patterns.RUSH:
                Rush();
                break;
            default:
                break;
        }
    }
    public override void Hard_pattern()
    {
        base.Hard_pattern();
        switch (Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].hard_pattern_type)
        {
            case (sbyte)Cha_in_gureumi_hard_patterns.IDLE:
                Idle();
                break;
            case (sbyte)Cha_in_gureumi_hard_patterns.BROAD_LIGHTNING:
                Broad_lightning();
                break;
            case (sbyte)Cha_in_gureumi_hard_patterns.LIGHTNING_BALL:
                Lightning_ball();
                break;
            case (sbyte)Cha_in_gureumi_hard_patterns.SINGLE_LIGHTNING:
                Single_lightning();
                break;
            default:
                break;
        }
    }*/
   
    public void Idle()
    {
        Anim_state_machin(anim_state["simple_pattern0"]);
    }
    /*public void Rain_drop()
    {
        Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Rain_drop")).transform.position = new Vector2(rain_drop.center_pos.position.x + Random.Range(-rain_drop.pos_x, rain_drop.pos_x), rain_drop.pos_y.position.y);
        Anim_state_machin(anim_state["simple_pattern1"]);
    }*/
    public void Rain_storm()
    {
        if (!Managers.GameManager.pattern_data[Managers.GameManager.pattern_num].ready)
        {
            Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Rain_drop")).transform.position = new Vector2(rain_storm.center_pos.position.x + Random.Range(-rain_storm.pos_x, rain_drop.pos_x), rain_storm.pos_y.position.y);
            Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Rain_drop")).transform.position = new Vector2(rain_storm.center_pos.position.x + Random.Range(-rain_storm.pos_x, rain_drop.pos_x), rain_storm.pos_y.position.y);
        }
        
        
    }
    public void Shower()
    {

    }
    public void Rush()
    {
        switch ((rush_pattern.pattenr_data[rush_pattern.pattern_count].action_num))
        {
            case 0:
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
                break;
        }
    }
    public void Broad_lightning()
    {

    }
    public void Lightning_ball()
    {

    }
    public void Single_lightning()
    {
        /*if (!lightning.pattern_setting)
        {
            lightning.pos_select_range = lightning.pos_min_x.Length - 1;
            lightning.pattern_setting = true;
            lightning.pattern_count = lightning.pattern_num;
            for (int i = lightning.pos_min_x.Length - 1; i >= 0; i--)
            {
                lightning.temp_x.Add(lightning.pos_min_x[i]);
            }

            for (int i = 0; i < lightning.pattern_num; i++)
            {
                int num = Random.Range(0, lightning.pos_select_range);
                lightning.pos_x.Push(lightning.temp_x[num]);
                lightning.temp_x.RemoveAt(num);
                lightning.pos_select_range--;
            }
        }

        if (0 < lightning.pattern_count)
        {
            lightning.pos = new Vector2(lightning.pos_x.Pop(), lightning.pos_y);
            GameObject obj = Instantiate(lightning.lightning);
            obj.transform.position = lightning.pos;
            lightning.pattern_count--;
            if (lightning.pattern_count <= 0)
            {
                lightning.temp_x.Clear();
                lightning.pos_x.Clear();
                lightning.pattern_setting = false;
            }
        }*/
    }
    [Serializable]
    public class Lightning_pattern : Pattern_base_data
    {
        [Header("������ �����ϴ� ��ġ(����)")]
        public float pos_y;
        [Header("������ �����ϴ� x�� ��ġ��")]
        public float[] pos_min_x;
        [Header("������ �����ϴ� Ƚ��")]
        public sbyte pattern_num;
        [HideInInspector]
        public Stack<float> pos_x = new Stack<float>();
        [HideInInspector]
        public List<float> temp_x = new List<float>();
        /*[HideInInspector]
        public sbyte pattern_count;*/
        [HideInInspector]
        public Vector2 pos;
        [HideInInspector]
        public bool pattern_setting = false;
        [Header("���� ������Ʈ �Ҵ�")]
        public GameObject lightning;
        [HideInInspector]
        public int pos_select_range;
    }
    [Serializable]
    /*public class Rain_drop_pattern : Pattern_base_data
    {
        [Header("�����Ǵ� ����")]
        public Transform pos_y;
        [Header("�����Ǵ� x�� ���� ����")]
        public float[] pos_x = new float[8];
        public Transform center_pos;
    }*/
    [Serializable]
    public class Rain_storm_pattern : Pattern_base_data
    {
        [Header("�����Ǵ� ����")]
        public Transform pos_y;
        [Header("�����Ǵ� x�� ���� ����")]
        public float pos_x;
        public Transform center_pos;
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
        [HideInInspector]
        public bool rush_start = false;

    }
}
