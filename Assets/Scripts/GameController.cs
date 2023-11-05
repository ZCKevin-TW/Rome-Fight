using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    [SerializeField] private HpBar PlayerHP;
    [SerializeField] private HpBar EnemyHP;

    [SerializeField] private AudioSource countdownSound;
    [SerializeField] private AudioSource battleBGM;

    [SerializeField] private GameObject WinString;
    [SerializeField] private GameObject LoseString;

    public bool battling;

    // Start is called before the first frame update
    void Start()
    {
        battling = false;
        countdownSound.Play();
        Invoke("GameStart", 3);
    }

    private void GameStart()
    {
        Debug.Log("start");
        battling = true;
        battleBGM.Play();
        PlayerHP.SetStarted(true);
        EnemyHP.SetStarted(true);
    }
    public void GameFinished(bool playerWin)
    {
        Debug.Log("finish");
        battling = false;
        battleBGM.Stop();
        PlayerHP.SetStarted(false);
        EnemyHP.SetStarted(false);
        if (!playerWin)
        {
            Debug.Log("player win");
            WinString.SetActive(true);
        }
        else
        {
            Debug.Log("player lose");
            LoseString.SetActive(true);
        }

        Invoke("ReturnToMenu", 3);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
