using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucumberIntroEvent : MonoBehaviour
{
    Animator animator;
    Transform mychars;
    Animator animator_background;

    private void Start()
    {
        animator = GameObject.Find("Ccucumber1").GetComponent<Animator>();
        Debug.Log("animator", animator);
        mychars = GameObject.Find("Chars").transform;
        Debug.Log("mychars", mychars);
        animator_background = GameObject.Find("Middle").GetComponent<Animator>();
        Debug.Log("background", animator_background);
    }
    public void IntroOutro()
    {
        StartCoroutine(Alltros());
    }

    IEnumerator Type()
    {
        foreach (RectTransform c in mychars)
        {
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
        StartCoroutine(Type());
        yield return new WaitForSeconds(2);
        StartCoroutine(Fade());
        yield return new WaitForSeconds(1);
        animator_background.Play("BackgroundFlash");
    }
    
}
