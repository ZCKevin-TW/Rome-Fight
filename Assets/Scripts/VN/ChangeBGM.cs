using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    [SerializeField] private GameObject MusicPlayer;
    [SerializeField] private int oldIndex;
    [SerializeField] private int newIndex;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audios = MusicPlayer.GetComponents<AudioSource>();
        audios[oldIndex].Stop();
        audios[newIndex].Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
