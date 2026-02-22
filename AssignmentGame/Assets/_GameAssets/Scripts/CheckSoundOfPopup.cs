using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSoundOfPopup : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnEnable()
    {
        if (SoundManager.instance.isSoundOn)
            audioSource.Play();
    }
}