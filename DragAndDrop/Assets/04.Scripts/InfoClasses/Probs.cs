using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Unit : MonoBehaviour
{
    public abstract void Hit(float damage);
}


public class playerData : Unit
{
    [Header("���ư��� �ִ� �ӵ�")]
    public float shoot_speed;
    [Header("���� �����ϴ� �ӵ��� ���� 1�� ���� �� �ʴ� �ӷ� 1�� �پ��")]
    public float gradually_down_speed;
    [Header("�巡�� ������ �ӵ� �� ���ư��� �ּ� �ӵ�")]
    public float slow_speed;
    [Header("�ǰ� ���� �� ���� �ð�")]
    public float invincibility_time;
    [Header("ó�� ��� ����")]
    public byte player_life = 3;
    


    public override void Hit(float damage)
    {

    }
}
public enum Player_statu
{
    IDLE,
    DRAG,
    RUN,
    HIT,
}
public class slow_eligibility : MonoBehaviour
{
    public float slow_speed;
    public float speed;
    public virtual void Slow_apply()
    {
        speed = slow_speed;
    }
}

namespace Cha_in_gureumi
{
    public enum Cha_in_gureumi_simple_patterns
    {
        IDLE,
        RAINDROPS,
        RAIN_STORM,
        SHOWER,
        RUSH
    }
    public enum Cha_in_gureumi_hard_patterns
    {
        IDLE,
        BROAD_LIGHTNING,
        SINGLE_LIGHTNING,
        LIGHTNING_BALL
    }
}
namespace Test_boss
{
    public enum Test_patterns_enum
    {
        PATTERN1,
        PATTERN2,
        PATTERN3,
    }

}
interface IInteraction_obj
{
    void practice();
}
[System.Serializable]
public class Pattern_state
{
    [SerializeField]public float time;
    [SerializeField]public sbyte simple_pattern_type;
    [SerializeField]public sbyte hard_pattern_type;
    [SerializeField]public sbyte action_num;
}
public class Stage_setting
{
    public float beat;
    public float bgm_length;
}
public abstract class Anim_stage_state
{
    public Animator an;
    public string temp_name;
    public abstract void On_state_enter();
    public abstract void On_state_update(sbyte loop_num);
    public abstract void On_state_exit();
}
namespace Stage_FSM
{
    public class Simple_pattern : Anim_stage_state
    {
        public Simple_pattern(string anim_name, Animator temp_an)
        {
            this.an = temp_an;
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }
    /*public class Simple_pattern1 : Anim_stage_state
    {
        public Simple_pattern1(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }

    }
    public class Simple_pattern2 : Anim_stage_state
    {
        public Simple_pattern2(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }*/
    public class Hard_pattern : Anim_stage_state
    {
        public Hard_pattern(string anim_name, Animator temp_an)
        {
            an = temp_an;
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }
    /*public class Hard_pattern1 : Anim_stage_state
    {
        public Hard_pattern1(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }
    public class Hard_pattern3 : Anim_stage_state
    {
        public Hard_pattern3(string anim_name)
        {
            temp_name = anim_name;
        }
        public override void On_state_enter()
        {
            an.Play(temp_name);
        }
        public override void On_state_update(sbyte loop_num)
        {

        }
        public override void On_state_exit()
        {

        }
    }*/
}



