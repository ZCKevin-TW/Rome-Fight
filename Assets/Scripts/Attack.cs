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
    private Coroutine lastRoutine = null;
    private Animator Anim; 

    // Sound Effects
    [SerializeField] private AudioSource swingSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource dizzySound;

    public enum Status { 
        IdleStage,
        PreStage,
        PostStage,
        BlockedStage
        // CooldownStage
    };
    public Status CurrentStatus; 
    void SetStatus(Status status)
    {
        CurrentStatus = status;
    }
    public bool Moveable()
    {
        return !InPre() && CurrentStatus != Status.BlockedStage;
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
        // Debug.Log("Start attacking procedure");
        Anim.SetBool("PreAttack", true);
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
        Anim.SetBool("PreAttack", false);

        Anim.SetBool("InAttack", true);
        swingSound.Play();
        bool IsBlocked = AttackEvent();
        yield return new WaitForSeconds(InTime);
        Anim.SetBool("InAttack", false);

        // Debug.Log("Now sete the status to post stage");
        if (!IsBlocked)
        {
            SetStatus(Status.PostStage);
            Anim.SetBool("PostAttack", true);
            Debug.Log("Cannot attack for " + PostTime + " sec(s)");
            yield return new WaitForSeconds(PostTime);
            Anim.SetBool("PostAttack", false);
        }
        else
        {
            SetStatus(Status.BlockedStage);
        //    Debug.Log("I am blocked, now wait longer");
            Anim.SetBool("InDizzy", true);
            dizzySound.Play();
            Debug.Log("Being Dizzy for " + (BlockedPenalty));
            Player.BanMovement(BlockedPenalty);
            //Player.BanMovementOut();
            yield return new WaitForSeconds(BlockedPenalty);
            // TODO(ZoeTsou): Fixed this bug... the sound won't stop
            dizzySound.Stop();
            Anim.SetBool("InDizzy", false);
        }
        Debug.Log("Go back to idle");
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        lastRoutine = null;
        //ResetAnim();
        yield return null; 
    }
    public void Cancel()
    {
        if (IsActive())
        {
            Debug.Log("Return to idle");
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
    // else return false;
    public bool AttackEvent()
    {
        Debug.Log("Player hit at " + Player.GetAttackPoint());
        Debug.Log("Enemy hitbox position between " + Player.Enemy.Lborder() + ", " + Player.Enemy.Rborder());
        if (Player.Enemy.InsideHitBox(Player.GetAttackPoint()))
        {
            if (Player.Enemy.IsHit())
            {
                // Attack success
                hitSound.Play();
                return false;
            }
            else
                return true; 
        }
        return false;
    }
}
