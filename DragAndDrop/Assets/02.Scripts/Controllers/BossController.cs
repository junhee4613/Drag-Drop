using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    #region ���߿� private�� �ٲ� �͵�
    public int pattern_num;
    public bool Pattern_in_progress = false;
    public int hp = 100;
    public int hard_pattern_start_hp;
    public GameObject[] simple_patterns;        //�̰� ���߿� ��巹����� �����ؼ� ������ �ҷ����� ������ ����
    public GameObject[] hard_patterns;          //�̰� ���߿� ��巹����� �����ؼ� ������ �ҷ����� ������ ����
    #endregion

    // Start is called before the first frame update
}
