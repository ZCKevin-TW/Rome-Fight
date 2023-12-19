using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CucumberIntroEvent : MonoBehaviour
{
    Animator animator;
    Transform mychars;
    Animator animator_background;

    [SerializeField] private GameObject MusicPlayer;
    [SerializeField] private AudioClip charAppearSound;
    [SerializeField] private AudioClip BGM;
    private AudioSource sfxPlayer;
    [SerializeField] private float charAppearDelay = 2.68f;

    private void Start()
    {
        animator = GameObject.Find("Ccucumber1").GetComponent<Animator>();
        Debug.Log("animator", animator);
        mychars = GameObject.Find("Chars").transform;
        Debug.Log("mychars", mychars);
        animator_background = GameObject.Find("Middle").GetComponent<Animator>();
        Debug.Log("background", animator_background);
        sfxPlayer = GetComponent<AudioSource>();
    }
    public void IntroOutro()
    {
        // play intro music
        // sfxPlayer.PlayOneShot(BGM, .7f);
        GameObject.FindGameObjectWithTag("IntroMusic").GetComponent<MusicClass>().PlayMusic();

        // stop VN music
        AudioSource[] music = MusicPlayer.GetComponents<AudioSource>();
        foreach (AudioSource source in music)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        StartCoroutine(Alltros());
    }

    IEnumerator Type()
    {
        foreach (RectTransform c in mychars)
        {
            sfxPlayer.PlayOneShot(charAppearSound, 1f);
            c.gameObject.SetActive(true);
            yield return new WaitForSeconds(.3f);
        }
        yield return null;
    }

    IEnumerator Fade()
    {
        foreach(RectTransform c in mychars)
        {
            c.gameObject.SetActive(false);
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }
    IEnumerator Alltros()
    {
        yield return new WaitForSeconds(charAppearDelay);
        StartCoroutine(Type());
        yield return new WaitForSeconds(3);
        StartCoroutine(Fade());
        yield return new WaitForSeconds(1);
        animator_background.Play("BackgroundFlash");
    }
    
}
