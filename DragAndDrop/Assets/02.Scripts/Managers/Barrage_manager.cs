using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage_manager
{
    public List<GameObject> Projectile_spawners = new List<GameObject>();
    public bool addtional_option = false;
    

}
public enum Projectile_moving_mode
{
    GENERAL,
    GUIDED_X,
    GUIDED_Y,
    NON
}
public enum Spawner_mode
{
    REPEAT_END,
    NON

    //���⿡ ź�� ���� ��Ģ ���� ��
}
public enum Projectile_shape
{
    CIRCLE,
    BOX,
    TRIANGLE,
    CAPSULE,
    NON
}
