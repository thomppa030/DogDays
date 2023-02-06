using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using NaughtyAttributes;


[System.Serializable]
public class Sound
{
    public AudioMixerGroup audioMixerGroup;

    private AudioSource source;

    public string clipName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop = false;
    public bool playOnAwake = false;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.playOnAwake = playOnAwake;
        source.outputAudioMixerGroup = audioMixerGroup;

    }
    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField, ReorderableList]
    Sound[] sound;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < sound.Length; i++)
        {
            GameObject _go = new GameObject("Sound" + i + "_" + sound[i].clipName);
            _go.transform.SetParent(this.transform);
            sound[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].clipName == _name)
            {
                sound[i].Play();
                return;
            }
        }

    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].clipName == _name)
            {
                sound[i].Stop();
                return;
            }
        }
    }

    public AudioClip GetClip(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].clipName == _name)
            {
                return sound[i].clip;
            }
        }
        return null;
    }
}
