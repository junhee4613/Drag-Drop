using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Stage_FSM;

public static class Extension //������ ���� Ŭ�����̴�. �Ʒ��� �ִ� �޼������ Ȯ�� �޼����� �ϴµ� staticŰ����� �Բ� ���� Ŭ���� ���� ���ǵ��־�ߵǸ� this��� Ű���带 ��ߵȴ�. �׷��� �ش� Ÿ�԰� ���� ���� �ڿ��� �ٷ� �ش� (Ȯ��)�޼��带 �����ִ�.
{
    public static T GetOrAddComponent<T>(this GameObject go)where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    /*public static void BindEvent(this GameObject go
        ,Action action = null
        ,Action<BaseEventData> dragAction = null
        ,Define.UIEvent type = Define.UIEvent.Click)
    {
        UIBase.BindEvent(go, action, dragAction, type);
    }*/

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static void DestoryChilds(this GameObject go)
    {
        Transform[] children = new Transform[go.transform.childCount];
        for (int i = 0; i < go.transform.childCount; i++)
        {
            children[i] = go.transform.GetChild(i);
        }

        foreach(Transform child in children)    //��� �ڽ� ������Ʈ ���� 
        {
            Managers.Resource.Destroy(child.gameObject);
        }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while( n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void Anim_processing(this Dictionary<string, Anim_stage_state> dic, ref Animator anim, sbyte simplae_pattern_num, sbyte hard_pattern_num )
    {           //FIX : ���߿� ���� �����丵 �ؾߵ�
        if (anim == null)
        {
            Debug.LogError("�ִϸ��̼��� ����");
        }
        Animator temp = anim;
        dic.Add("1_phase_idle", new Phase1_idle(anim));
        dic.Add("2_phase_idle", new Phase2_idle(anim));
        for (int i = 0; i < simplae_pattern_num; i++)
        {
            dic.Add($"simple_pattern{i}", new Simple_pattern($"simple_pattern{i}", anim));
        }
        for (int i = 0; i < hard_pattern_num; i++)
        {
            dic.Add($"hard_pattern{i}", new Hard_pattern($"hard_pattern{i}", anim));

        }
    }
    public static void Anim_processing2(this Dictionary<string, Anim_stage_state> dic, ref Animator anim, string[] anims_name)
    {           //�迭 �������� �ٲ㼭 �ִϸ��̼� �̸��� ���ϰ� �����ϰ� �ǵ��� ����
        if (anim == null)
        {
            Debug.LogError("�ִϸ��̼��� ����");
        }
        Animator temp = anim;
        foreach (string s in anims_name)
        {
            Debug.Log(s);
            dic.Add(s, new Pattern_anim(s, anim));
        }
    }
}
