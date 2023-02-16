using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    BGM,
    EFFECT,
    COUNT,
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            else
                return instance;
        }
    }

    AudioSource[] audioSources = new AudioSource[(int)Sound.COUNT];

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "BGM", "EFFECT"
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };  // BGM�� EFFECT�� ������ ������Ʈ�� �ϳ��� ����
                audioSources[i] = go.AddComponent<AudioSource>();
                audioSources[i].volume = 0.3f;
                go.transform.parent = root.transform;
            }

            audioSources[(int)Sound.BGM].loop = true; // bgm ������ ���� �ݺ� ���
        }
    }

    public void Play(string clipName, Sound soundType = Sound.EFFECT)
    {
        AudioClip audioClip = GetOrAddAudioClip(clipName, soundType);

        if (audioClip == null)
            return;

        if (soundType == Sound.BGM) // BGM ������� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect ȿ���� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.EFFECT];
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void Play(AudioClip clipName, Sound soundType = Sound.EFFECT)
    {
        if (clipName == null)
            return;

        if (soundType == Sound.BGM) // BGM ������� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = clipName;
            audioSource.Play();
        }
        else // Effect ȿ���� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.EFFECT];
            audioSource.PlayOneShot(clipName);
        }
    }

    private AudioClip GetOrAddAudioClip(string clipName, Sound soundType = Sound.EFFECT)
    {
        string path = $"Sounds/{clipName}";
        AudioClip clip = null;

        if (soundType == Sound.BGM)
        {
            clip = Resources.Load<AudioClip>(path);
        }
        else
        {
            if (audioClips.TryGetValue(clipName, out clip) == false)
            {
                clip = Resources.Load<AudioClip>(path);
                audioClips.Add(clipName, clip);
            }
        }

        Debug.Assert(clip != null, $"wrong Clip Name : {clipName}");

        return clip;
    }

    public void SetAudioEnabled(bool isOn, Sound soundType)
    {

        audioSources[(int)soundType].enabled = isOn;
    }

    public void GetAudioSourcesEnabled(out bool bgm, out bool effect)
    {
        bgm = audioSources[(int)Sound.BGM].enabled;
        effect = audioSources[(int)Sound.EFFECT].enabled;
    }

    public void StopBgm()
    {
        AudioSource audioSource = audioSources[(int)Sound.BGM];
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public void PlayButtonSound()
    {
        Play("ButtonSound");
    }
}
