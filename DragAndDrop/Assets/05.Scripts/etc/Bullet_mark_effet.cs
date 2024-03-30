using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet_mark_effet : MonoBehaviour
{
    public float fade_time;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //FIX : ���Ŀ� �������̽� ���� ȿ�� ó���ϴ� �ɷ� ����
        sr.DOFade(0, fade_time).OnComplete(() => gameObject.SetActive(false));
    }
    private void OnEnable()
    {
        sr.DOFade(0, 0);
    }
}
