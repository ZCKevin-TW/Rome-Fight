using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    [SerializeField] private HpBar PlayerHP;
    [SerializeField] private HpBar EnemyHP;
    [SerializeField] private GameObject WinString;
    [SerializeField] private GameObject LoseString;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GameStart", 3);
    }

    private void GameStart()
    {
        Debug.Log("start");
        PlayerHP.SetStarted(true);
        EnemyHP.SetStarted(true);
    }
    public void GameFinished(bool isPlayer)
    {
        Debug.Log("finish");
        PlayerHP.SetStarted(false);
        EnemyHP.SetStarted(false);
        if (!isPlayer)
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
