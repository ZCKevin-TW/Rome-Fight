using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerControl Player;
    [SerializeField] private float PreTime = .3f;
    [SerializeField] private float PostTime = .5f;
    [SerializeField] private float BlockedPenalty = .5f;
    public float AttackRange = 2f;
    //[SerializeField] private float AttackRange = 1f;
    // [SerializeField] private BoxCollider2D AimPoint;
    private Coroutine lastRoutine = null;
    private Animator anim;

    public enum Status { 
        IdleStage,
        PreStage,
        PostStage,
        // CooldownStage
    };
    public Status CurrentStatus; 
    void SetStatus(Status status)
    {
        CurrentStatus = status;
    }
    public bool Moveable()
    {
        return !InPre();
    }
    public bool Vulnerable()
    {
        return CurrentStatus == Status.PostStage;
    }
    public bool IsActive()
    {
        return CurrentStatus != Status.IdleStage;
    }
    public bool InPre()
    {
        return CurrentStatus == Status.PreStage;
    }
    
    void Start()
    {
        Player = GetComponent<PlayerControl>();
        SetStatus(Status.IdleStage); 
        anim = GetComponentInChildren<Animator>();
    }

    IEnumerator Trigger()
    {
        Player.NoteAttack();
        Debug.Log("Start attacking procedure");
        anim.SetBool("Attack", true);
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
        bool IsBlocked = AttackEvent();
        Debug.Log("Now sete the status to post stage");
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
        Debug.Log("Set to Idel staget");
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        Debug.Log("Attack end");
        anim.SetBool("Attack", false);

        lastRoutine = null;
        yield return null; 
    }
    public void Cancel()
    {
        if (IsActive())
        { 
            StopCoroutine(lastRoutine);
            lastRoutine = null;
            anim.SetBool("Attack", false); 
            SetStatus(Status.IdleStage); 
        }
    }
    // Update is called once per frame
    public void Fire()
    {
        if (!IsActive())
        {
            lastRoutine = StartCoroutine(Trigger());
        }
    }
    // If the attack was blocked, return true;
    // else return fales;
    public bool AttackEvent()
    { 
        if (Mathf.Abs(Player.EnemyDelta()) <= AttackRange)
        {
            if (Player.Enemy.IsHit())
            {
                // Attack success
                return false;
            }
            else
                return true; 
        }
        return false;
    }
}
