using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

public class The_most_angry_gunman : BossController
{
    public Gun_shoot gun_shoot;
    protected override void Awake()
    {
        //base.Awake();
        gun_shoot.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_shoot_data").text);
    }
    // Start is called before the first frame update
    void Start()
    {
        Managers.GameManager.game_start = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Pattern_processing()
    {
        base.Pattern_processing();
        Pattern_function(ref gun_shoot.pattern_data, ref gun_shoot.pattern_ending, ref gun_shoot.duration,ref gun_shoot.pattern_count, Gun_shoot_pattern);
        //Ȱ��ȭ�� ���ӵ��� �ٱ��ʿ��� �����̴� �ڵ�
        if (gun_shoot.aim_idle_state[0] && gun_shoot.aims[0] != null)
        {
            Scope_side_move(ref gun_shoot.aims[0], ref gun_shoot.aims_dir[0].criteria_dir_x, ref gun_shoot.aims_dir[0].criteria_dir_y
                , gun_shoot.criteria_x, gun_shoot.criteria_y, gun_shoot.pop_pos[0].x, gun_shoot.pop_pos[0].y, gun_shoot.aim_speed);
        }
        if (gun_shoot.aim_idle_state[1] && gun_shoot.aims[1] != null)
        {
            Scope_side_move(ref gun_shoot.aims[1], ref gun_shoot.aims_dir[1].criteria_dir_x, ref gun_shoot.aims_dir[1].criteria_dir_y
                , gun_shoot.criteria_x, gun_shoot.criteria_y, gun_shoot.pop_pos[1].x, gun_shoot.pop_pos[1].y, gun_shoot.aim_speed);
        }
    }
    public void Gun_shoot_pattern()
    {   
        
        //���Ϻ� �ൿ��
        switch (gun_shoot.pattern_data[gun_shoot.pattern_count].action_num)
        {
            case 0:     //���� ����
                if(gun_shoot.aims[0] == null && gun_shoot.aims[1] == null)
                {
                    Scope_create(ref gun_shoot.aims[0], gun_shoot.pop_pos[0]);
                    Scope_create(ref gun_shoot.aims[1], gun_shoot.pop_pos[1]);
                }
                Scope_appearance(gun_shoot.aims[0], (value) => gun_shoot.aim_idle_state[0] = value);
                break;
            case 1:     //���ӵ��� �÷��̾� ��ġ�� �̵�
                if (!gun_shoot.right_aim_chasing)
                {
                    gun_shoot.move_befor_pos[0] = gun_shoot.aims[0].transform.position;
                    gun_shoot.aims[0].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.3f);
                    gun_shoot.aim_idle_state[0] = false;
                }
                else
                {
                    gun_shoot.move_befor_pos[1] = gun_shoot.aims[1].transform.position;
                    gun_shoot.aims[1].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.3f);
                    gun_shoot.aim_idle_state[1] = false;
                }
                break;
            case 2:     //�ش� ��ġ���� �� �� 0.3�� �ڿ� ��� �������� ���ư�
                if (!gun_shoot.right_aim_chasing)
                {
                    Managers.Main_camera.Punch(4.8f, 5);
                    gun_shoot.aims[0].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() => 
                    gun_shoot.aims[0].transform.DOLocalMove(gun_shoot.move_befor_pos[0], 0.2f).OnComplete(() => 
                    gun_shoot.aim_idle_state[0] = true));

                    if (gun_shoot.aims[1].activeSelf)
                    {
                        gun_shoot.right_aim_chasing = true;
                    }
                }
                else
                {
                    Managers.Main_camera.Punch(4.8f, 5);
                    gun_shoot.aims[1].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
                    gun_shoot.aims[1].transform.DOLocalMove(gun_shoot.move_befor_pos[1], 0.2f).OnComplete(() => 
                    gun_shoot.aim_idle_state[1] = true));
                    gun_shoot.right_aim_chasing = false;
                }
                break;
            case 3:     //�����ϸ鼭 �� �ϳ��� ���� ����
                Scope_appearance(gun_shoot.aims[1], (value) => gun_shoot.aim_idle_state[1] = value);
                Managers.Main_camera.Punch(4.8f, 5);
                gun_shoot.aims[0].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
                gun_shoot.aims[0].transform.DOLocalMove(gun_shoot.move_befor_pos[0], 0.2f).OnComplete(() =>
                gun_shoot.aim_idle_state[0] = true));
                gun_shoot.right_aim_chasing = true;
                break;
            case 4:     //����
                break;
            default:
                break;
        }
    }
    public void Scope_side_move(ref GameObject aim, ref float dir_x, ref float dir_y, float range_x, float range_y, float pop_pos_x , float pop_pos_y, float speed)
    {
        aim.transform.position = new Vector3(Mathf.Clamp(aim.transform.position.x + Time.deltaTime * Mathf.Sin(45 * Mathf.Deg2Rad) * dir_x * speed, pop_pos_x - range_x, pop_pos_x + range_x),
                    Mathf.Clamp(aim.transform.position.y + Time.deltaTime * Mathf.Cos(315 * Mathf.Deg2Rad) * dir_y * speed, pop_pos_y - range_y, pop_pos_y + range_y));
        if (aim.transform.position.x == pop_pos_x + range_x || aim.transform.position.x == pop_pos_x - range_x)
        {
            dir_x = -dir_x;
        }
        if (aim.transform.position.y == pop_pos_y + range_y || aim.transform.position.y == pop_pos_y - range_y)
        {
            dir_y = -dir_y;
        }
    }
    public void Scope_create(ref GameObject scope, Vector3 pop_pos)
    {
        scope = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Scope"));
        scope.transform.position = pop_pos;
        scope.SetActive(false);
    }
    public void Scope_appearance(GameObject scope, Action<bool> scope_action_end)
    {
        scope.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(scope.transform.DOScale(Vector3.one * 1.5f, 0.2f));
        sequence.Append(scope.transform.DOScale(Vector3.one * 1f, 0.2f).OnComplete(() => scope_action_end(true)));
    }
    [Serializable]
    public class Gun_shoot : Pattern_base_data
    {
        public GameObject[] aims;       //���� ������Ʈ��
        public Vector3[] init_pos;     //���ӵ��� �÷��̾ ���󰡱� �� ������ ��ġ
        public Vector3[] pop_pos;     //���ӵ��� �÷��̾ ���󰡱� �� ������ ��ġ
        public Vector3[] move_befor_pos;     //���ӵ��� �÷��̾ ���󰡱� �� ������ ��ġ
        public Aims_dir[] aims_dir;    //���ӵ��� �ٱ� �ʿ� ��ġ�� �� �̵��ϴ� ����
        public bool[] aim_idle_state = new bool[2]; //������ �÷��̾ �i�� ���ݱ��� �ߴ���
        public float aim_speed = 5f;    //���� ���ǵ�
        public float criteria_x;        //���ӵ��� �����̴� x�� ���� 
        public float criteria_y;        //���ӵ��� �����̴� y�� ����
        public bool right_aim_chasing = false;    //���ӵ��� ������ �� �����ʺ��� ������ �����ϴ���
        [Serializable]
        public class Aims_dir
        {
            public float criteria_dir_x = 1;
            public float criteria_dir_y = 1;
        }
    }

}
    
