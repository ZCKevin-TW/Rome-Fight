using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterFightBGM : MonoBehaviour
{
    [SerializeField] FightingResults record;
    bool IsWin { get => record == null ? false : record.playerWin; }

    private AudioSource bgmPlayer;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    // Start is called before the first frame update
    void Start()
    {
        bgmPlayer = GetComponent<AudioSource>();
        bgmPlayer.clip = IsWin ? winMusic : loseMusic;
        bgmPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
