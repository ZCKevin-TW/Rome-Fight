using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CucumberVanish : MonoBehaviour
{
    // Start is called before the first frame update
    Animator cucumber;
    void Start()
    {
        cucumber = GameObject.Find("Ccucumber1").GetComponent<Animator>();
        Debug.Log(cucumber);
    }

    public void Fillin_done()
    {
        cucumber.Play("CucumberVanish");
    }

    public void ToMainPlay()
    {
        SceneManager.LoadScene("MainPlay");
    }
}
