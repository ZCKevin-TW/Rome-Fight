using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    private float LeftEdge, RightEdge;
    private PlayerControl Player;
    [SerializeField] private float PreTime = .5f;
    [SerializeField] private float PostTime = .5f;
    [SerializeField] private float BlockedPenalty = .5f;
    [SerializeField] private BoxCollider2D AimPoint;
    private Animator anim;

    enum Status { 
        IdleStage,
        PreStage,
        PostStage,
        // CooldownStage
    };
    private Status CurrentStatus; 
    void SetStatus(Status status)
    {
        CurrentStatus = status;
    }
    public bool Moveable()
    {
        return CurrentStatus != Status.PreStage; 
    }
    public bool Vulnerable()
    {
        return CurrentStatus == Status.PostStage;
    }
    public bool IsActive()
    {
        return CurrentStatus != Status.IdleStage;
    }
    
    void Start()
    {
        Player = GetComponent<PlayerControl>();
        SetStatus(Status.IdleStage); 

        RightEdge = (float)Random.Range(2, 4);
        LeftEdge = (float)Random.Range(-4, -2);
        Debug.Log(LeftEdge);
        Debug.Log(RightEdge);
        anim = GetComponent<Animator>();
    }

    IEnumerator Trigger()
    {
        Debug.Log("Start attacking procedure");
        anim.SetBool("Attack", true);
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
        bool IsBlocked = AttackEvent();
        SetStatus(Status.PostStage);
        if (!IsBlocked)
        {
            yield return new WaitForSeconds(PostTime);
        }
        else
        {
            Debug.Log("I am blocked, now wait longer");
            yield return new WaitForSeconds(PostTime + BlockedPenalty);
        }
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        Debug.Log("Attack end");
        anim.SetBool("Attack", false);
        yield return null; 
    }
    public void Cancel()
    {
        if (IsActive())
        {
            Debug.Log("Attack is cancelled");
            anim.SetBool("Attack", false);
            SetStatus(Status.IdleStage);
            StopCoroutine("Trigger");
        }
    }
    // Update is called once per frame
    public void Fire()
    {
        if (!IsActive())
        {
            StartCoroutine("Trigger");
        }
    }
    // If the attack was blocked, return true;
    // else return fales;
    public bool AttackEvent()
    {
        Debug.Log("Attack! " + AimPoint.ToString());
        // the 5 should be set to the maximum number of collider that hit it
        Collider2D[] HitObjects = new Collider2D[5];
        int NumHit = AimPoint.OverlapCollider(new ContactFilter2D().NoFilter(), HitObjects);
        Debug.Log(AimPoint.transform.position);
        for (int i = 0;i < NumHit;++i) {
            if (HitObjects[i].tag == "Enemy")
            {
                Debug.Log(HitObjects[i].name); 
                GameObject Enemy = HitObjects[i].gameObject;
                if (Enemy.GetComponent<PlayerControl>().IsHit())
                {
                    Debug.Log("Hit enemy Success");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        Debug.Log("Hit " + NumHit);
        
        return false;
    }
}
