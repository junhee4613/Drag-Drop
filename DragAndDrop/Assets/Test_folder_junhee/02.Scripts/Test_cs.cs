using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_cs : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] lastData;
    public float beatThreshold_num;
    public bool temp_bool = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastData = new float[audioSource.clip.samples * audioSource.clip.channels];
        audioSource.Play();
        Debug.Log(Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        if (temp_bool)
        {
            Debug.Log(Time.deltaTime);
            temp_bool = false;
        }
        Debug.Log(audioSource.time);

    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        // ��Ʈ �м��� ���� ���� �Ӱ谪 (���� ����)
        float beatThreshold = beatThreshold_num;

        // ���� �������� �����Ϳ� ���� �������� �����͸� ���Ͽ� ��Ʈ ������ ���
        for (int i = 0; i < data.Length; i++)
        {
            float amplitude = Mathf.Abs(data[i]);
            float lastAmplitude = Mathf.Abs(lastData[i]);
            // ������ �Ӱ谪�� �ʰ��ϸ� ��Ʈ�� ����
            if (amplitude - lastAmplitude > beatThreshold)
            {
                Debug.Log("Beat detected!");
                // ���⿡�� ���ϴ� �̺�Ʈ�� ������ �� �ֽ��ϴ�.
            }
        }

        // ���� �������� �����͸� ����
        System.Array.Copy(data, lastData, data.Length);
    }
}
