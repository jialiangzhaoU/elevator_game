using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//audio.PlayOneShot(AudioClip audioClip, Float volumeScale);

public class PlayAudio : MonoBehaviour
{
    public AudioSource audioSource;
    void Start()
    {
        audioSource.Play();
    }
}