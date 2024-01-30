using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : Unit
{
    [Header("�� ������ ���� ������ �����ؾߵǴ� Ƚ��")]
    public int gimmick_complete_num;
    [Header("�뷡 ���൵")]
    public float hp;
    [Header("�ϵ����� ���� ���� ���൵")]
    public float hard_pattern_start_hp;
    [Header("�� ������ ���� ������ �����ϴ� Ƚ��")]
    public int gimmick_num;
    [Header("�� ������ ������ �������� �ӵ�")]       //�̰� �� ���� ����
    public int patterns_speed;
    [Header("�����ؾߵǴ� ���� ��Ʈ ����")]
    public RaycastHit2D pattern_note;
    [Header("���� ������")]
    public LayerMask pattern_kind;
    public Transform note_pos;
    #region ���߿� private�� �ٲ� �͵�
    protected int gimmick_complete_count = 0;      //���� ������� ������ Ƚ��
    //protected bool Pattern_in_progress = false;
    protected float time;
    protected bool weakness_pattern_start = false; //����� �����ߴ���
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
                //���� Ŭ����
            }
            else
            {
                //���� ����
            }
        }
    }
    #endregion

    // Start is called before the first frame update
}
