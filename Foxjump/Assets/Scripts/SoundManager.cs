using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager masterSound;//静态类实例
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpAudio, gameoverAudio, cherryAudio;

    private void Awake()
    {
        masterSound = this;
    }

    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = gameoverAudio;
        audioSource.Play();
    }

    public void CherryAudio()
    {
        audioSource.clip = cherryAudio;
        audioSource.Play();
    }
}
