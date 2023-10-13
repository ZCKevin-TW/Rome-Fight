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
    [SerializeField] private float InTime = .1f;
    [SerializeField] private float PostTime = .5f;
    [SerializeField] private float BlockedPenalty = .5f;
    public float AttackRange = 2f;
    //[SerializeField] private float AttackRange = 1f;
    // [SerializeField] private BoxCollider2D AimPoint;
    private Coroutine lastRoutine = null;
    private Animator Anim;

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
        Anim = GetComponentInChildren<Animator>();
    }

    private void ResetAnim()
    {
        Anim.SetBool("PreAttack", false);
        Anim.SetBool("InAttack", false);
        Anim.SetBool("PostAttack", false);
        Anim.SetBool("InDefense", false);
        Anim.SetBool("InDizzy", false);
    }
    IEnumerator Trigger()
    {
        Player.NoteAttack();
        Debug.Log("Start attacking procedure");
        Anim.SetBool("PreAttack", true);
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
        Anim.SetBool("PreAttack", false);

        Anim.SetBool("InAttack", true);
        bool IsBlocked = AttackEvent();
        yield return new WaitForSeconds(InTime);
        Anim.SetBool("InAttack", false);

        Debug.Log("Now sete the status to post stage");
        SetStatus(Status.PostStage);
        if (!IsBlocked)
        {
            Anim.SetBool("PostAttack", true);
            yield return new WaitForSeconds(PostTime);
            Anim.SetBool("PostAttack", false);
        }
        else
        {
            Debug.Log("I am blocked, now wait longer");
            Anim.SetBool("InDizzy", true);
            yield return new WaitForSeconds(PostTime + BlockedPenalty);
            Anim.SetBool("InDizzy", false);
        }
        Debug.Log("Set to Idel staget");
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        Debug.Log("Attack end");
        lastRoutine = null;
        //ResetAnim();
        yield return null; 
    }
    public void Cancel()
    {
        if (IsActive())
        { 
            StopCoroutine(lastRoutine);
            ResetAnim();
            lastRoutine = null;
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
