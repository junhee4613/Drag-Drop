using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Single_lightning : MonoBehaviour
{
    //������� a�� 0���� ����
    [Header("�����̴� Ƚ��")]
    public sbyte fade_num;
    [Header("a���� ��ǥġ���� ���ϴ� �ð�")]
    public float fade_time;
    [Header("�ּ� a��")]
    public float min_a;
    bool pattern_start = false;
    public SpriteRenderer warning_sprite;
    public GameObject lightning;
    bool warning_end = false;
    float time;
    private void FixedUpdate()
    {
        if (!warning_end)
        {
            if (!pattern_start)
            {

                pattern_start = true;
                warning_sprite.DOFade(min_a, fade_time).SetLoops(fade_num, LoopType.Yoyo).OnComplete(() =>
                {
                    warning_end = true;
                });
            }
        }
        else
        {
            if (time < 1)
            {
                time += Time.fixedDeltaTime;
                if (!lightning.activeSelf)
                {
                    lightning.SetActive(true);
                }
            }
            else
            {
                time = 0;
                lightning.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
