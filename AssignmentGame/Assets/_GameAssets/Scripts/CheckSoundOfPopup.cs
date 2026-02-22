using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSoundOfPopup : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnEnable()
    {
        if (SaveManager.Instance.state.isSound)
            audioSource.Play();
    }
}