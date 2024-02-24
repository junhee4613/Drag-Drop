using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Test_cs : MonoBehaviour
{
    public Transform big_ring;
    public Transform middle_ring;
    public Transform small_ring;
    public float big_speed;
    public float middle_speed;
    public float small_speed;

    private void Awake()
    {
    }
    public void FixedUpdate()
    {
        
        /*if (Input.GetKey(KeyCode.S))
        {
            big_ring.Rotate(0, 0, big_speed * Time.deltaTime);
            middle_ring.Rotate(0, 0, -middle_speed * Time.deltaTime);
            small_ring.Rotate(0, 0, small_speed * Time.deltaTime);
        }*/
    }
    /* public Rush_pattern rush_pattern;
     private void Awake()
     {
     }
     private void Start()
     {
         rush_pattern.pattenr_data = JsonConvert.DeserializeObject<List<Pattern_state_date>>(Managers.Resource.Load<TextAsset>("Cloud_rush_data").text);
     }
     private void FixedUpdate()
     {
         if (rush_pattern.pattenr_data[rush_pattern.pattern_count].time <= Managers.Sound.bgSound.time && !rush_pattern.pattern_ending)
         {
             rush_pattern.pattern_starting = true;
             Rush();

         }
         if (rush_pattern.pattern_starting)
         {
             switch ((rush_pattern.pattenr_data[rush_pattern.pattern_count].action_num))
             {
                 case 0:
                     transform.Translate(new Vector3(0, rush_pattern.up_move_speed * Time.fixedDeltaTime, 0));
                     if (rush_pattern.time >= 0.5f)
                     {
                         rush_pattern.time -= 0.5f;

                         rush_pattern.pattern_count++;
                         if (rush_pattern.pattenr_data.Count == rush_pattern.pattern_count)
                         {
                             rush_pattern.pattern_ending = true;
                             rush_pattern.pattern_starting = false;
                         }
                     }
                     rush_pattern.time += Time.fixedDeltaTime;
                     break;
                 case 2:
                     transform.Translate(new Vector3(rush_pattern.rush_speed * Time.fixedDeltaTime, 0, 0));
                     if (rush_pattern.time >= 0.5f)
                     {
                         rush_pattern.time -= 0.5f;

                         rush_pattern.pattern_count++;
                         if (rush_pattern.pattenr_data.Count == rush_pattern.pattern_count)
                         {
                             rush_pattern.pattern_ending = true;
                             rush_pattern.pattern_starting = false;
                         }
                     }
                     rush_pattern.time += Time.fixedDeltaTime;
                     break;
                 default:
                     break;
             }

         }
     }
     public void Rush()
     {
         switch ((rush_pattern.pattenr_data[rush_pattern.pattern_count].action_num))
         {
             case 1:
                 for (int i = 0; i < rush_pattern.rush_pos_deciding_count[rush_pattern.rush_pattern_num]; i++)
                 {
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
                 transform.position = new Vector3(rush_pattern.pos_x * rush_pattern.pos_x_dir[rush_pattern.rush_count], rush_pattern.rush_height.Dequeue(), 0);
                 rush_pattern.rush_speed = rush_pattern.rush_speed_option * rush_pattern.pos_x_dir[rush_pattern.rush_count];
                 break;
             default:
                 break;
         }
     }
     [System.Serializable]
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
         [HideInInspector]
         public bool rush_start = false;

     }*/
}


