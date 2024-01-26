using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    [Header("�� ������ ���� ������ �����ؾߵǴ� Ƚ��")]
    public int gimmick_complete_num;
    [Header("���� �Ǵ� ������ �����")]
    public int hp;
    [Header("�� ������ ���� ������ �����ϴ� Ƚ��")]
    public int gimmick_num;
    [Header("�� ������ ������ �������� �ӵ�")]
    public int patterns_speed;
    #region ���߿� private�� �ٲ� �͵�
    public int pattern_num;
    public int gimmick_complete_count = 0;      //���� ������� ������ Ƚ��
    public bool Pattern_in_progress = false;
    public float hard_pattern_start_hp;
    public GameObject[] simple_patterns;        //�̰� ���߿� ��巹����� �����ؼ� ������ �ҷ����� ������ ����
    public GameObject[] hard_patterns;          //�̰� ���߿� ��巹����� �����ؼ� ������ �ҷ����� ������ ����
    public bool weakness_pattern_start = false; //����� �����ߴ���
    
    protected int gimmick_count;  //����� ���� �� ������ ���ڰ� ������
    
    public void Weakness_pattern()
    {

    }
    public virtual void Start()
    {
        gimmick_count = gimmick_num - 1;
    }
    public void die()
    {
        if(hp <= 0)
        {
            if(gimmick_complete_num <= gimmick_complete_count)
            {
                //������ ���� �ӵ��� �� ������
            }
            else
            {
                //die(Ŭ����)����
            }

        }
    }
    #endregion

    // Start is called before the first frame update
}
