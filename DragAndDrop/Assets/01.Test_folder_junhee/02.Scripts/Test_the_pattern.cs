using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test_the_pattern : MonoBehaviour
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
        /*if (!pattern_start)
        {
            pattern_start = true;
            
        }*/
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
/*public float bpm;       //0�� �ȳ��ö�� float / float�� �ؾߵȴ�
    public float pattern_end_time;
    public AudioSource au;
    public AudioClip clip;
    float pattern_duration;     //������ ���ӵ� �ð�
    float beat;                     //��Ʈ(����)
    float time_gone;        //�귯�� �ð�
    Test_patterns_enum current_pattern;
    bool pattern_start;
    public Pattern1_tle test;
    public class Pattern1_tle 
    {
       public sbyte rain_drop_num;
    }


    private void Awake()
    {
    }
    private void Start()
    {
        beat = 60 / bpm;
    }
    private void FixedUpdate()
    {
        if (au.clip != clip)
        {
            au.clip = clip;
            au.Play();
        }
        time_gone += Time.fixedDeltaTime;

        if (beat <= time_gone)
        {
            time_gone -= beat;
            if (!pattern_start)
            {
                pattern_start = true;
                sbyte pattern_num = (sbyte)Random.Range(0, 3);
                current_pattern = (Test_patterns_enum)pattern_num;
            }
        }
        if (pattern_start)
        {
            Pattern_in_progress();
        }
    }
    public void Pattern_in_progress()
    {
        switch (current_pattern)
        {
            case Test_patterns_enum.PATTERN1:
                Pattern1();
                break;
            case Test_patterns_enum.PATTERN2:
                Pattern2();
                break;
            case Test_patterns_enum.PATTERN3:
                Pattern3();
                break;
            default:
                break;
        }
        

    }
    
    public void Pattern1()
    {
        if (pattern_duration <= pattern_end_time)
        {

            pattern_duration += Time.fixedDeltaTime;
        }
        else if (beat <= time_gone)
        {
            pattern_duration = 0;
            pattern_start = false;
        }
    }
    public void Pattern2()
    {
        if (pattern_duration <= pattern_end_time)
        {

            pattern_duration += Time.fixedDeltaTime;

        }
        else if (beat <= time_gone)
        {
            pattern_duration = 0;
            pattern_start = false;
        }
    }
    public void Pattern3()
    {
        if (pattern_duration <= pattern_end_time)
        {

            pattern_duration += Time.fixedDeltaTime;

        }
        else if (beat <= time_gone)
        {
            pattern_duration = 0;
            pattern_start = false;
        }
    }*/
