using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StopMusicOnClick()
    {
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("IntroMusic");
        musicPlayer.GetComponent<MusicClass>().StopMusic();
        Destroy(musicPlayer);
    }
}
