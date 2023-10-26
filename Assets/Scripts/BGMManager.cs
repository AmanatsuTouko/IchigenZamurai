using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    /*
    0:Title
    1:Tutorial
    2:Doing Game
    3:Doing Game
    4:Doing Game
    5:Doing Game
    6:Doing Game
    */

    public AudioSource audioSource_titleSE;

    // Start is called before the first frame update
    void Start()
    {
        PlayTitle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayBGM_Index(int idx)
    {
        if (idx >= audioClips.Count) return;
        audioSource.Stop();
        audioSource.clip = audioClips[idx];
        audioSource.Play();
    }
    public IEnumerator StopBGM()
    {
        audioSource.Stop();
        yield return 0;
    }

    public IEnumerator PlayTitle()
    {
        audioSource_titleSE.Play();
        PlayBGM_Index(0);
        yield return 0;
    }
    public IEnumerator PlayGaming(int idx)
    {
        PlayBGM_Index(idx);
        yield return 0;
    }
}
