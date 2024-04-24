using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using static The_most_angry_gunman;
using static UnityEditor.Progress;
using TMPro;

public class The_most_angry_gunman : BossController
{
    public Gun_shoot gun_shoot;
    public Dynamite dynamite;
    public Tumbleweed tumbleweed;
    public Powder_keg powder_keg;
    public Animator[] hand_ans;
    public GameObject boss_character;
    public GameObject[] hands = new GameObject[2];
    Dictionary<string, Anim_stage_state> right_hand_state = new Dictionary<string, Anim_stage_state>();
    Dictionary<string, Anim_stage_state> left_hand_state = new Dictionary<string, Anim_stage_state>();
    string[] anims = new string[] {"idle", "reload", "right_move","right_shot","left_move", "left_shot", "stage2_dead" };
    string[] hand_anims = new string[] {"gun_idle", "reload", "shot", "look_on", "gun_shot_init" };
    protected override void Awake()
    {
        anim_state.Anim_processing2(ref an, anims);
        right_hand_state.Anim_processing2(ref hand_ans[0], hand_anims);
        left_hand_state.Anim_processing2(ref hand_ans[1], hand_anims);
        gun_shoot.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_shot_data").text);
        dynamite.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_dynamite_data").text);
        powder_keg.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_powder_keg_data").text);
        tumbleweed.pattern_data = JsonConvert.DeserializeObject<List<Pattern_json_date>>(Managers.Resource.Load<TextAsset>("Stage2_tumbleweed_data").text);
        Managers.GameManager.game_start = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Anim_state_machin2(anim_state["idle"], true);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public override void Pattern_processing()
    {
        base.Pattern_processing();
        Pattern_function(ref gun_shoot.pattern_data, ref gun_shoot.pattern_ending, ref gun_shoot.duration,ref gun_shoot.pattern_count, Gun_shoot_pattern);
        Aim_moving();
        Pattern_function(ref dynamite.pattern_data, ref dynamite.pattern_ending, ref dynamite.duration, ref dynamite.pattern_count, Dynamite_pattern);
        Pattern_function(ref tumbleweed.pattern_data, ref tumbleweed.pattern_ending, ref tumbleweed.duration, ref tumbleweed.pattern_count, Tumbleweed_pattern);
        Pattern_function(ref powder_keg.pattern_data, ref powder_keg.pattern_ending, ref powder_keg.duration, ref powder_keg.pattern_count, Powder_keg_pattern);

    }
    public void Aim_moving()    //Ȱ��ȭ�� ���ӵ��� �ٱ��ʿ��� �����̴� �ڵ�
    {
        if (gun_shoot.aim_idle_state[0] && gun_shoot.aims[0] != null && gun_shoot.aims[0].activeSelf)
        {

            Scope_side_move(ref gun_shoot.aims[0], ref gun_shoot.aims_data[0].criteria_dir_x, ref gun_shoot.aims_data[0].criteria_dir_y
                , gun_shoot.criteria_x, gun_shoot.criteria_y, gun_shoot.pop_pos[0].x, gun_shoot.pop_pos[0].y, gun_shoot.aim_speed);
        }
        if (gun_shoot.aim_idle_state[1] && gun_shoot.aims[1] != null && gun_shoot.aims[1].activeSelf)
        {
            Scope_side_move(ref gun_shoot.aims[1], ref gun_shoot.aims_data[1].criteria_dir_x, ref gun_shoot.aims_data[1].criteria_dir_y
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
                if (gun_shoot.aim_idle_state[0] && gun_shoot.aims[0].activeSelf && !gun_shoot.right_shoot)
                {
                    Lock_on(ref gun_shoot, 0);
                    gun_shoot.aims_data[0].attack_action = true;
                    if (gun_shoot.aims[1].activeSelf)
                    {
                        gun_shoot.right_shoot = true;
                    }
                }
                else if(gun_shoot.aim_idle_state[1] && gun_shoot.aims[1].activeSelf)
                {
                    Lock_on(ref gun_shoot, 1);
                    gun_shoot.aims_data[1].attack_action = true;
                    gun_shoot.right_shoot = false;
                }
                break;
            case 2:     //�ش� ��ġ���� �� �� 0.3�� �ڿ� ��� �������� ���ư�
                if (!gun_shoot.aim_idle_state[0] && gun_shoot.aims_data[0].attack_action)
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                    Shoot_after_init_pos((value) => gun_shoot.aim_idle_state[0] = value, 0,ref gun_shoot.aims_data[0].attack_action);
                }
                else if (!gun_shoot.aim_idle_state[1] && gun_shoot.aims_data[1].attack_action)
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                    Shoot_after_init_pos((value) => gun_shoot.aim_idle_state[1] = value, 1, ref gun_shoot.aims_data[1].attack_action);
                }
                break;
            case 3:     //�����ϸ鼭 �� �ϳ��� ���� ����
                Scope_appearance(gun_shoot.aims[1], (value) => gun_shoot.aim_idle_state[1] = value);
                Managers.Main_camera.Punch(4.8f, 5, 0.05f);
                Shoot_after_init_pos((value) => gun_shoot.aim_idle_state[0] = value, 0, ref gun_shoot.aims_data[0].attack_action);
                break;
            case 4:     //4����   0.15�� �������� ���� ���.
                gun_shoot.aim_idle_state[0] = false;
                gun_shoot.aims[0].transform.position = gun_shoot.aims[0].transform.position;
                Chasing_shoot(0, (value) => gun_shoot.aim_idle_state[0] = value);
                break;
            case 5:     //����
                if (!hands[0].activeSelf)
                {
                    foreach (var item in hands)
                    {
                        item.SetActive(true);
                    }
                }
                Anim_state_machin2(right_hand_state["reload"], false);
                Anim_state_machin2(left_hand_state["reload"], false);
                Anim_state_machin2(anim_state["reload"], true);
                break;
            case 6:     //���ӵ� �����
                gun_shoot.aim_idle_state[0] = false;
                gun_shoot.aim_idle_state[1] = false;
                gun_shoot.aims[0].transform.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
                {
                    gun_shoot.aims[0].SetActive(false);
                    gun_shoot.aims[1].transform.DOScale(Vector3.zero, 0.35f).OnComplete(() => gun_shoot.aims[1].SetActive(false));
                });
                break;
            case 7:
                gun_shoot.aim_idle_state[1] = false;
                gun_shoot.aims[1].transform.position = gun_shoot.aims[1].transform.position;
                Chasing_shoot(1, (value) => gun_shoot.aim_idle_state[1] = value);
                break;
            default:
                break;
        }
    }
    public void Scope_side_move(ref GameObject aim, ref float dir_x, ref float dir_y, float range_x, float range_y, float pop_pos_x , float pop_pos_y, float speed)
    {                   //���ӵ��� �ܰ����� �����̴� �ڵ�
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
    public void Chasing_shoot(sbyte gun_num, Action<bool> scope_action_end)     //����
    {
        gun_shoot.aims[gun_num].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.2f).OnComplete(() =>
        {
            Managers.Main_camera.Punch(4.8f, 5, 0.1f);
            if (!hands[0].activeSelf)
            {
                foreach (var item in hands)
                {
                    item.SetActive(true);
                }
            }
            switch (gun_num)
            {
                case 0:
                    Anim_state_machin2(anim_state["right_shot"], false, true);
                    Anim_state_machin2(right_hand_state["shot"], false, true);
                    break;
                case 1:
                    Anim_state_machin2(anim_state["left_shot"], false, true);
                    Anim_state_machin2(left_hand_state["shot"], false, true);
                    break;
            }
            gun_shoot.aims[gun_num].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
            {
                Bullet_mark_ceate(gun_shoot.aims[gun_num].transform.position);
                gun_shoot.aims[gun_num].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.2f).OnComplete(() =>
                {
                    Managers.Main_camera.Punch(4.8f, 5, 0.1f);
                    switch (gun_num)
                    {
                        case 0:
                            Anim_state_machin2(anim_state["right_shot"], false, true);
                            Anim_state_machin2(right_hand_state["shot"], false, true);
                            break;
                        case 1:
                            Anim_state_machin2(anim_state["left_shot"], false, true);
                            Anim_state_machin2(left_hand_state["shot"], false, true);
                            break;
                    }
                    gun_shoot.aims[gun_num].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
                    {
                        Bullet_mark_ceate(gun_shoot.aims[gun_num].transform.position);
                        gun_shoot.aims[gun_num].transform.DOLocalMove(gun_shoot.move_befor_pos[gun_num], 0.2f).OnComplete(() => 
                        { 
                            scope_action_end(true);
                        });
                    });
                });
            });
        });
    }
    public void Scope_create(ref GameObject scope, Vector3 pop_pos)         //������ ����
    {
        scope = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Scope"));
        scope.transform.position = pop_pos;
        scope.SetActive(false);
    }
    public void Scope_appearance(GameObject scope, Action<bool> scope_action_end)   //������ ����
    {
        scope.SetActive(true);
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(scope.transform.DOScale(Vector3.one * 1.5f, 0.2f));
        sequence.Append(scope.transform.DOScale(Vector3.one * 1f, 0.2f).OnComplete(() => 
        { 
            scope_action_end(true);
        }));
    }
    public void Shoot_after_init_pos(Action<bool> scope_action_end, sbyte num, ref bool attack)
    {               //������ �߻� �� ���� ��ġ�� �̵�
        if (!hands[0].activeSelf)
        {
            foreach (var item in hands)
            {
                item.SetActive(true);
            }
        }
        attack = false;
        switch (num)
        {
            case 0:
                Anim_state_machin2(anim_state["right_shot"], false);
                Anim_state_machin2(right_hand_state["shot"], false);
                break;
            case 1:
                Anim_state_machin2(anim_state["left_shot"], false);
                Anim_state_machin2(left_hand_state["shot"], false);
                break;
        }
        gun_shoot.aims[num].transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).OnComplete(() =>
        {
            Bullet_mark_ceate(gun_shoot.aims[num].transform.position);
            switch (num)
            {
                case 0:
                    Anim_state_machin2(right_hand_state["gun_shot_init"], false);
                    break;
                case 1:
                    Anim_state_machin2(left_hand_state["gun_shot_init"], false);
                    break;
            }
            gun_shoot.aims[num].transform.DOLocalMove(gun_shoot.move_befor_pos[num], 0.2f).OnComplete(() => 
            { 
                scope_action_end(true);
            });
        });
    }
    public void Lock_on(ref Gun_shoot gun_Shoot, sbyte num)         //������ �÷��̾� ��ġ�� �̵�
    {
        if (!hands[0].activeSelf)
        {
            foreach (var item in hands)
            {
                item.SetActive(true);
            }
        }
        switch (num)
        {
            case 0:
                Anim_state_machin2(anim_state["right_move"], false);
                Anim_state_machin2(right_hand_state["look_on"], false);
                break;
            case 1:
                Anim_state_machin2(anim_state["left_move"], false);
                Anim_state_machin2(left_hand_state["look_on"], false);
                break;
        }
        gun_Shoot.move_befor_pos[num] = gun_shoot.aims[num].transform.position;
        gun_Shoot.aims[num].transform.DOLocalMove(Managers.GameManager.Player_character.position, 0.3f);
        gun_Shoot.aim_idle_state[num] = false;
    }
    public void Bullet_mark_ceate(Vector3 create_pos)
    {
        Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Bullet_mark")).transform.position = create_pos;
    }
    public void Dynamite_pattern()
    {
        switch (dynamite.pattern_data[dynamite.pattern_count].action_num)
        {
            case 0:     //���̳ʸ���Ʈ ���� 0.3�ʵڿ� ����
                foreach (var item in dynamite.dynamite_objs)
                {
                    if (!item.activeSelf)
                    {
                        item.SetActive(true);
                        if (dynamite.dynamite_obj == null)
                        {
                            dynamite.dynamite_obj = item;
                        }
                        item.transform.localPosition = Vector3.zero;
                        break;
                    }
                }
                break;
            case 1:     //���̳ʸ���Ʈ ��� ������ 1�ʰ� ������
                transform.transform.DOMoveX(Random.Range(0, 3) * dynamite.dir, 0.7f);
                dynamite.dir = -dynamite.dir;
                transform.localScale = new Vector3(dynamite.dir, 1, 1);
                if (dynamite.throw_dynamite)
                {
                    dynamite.throw_dynamite = false;
                    foreach (var item in dynamite.dynamite_objs)
                    {
                        if (!item.activeSelf)
                        {
                            item.SetActive(true);
                            if (dynamite.dynamite_obj == null)
                            {
                                dynamite.dynamite_obj = item;
                            }
                            item.transform.localPosition = new Vector3(1.28f, 0, 0);
                            break;
                        }
                    }
                }
                break;
            case 2:     //���̳ʸ���Ʈ ����� 0.7 �̶� ����
                dynamite.dynamite_landing_pos_x = Random.Range(transform.position.x, 3.5f * dynamite.dir);
                Debug.Log(dynamite.dynamite_obj.name);
                dynamite.dynamite_obj.transform.parent = null;
                DG.Tweening.Sequence sequence = DOTween.Sequence();
                sequence.Join(dynamite.dynamite_obj.transform.DOLocalJump(new Vector3(dynamite.dynamite_landing_pos_x, -3, 0), 5, 1, 0.5f).SetEase(Ease.InSine));
                sequence.Join(dynamite.dynamite_obj.transform.DORotate(new Vector3(0, 0, 360), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(2));
                dynamite.warning = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
                dynamite.warning.transform.position = new Vector3(dynamite.dynamite_landing_pos_x, -1.5f, 0);
                dynamite.warning.transform.localScale = new Vector3(2, 5, 0);
                dynamite.throw_dynamite = true;
                break;
            case 3:     //���̳ʸ���Ʈ ����
                GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Boom"));
                temp.transform.position = new Vector3(dynamite.dynamite_landing_pos_x, -1.5f, 0);
                temp.GetComponent<Animator>().Play("dynamite_boom");
                Managers.Main_camera.Shake_move();
                Managers.Pool.Push(dynamite.warning);
                dynamite.dynamite_obj.SetActive(false);
                foreach (var item in dynamite.dynamite_objs)
                {
                    if (item.activeSelf)
                    {
                        dynamite.dynamite_obj.transform.parent = dynamite.left_hand;
                        dynamite.dynamite_obj = item;
                        break;
                    }
                }
                break;
            case 4:     //����ź ������ 0.7 ���� ��ġ :(0, 0, 0) ������ : (1.5, 1.5, 1.5)
                GameObject flash_bang = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Flashbang"));
                flash_bang.transform.DOMoveY(0f, 0.7f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    Managers.UI_jun.Fade_out_in("White", 0f, 0.1f, 0.4f, 0.2f, Beat_box_size_up);
                    Managers.Pool.Push(flash_bang);
                });
                break;
            default:
                break;
        }
    }
    public void Beat_box_size_up()
    {
        boss_character.SetActive(false);
        Managers.GameManager.Beat_box.transform.position = Vector3.zero;
        Managers.GameManager.Beat_box.transform.localScale = Vector3.one * 1.5f;
    }
    public void Tumbleweed_pattern()
    {
        //���� �����ִ� ����
        tumbleweed.dir = Random.Range(0, 2) == 1 ? 1 : -1;
        switch (tumbleweed.pattern_data[tumbleweed.pattern_count].action_num)
        {
            case 0:     //������
                foreach (var item in tumbleweed.horizontal_tumbleweed_instance)
                {
                    if (tumbleweed.small_turn)
                    {
                        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Small_tumbleweed"));
                        temp.transform.localScale = new Vector3(temp.transform.localScale.x * tumbleweed.dir, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.position = new Vector3(10 * tumbleweed.dir, item, 0);
                        temp.transform.DOMoveX(-10 * tumbleweed.dir, 1.8f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            Managers.Pool.Push(temp);
                            Managers.Pool.Push(tumbleweed.warning[0]);
                            tumbleweed.warning.RemoveAt(0);
                        }); ;
                        //���⿡ ������� ������� ����
                        tumbleweed.small_tumbleweed.Add(item);
                    }
                    else
                    {
                        GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Big_tumbleweed"));
                        temp.transform.localScale = new Vector3(temp.transform.localScale.x * tumbleweed.dir, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.position = new Vector3(10 * tumbleweed.dir, item, 0);
                        temp.transform.DOMoveX(-10 * tumbleweed.dir, 1.8f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            Managers.Pool.Push(temp);
                            Managers.Pool.Push(tumbleweed.warning[0]);
                            tumbleweed.warning.RemoveAt(0);
                        });
                        //���⿡ ������� ������� ����
                        tumbleweed.big_tumbleweed.Add(item);
                    }
                }
                tumbleweed.horizontal_tumbleweed_instance.Clear();
                break;
            case 1:     //ū ����� 1.5
                tumbleweed.small_turn = false;
                int big_num = Random.Range(0, tumbleweed.big_tumbleweed.Count - 1);
                tumbleweed.warning.Add(Warning_box_punch_scale(new Vector3(0, tumbleweed.big_tumbleweed[big_num], 0), new Vector3(15, 1.25f, 0), new Vector3(15, 1.4f, 0), 0.1f, new Vector3(15, 1.5f, 0), 0.05f, false, true));
                tumbleweed.horizontal_tumbleweed_instance.Add(tumbleweed.big_tumbleweed[big_num]);
                tumbleweed.big_tumbleweed.RemoveAt(big_num);
                break;
            case 2:    //(���� ����)) 1.25
                tumbleweed.small_turn = true;
                int small_num = Random.Range(0, tumbleweed.small_tumbleweed.Count - 1);
                tumbleweed.warning.Add(Warning_box_punch_scale(new Vector3(0, tumbleweed.small_tumbleweed[small_num], 0), new Vector3(15, 0.8f, 0), new Vector3(15, 1, 0), 0.1f, new Vector3(15, 1.25f, 0), 0.05f, false, true));
                tumbleweed.horizontal_tumbleweed_instance.Add(tumbleweed.small_tumbleweed[small_num]);
                tumbleweed.small_tumbleweed.RemoveAt(small_num);
                break;
            case 3:     //������ ȸ���� �������� ��Ʈ�ڽ� �Ʒ��κп� ������ �ٶ� �δ� �������� ������(0.7�ʵ��� ��������� ����� ������) 1.5
                float pos_x = tumbleweed.vertical_tumbleweed[Random.Range(0, tumbleweed.vertical_tumbleweed.Count - 1)];
                tumbleweed.vertical_tumbleweed.Remove(pos_x);
                GameObject warning = Warning_box_punch_scale(new Vector3(pos_x, 0, 0), new Vector3(1.5f, 7.5f, 0),
                    new Vector3(1.8f, 7.5f, 0), 0.5f, new Vector3(2.5f, 7.5f, 0), 0.1f, true, false, true, () =>
                    {
                        Vertical_tumble(Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Vertical_tumbleweed")), pos_x);
                    });
                break;
            default:
                break;
        }
    }
    public void Vertical_tumble(GameObject temp, float pos_x)
    {
        temp.transform.position = new Vector3(pos_x, 7, 0);
        temp.transform.DOMoveY(-7, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            tumbleweed.vertical_tumbleweed.Add(temp.transform.position.x);
            Managers.Pool.Push(temp);
        });
    }
    public void Powder_keg_pattern()
    {
        switch (powder_keg.pattern_data[powder_keg.pattern_count].action_num)
        {
            case 0:     //ȭ���� ���� �� ���� ���� �� ����
                powder_keg.num = Random.Range(0, powder_keg.deployable_pos.Count - 1);
                GameObject temp = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Powder_keg"));
                temp.GetComponent<Animator>().Play("Powder_keg_boom");
                temp.transform.localScale = Vector3.zero;
                temp.transform.position = powder_keg.deployable_pos[powder_keg.num];
                temp.transform.DOScale(Vector3.one, 0.35f);
                powder_keg.deployable_pos.RemoveAt(powder_keg.num);
                powder_keg.objs.Add(temp.transform);
                if (powder_keg.objs.Count != 0)
                {
                    Debug.Log(temp.transform.position);
                    foreach (var item in powder_keg.objs)
                    {
                        if (item.transform.position != temp.transform.position)
                        {
                            if (item.position.x == temp.transform.position.x && !powder_keg.boom.Contains(item))
                            {
                                Warning_box_fade(new Vector3(5f, 7.5f, 0), new Vector3(item.position.x, 0, 0), true, 3, 0.23f,() => Powder_keg_pattern_boom(item, temp, true));

                                //FIX : ���߿� ���� ������ �ִϸ��̼� �۵��ǰ� ����
                            }
                            else if (item.position.y == temp.transform.position.y && !powder_keg.boom.Contains(item))
                            {
                                Warning_box_fade(new Vector3(15f, 2.5f, 0), new Vector3(0, item.position.y, 0), true, 3, 0.23f,() => Powder_keg_pattern_boom(item, temp, false));
                            }
                        }
                    }
                }//���ÿ� ó���� ������ �������� �� x��� y���� ���ÿ� ������
                break;
            case 1:     //ȭ���� �� ����� 0.4�� dotween �׸��� �ڽ� �۾���: 0, -1.5, 0�� ��ġ�� 1�� ������
                foreach (var item in powder_keg.objs)
                {
                    if (item.gameObject.activeSelf)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
                Managers.UI_jun.Fade_out_in("White", 0f, 0.3f, 0.1f, 0.3f, Boss_die);
                Managers.GameManager.Beat_box.transform.position = new Vector3(0, -1.5f, 0);
                Managers.GameManager.Beat_box.transform.localScale = Vector3.one;
                break;
            default:
                break;
        }
    }
    public void Boss_die()
    {
        boss_character.SetActive(true);
        boss_character.transform.position = new Vector3(0, 0.25f, 0);
        foreach (var item in hands)
        {
            item.SetActive(false);
        }
        Anim_state_machin2(anim_state["stage2_dead"], false);
    }
    public void Powder_keg_pattern_boom(Transform item, GameObject temp, bool criteria_pos_x)
    {
        Managers.Main_camera.Shake_move();
        powder_keg.boom.Add(item);
        powder_keg.boom.Add(temp.transform);
        Managers.Pool.Push(item.gameObject);
        Managers.Pool.Push(temp);
        if (criteria_pos_x)
        {
            GameObject temp2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Boom"));
            temp2.transform.position = new Vector3(item.position.x, 0, 0);
            if (temp2.transform.localScale != new Vector3(2.5f, 1.5f, 0))
                temp2.transform.localScale = new Vector3(2.5f, 1.5f, 0);
            temp2.GetComponent<Animator>().Play("dynamite_boom");
        }
        /*else FIX: ���߿� ���η� ������ �ִϸ��̼� ������ �ٽ� �ּ� Ǯ��
        {
            GameObject temp2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Boom"));
            temp.transform.position = new Vector3(0, item.position.y, 0);
            temp.GetComponent<Animator>().Play("dynamite_boom");
        }*/
        foreach (var item2 in powder_keg.boom)
        {
            //item2.GetComponent<Animator>().Play("Powder_keg_boom");
            powder_keg.deployable_pos.Add(item2.position);
            powder_keg.objs.Remove(item2);
            //objs �� ���� ��ġ�� ȭ����
        }
        powder_keg.boom.Clear();
    }
    [Serializable]
    public class Gun_shoot : Pattern_base_data
    {
        public GameObject[] aims;       //���� ������Ʈ��
        public Vector3[] init_pos;     //���ӵ��� �÷��̾ ���󰡱� �� ������ ��ġ
        public Vector3[] pop_pos;     //���ӵ��� �÷��̾ ���󰡱� �� ������ ��ġ
        public Vector3[] move_befor_pos;     //���ӵ��� �÷��̾ ���󰡱� �� ������ ��ġ
        public Aims_dir[] aims_data;    //���ӵ��� �ٱ� �ʿ� ��ġ�� �� �̵��ϴ� ����
        public bool[] aim_idle_state = new bool[2]; //������ �÷��̾ �i�� ���ݱ��� �ߴ���
        public float aim_speed = 5f;    //���� ���ǵ�
        public float criteria_x;        //���ӵ��� �����̴� x�� ���� 
        public float criteria_y;        //���ӵ��� �����̴� y�� ����
        public bool right_shoot = false;        //�ΰ��� �����߿� ������ ������ ���� �Ǻ��ϱ� ���� ����
        [Serializable]
        public class Aims_dir           //���ӵ��� ���������� ���������� ���� ����
        {
            public float criteria_dir_x = 1;
            public float criteria_dir_y = 1;
            public bool attack_action = false;
        }
    }
    [Serializable]
    public class Dynamite : Pattern_base_data
    {
        public Transform left_hand;
        public GameObject[] dynamite_objs;
        public GameObject dynamite_obj;
        public GameObject warning;
        public float dynamite_landing_pos_x;
        public float dynamite_throw_pos_x_range;
        public int dir = 1;
        public bool throw_dynamite = false;
        public Animator dynamite_anim;
    }
    [Serializable]
    public class Tumbleweed : Pattern_base_data
    {
        public List<float> small_tumbleweed = new List<float>();            //x�� �������� �������� ȸ����
        public List<float> big_tumbleweed = new List<float>();              //x�� �������� �������� ȸ����
        public List<float> vertical_tumbleweed = new List<float>();         //Y�� �������� �������� ȸ����
        public int dir = 1;                 //�ٶ� �δ� ������ ������ �Ǵ� ��
        public bool small_turn = false;                                     //x�� �������� �������� ȸ���� 2�� �� ���� ���������� �����ִ� �Ұ�
        public List<float> horizontal_tumbleweed_instance = new List<float>();                  //������ x�� �������� �������� ȸ���ʸ� �־�δ� ����Ʈ
        public List<GameObject> warning = new List<GameObject>();                               //������� �����ϴ� ����
    }
    [Serializable]
    public class Powder_keg : Pattern_base_data
    {
        public List<Vector3> deployable_pos = new List<Vector3>();      //��ġ ������ ��ġ
        public HashSet<Transform> objs = new HashSet<Transform>();            //��ġ�� ������Ʈ��
        public HashSet<Transform> boom = new HashSet<Transform>();          //���� ������Ʈ��
        public int num;                     //List(deployable_pos)�� �ִ� ������ �������� ���� ���� ����
    }
}

