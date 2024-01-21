using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat_Test : MonoBehaviour
{
    float beat;
    public float bpm;       //0�� �ȳ��ö�� float / float�� �ؾߵȴ�
    float time;
    public AudioSource au;
    public AudioClip clip;
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
        time += Time.fixedDeltaTime;
        if (beat <= time)
        {
            
            Debug.Log("����");
            transform.position = transform.position * -1;
            time -= beat;

        }
    }
}
