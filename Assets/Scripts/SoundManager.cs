using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(this);   
        }
    }

    public enum BGM
    {
        
    }

    public enum SE
    {
        Slash,
        SlashNoHit,
        VoiceDamageGu,
        VoiceDamageGuaaa
    }

    [SerializeField] List<AudioClip> _bgmAudioClips = new List<AudioClip>();

    [SerializeField] List<AudioClip> _seAudioClips = new List<AudioClip>();

    // BGM用AudioSource
    private AudioSource _bgmAudioSource;

    // SE用AudioSource
    private List<AudioSource> _seAudioSources = new List<AudioSource>();

    public void Play(SE sound)
    {
        int idx = (int)sound;
        _seAudioSources[idx].Play();
    }

    public void Stop(SE sound)
    {
        int idx = (int)sound;
        _seAudioSources[idx].Stop();
    }

    private void Init()
    {
        // サウンド再生用オブジェクトの作成
        for(int i=0; i<_seAudioClips.Count; i++)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(this.gameObject.transform);
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = _seAudioClips[i];
            gameObject.name = _seAudioClips[i].name;
            _seAudioSources.Add(audioSource);
        }
    }
}
