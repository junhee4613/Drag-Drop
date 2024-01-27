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
        SINGLE_LIGHTNING,
        /*RANDOM_MULTIPLE_LIGHTNING,*/
        RAINDROPS,
        BROAD_BASED_LIGHTNING
    }
    public enum Cha_in_gureumi_hard_patterns
    {
        LIGHTNING_SPHERE,
        REINFORCED_RAINDROPS,
        SHOWER
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



