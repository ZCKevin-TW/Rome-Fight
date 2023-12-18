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
        // stop music
        AudioSource[] music = MusicPlayer.GetComponents<AudioSource>();
        foreach (AudioSource source in music)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        // intro music
        sfxPlayer.PlayOneShot(BGM, .7f);
        StartCoroutine(Alltros());
    }

    IEnumerator Type()
    {
        foreach (RectTransform c in mychars)
        {
            c.gameObject.SetActive(true);
            sfxPlayer.PlayOneShot(charAppearSound, 1f);
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
        yield return new WaitForSeconds(2.7f);
        StartCoroutine(Type());
        yield return new WaitForSeconds(3);
        StartCoroutine(Fade());
        yield return new WaitForSeconds(1);
        animator_background.Play("BackgroundFlash");
    }
    
}
