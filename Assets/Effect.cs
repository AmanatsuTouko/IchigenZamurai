using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();    
    }

    public void Play()
    {
        _particleSystem.Play();
    }

    public bool CanPlay()
    {
        return !_particleSystem.isPlaying;
    }
}
