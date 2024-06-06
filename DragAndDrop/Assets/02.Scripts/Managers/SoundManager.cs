using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundManager
{
    public AudioSource bgSound;
    public AudioMixer mixer;
    /*public void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("���� �Ŵ���");
        
    }*/
    // Fix : ���� ����(�������� ���� ���� ��Ī�� ���س��� �� ���� ��Ī���� ó��)
    public void BGMSound(AudioClip clip, bool loop)
    {
        bgSound.clip = clip;
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM_sound_volume")[0];
        bgSound.loop = loop;
        bgSound.Play();
    }
    public void SFXSound(string name, AudioClip clip)           //���߿� ������Ʈ Ǯ������ �����ϰ� ���� Ŭ���� ��巹����� �ҷ�����
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX_sound_volume")[0];
        audioSource.Play();

        UnityEngine.MonoBehaviour.Destroy(go, clip.length);
    }

    public void Game_over_sound()
    {
        bgSound.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        if (volume > 0)
        {
            mixer.SetFloat("BGM_sound_volume", Mathf.Log10(volume) * 20);

        }
        else
        {
            mixer.SetFloat("BGM_sound_volume", Mathf.Log10(-80));
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (volume > 0)
        {
            mixer.SetFloat("SFX_sound_volume", Mathf.Log10(volume) * 20);
        }
        else
        {
            mixer.SetFloat("SFX_sound_volume", Mathf.Log10(-80));
        }
    }
}
