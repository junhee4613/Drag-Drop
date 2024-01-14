using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    #region ���߿� private�� �ٲ� �͵�
    public int pattern_num;
    public bool Pattern_in_progress = false;
    public int hp;
    public int hard_pattern_start_hp;
    public GameObject[] simple_patterns;        //�̰� ���߿� ��巹����� �����ؼ� ������ �ҷ����� ������ ����
    public GameObject[] hard_patterns;          //�̰� ���߿� ��巹����� �����ؼ� ������ �ҷ����� ������ ����
    public bool weakness_pattern_start = false; //����� �����ߴ���
    [Header("�� ������ ���� ������ �����ϴ� Ƚ��")]
    public int gimmick_num;
    protected int gimmick_count;  //����� ���� �� ������ ���ڰ� ������
    public void Weakness_pattern()
    {

    }
    public virtual void Start()
    {
        gimmick_count = gimmick_num - 1;
    }
    #endregion

    // Start is called before the first frame update
}
