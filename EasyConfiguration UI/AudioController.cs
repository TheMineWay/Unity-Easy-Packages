using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public enum AudioMode {
        music,
        effects
    }
    public AudioMode audioMode = AudioMode.music;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetVolume();
        if(audioMode == AudioMode.music) TMW_Config.musicChanged += SetVolume;
        else TMW_Config.effectsChanged += SetVolume;
    }

    void SetVolume() {
        audioSource.volume = (audioMode == AudioMode.music ? TMW_Kernel.configuration.music : TMW_Kernel.configuration.effects) / 100f;
    }
}
