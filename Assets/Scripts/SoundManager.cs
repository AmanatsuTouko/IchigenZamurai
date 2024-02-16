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
        Title,
        Tutorial,
        Level1,
        Level2,
        Level3,
    }

    public enum SE
    {
        // Main Game
        Slash,
        SlashNoHit,
        VoiceDamageGu,
        VoiceDamageGuaaa,

        // Display Level
        DisplayLevel_Dodon,
        DisplayLevel_Chaki,
        DisplayLevel_Kakaxtu,
        DisplayLevel_ChakiEnd,

        // Result
        Result_Pon,
        Result_Explosion,
    }

    // BGM用AudioClips
    [SerializeField] List<AudioClip> _bgmAudioClips = new List<AudioClip>();
    // SE用AudioClips
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

    public void Play(BGM sound)
    {
        int idx = (int)sound;
        _bgmAudioSource.clip = _bgmAudioClips[idx];
        _bgmAudioSource.Play();
    }
    
    public void Stop(SE sound)
    {
        int idx = (int)sound;
        _seAudioSources[idx].Stop();
    }

    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

    private void Init()
    {
        // BGM再生用オブジェクトの再生
        GameObject bgmObject = new GameObject();
        bgmObject.transform.SetParent(this.gameObject.transform);
        AudioSource bgmAudioSource = bgmObject.AddComponent<AudioSource>();
        _bgmAudioSource = bgmAudioSource;

        // SE再生用オブジェクトの作成
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
