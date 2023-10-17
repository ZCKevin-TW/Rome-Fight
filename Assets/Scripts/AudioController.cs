using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource Audio;
    private bool Played;
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Played = false;
    }
    
    void AttackAudio()
    {
        Played = false;
        if (!Played)
        {
            Played = true;
            Audio.Play();
        }
    }
}
