using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public abstract void Hit(float damage);
}
public class playerData : Unit
{
    [Header("��ų ��� �� 100���� �ʴ� ȸ���ϴ� �ӵ�")]
    public float sp_recovery;
    [Header("��ų ��� �� 100���� �ʴ� �����ϴ� �ӵ�")]
    public float sp_eduction;
    [Header("���ư��� �ִ� �ӵ�")]
    public float shoot_speed;
    [Header("�巡�� ������ �ӵ� �� ���ư��� �ּ� �ӵ�")]
    public float slow_speed;
    [Header("�ǰ� ���� �� ���� �ð�")]
    public float invincibility_time;
    [Header("ó�� ��� ����")]
    public byte player_life = 3;
    [Header("��ų ����")]
    public float slow_skill_range;
    [Header("���ο� ��ų ������ ��� ���̾�")]
    public LayerMask slow_skill_targets;


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


