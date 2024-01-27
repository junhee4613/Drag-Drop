using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test_boss;
using DG.Tweening;

public class Beat_Test : MonoBehaviour
{
    public float pattern_end_time;
    float pattern_current_time;
    public float beat;
    public float bpm;       //0�� �ȳ��ö�� float / float�� �ؾߵȴ�
    float beat_time;
    public AudioSource au;
    public AudioClip clip;
    public Test_patterns_enum current_pattern;
    bool pattern_start;
    public sbyte pattern_num;
    public float test;


    #region �׽�Ʈ��
    public float test_pattern_end_time;
    public float test_pattern_current_time;
    #endregion

    private void Awake()
    {
        test = clip.length;
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
        beat_time += Time.fixedDeltaTime;
        if (beat <= beat_time)
        {
            beat_time -= beat;
            if (!pattern_start)
            {
                pattern_start = true;
                pattern_num = (sbyte)Random.Range(0, 3);
                current_pattern = (Test_patterns_enum)pattern_num;
                test_pattern_current_time = 0;
            }

            if(pattern_start && test_pattern_current_time <= test_pattern_end_time)   //�̰� �׽�Ʈ���̶� else�� �ؾߵ�
            {
                Test_beat_patterns();
            }
            else
            {
                pattern_start = false;
            }
        }
        test_pattern_current_time += Time.fixedDeltaTime;
        if(test_pattern_current_time > test_pattern_end_time)   //�̰� �׽�Ʈ���̶� else�� �ؾߵ�
        {
            pattern_start = false;
        }
        /*if (pattern_start)
        {
            Pattern_in_progress();
        }*/
    }
    public void Test_beat_patterns()
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
    public void Pattern_in_progress() 
    {

        if (pattern_current_time <= pattern_end_time)
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
            pattern_current_time += Time.fixedDeltaTime;

        }
        else if(beat <= beat_time)
        {
            pattern_current_time = 0;
            pattern_start = false;
        }

    }
    public void Pattern1()
    {
        transform.position = new Vector2(transform.position.x , transform.position.y * -1);
    }
    public void Pattern2()
    {
        transform.position = new Vector2(transform.position.x * -1, transform.position.y);
    }
    public void Pattern3()
    {
        transform.Rotate(0, 0, 15);
    }
}
