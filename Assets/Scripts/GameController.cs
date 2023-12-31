using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    [SerializeField] private HpBar PlayerHP;
    [SerializeField] private HpBar EnemyHP;
    [SerializeField] private Player[] players;

    [SerializeField] private AudioSource countdownSound;
    [SerializeField] private AudioSource battleBGM;

    [SerializeField] private GameObject WinString;
    [SerializeField] private GameObject LoseString;
    [SerializeField] private AudioSource WinSound;
    [SerializeField] private AudioSource LoseSound;
    [SerializeField] private FightingResults record;

    public bool battling;
    public bool gameFinished;

    // Start is called before the first frame update
    void Start()
    {
        battling = false;
        gameFinished = false;
        Invoke("GameStart", 4);
        Invoke("PlayCountDownSound", 1);
        Invoke("PlayBGM", 2.7f);
        Invoke("MusicVanish", .5f);
    }
    private void MusicVanish()
    {
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("IntroMusic");
        AudioSource introMusic = musicPlayer.GetComponent<AudioSource>();
        StartCoroutine(DestroyWrapper(musicPlayer, introMusic));
        
    }
    private IEnumerator DestroyWrapper(GameObject musicPlayer, AudioSource introMusic)
    {
        yield return StartCoroutine(MusicClass.FadeOut(introMusic, 1.5f));
        Destroy(musicPlayer);
    }
    private void PlayCountDownSound()
    {
        countdownSound.Play();
    }

    private void PlayBGM()
    {

        battleBGM.Play();
    }

    private void GameStart()
    {
        Debug.Log("start");
        battling = true;
        PlayerHP.SetStarted(true);
        EnemyHP.SetStarted(true);
        foreach (var p in players)
            p.GameStart();
    }
    public void GameFinished(bool playerWin)
    {
        Debug.Log("finish");
        gameFinished = true;
        battling = false;
        battleBGM.Stop();
        PlayerHP.SetStarted(false);
        EnemyHP.SetStarted(false);
        if (!playerWin)
        {
            Debug.Log("player win");
            players[0].CurrentState.ToNextStateOfWin();
            players[1].CurrentState.ToNextStateOfLose();
            record.playerWin = true;
            WinSound.Play();
            WinString.SetActive(true);
        }
        else
        {
            Debug.Log("player lose");
            players[1].CurrentState.ToNextStateOfWin();
            players[0].CurrentState.ToNextStateOfLose();
            record.playerWin = false;
            LoseSound.Play();
            LoseString.SetActive(true);
        }

        Invoke("ToAfterFight", 3);
    }

    private void ToAfterFight()
    {
        SceneManager.LoadScene("AfterFight");
    }
}
