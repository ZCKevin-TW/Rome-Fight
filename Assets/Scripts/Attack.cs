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
    [SerializeField] private float InTime = .125f;
    [SerializeField] private float PostTime = .5f;
    [SerializeField] private float BlockedPenalty = .5f;
    private Coroutine lastRoutine = null;
    private Animator Anim;
    private bool LastAttackblocked;

    // Sound Effects
    [SerializeField] private AudioSource swingSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource dizzySound;

    // Visual Effects
    [SerializeField] private GameObject hitImage;
    [SerializeField] private float hitEffectTime = .2f;

    public enum Status { 
        IdleStage,
        PreStage,
        PostStage,
        BlockedStage
        // CooldownStage
    };
    public enum AttackType
    {
        Normal,
        Side
    };
    public Status CurrentStatus; 
    void SetStatus(Status status)
    {
        CurrentStatus = status;
    }
    public bool Moveable()
    {
        return !IsActive();
       // return !InPre() && CurrentStatus != Status.BlockedStage;
    }
    public bool Vulnerable()
    {
        return CurrentStatus == Status.PostStage || CurrentStatus == Status.BlockedStage;
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
    IEnumerator PreAttack(AttackType Type)
    {
        Player.NoteAttack();
        Anim.SetBool("PreAttack", true);
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
    }
    // return true

    IEnumerator BlockedAttack()
    {
        Anim.SetBool("InAttack", false); 
        SetStatus(Status.BlockedStage);
        Anim.SetBool("InDizzy", true);
        dizzySound.Play();
        Debug.Log("Being Dizzy for " + (BlockedPenalty));
        Player.BanMovement(BlockedPenalty);
        //Player.BanMovementOut();
        yield return new WaitForSeconds(BlockedPenalty);
        dizzySound.Stop();
        Anim.SetBool("InDizzy", false); 
    }
    // In Attack flow
    // start animation
    // trigger attack at time {Intime/2}
    // If blocked, switch to Blocked effect
    // else normal
    IEnumerator InAttack(AttackType Type)
    {
        Anim.SetBool("PreAttack", false); 
        Anim.SetBool("InAttack", true);
        swingSound.Play();
        yield return new WaitForSeconds(InTime/2);
        AttackEvent(Type);
        if (LastAttackblocked && Type == AttackType.Normal)
            yield return BlockedAttack();
        else
            yield return new WaitForSeconds(InTime/2); 
    }
    IEnumerator PostAttack(AttackType Type)
    {
        if (LastAttackblocked)
            yield break;

        Anim.SetBool("InAttack", false); 
        Anim.SetBool("PostAttack", true);
        SetStatus(Status.PostStage);
        Debug.Log("Cannot attack for " + PostTime + " sec(s)");
        yield return new WaitForSeconds(PostTime);
        Anim.SetBool("PostAttack", false); 
    }
    IEnumerator SetIdle()
    {
        Debug.Log("Go back to idle");
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        lastRoutine = null;
        yield return null;
    }
    IEnumerator Trigger(AttackType Type)
    {
        yield return PreAttack(Type);
        yield return InAttack(Type);
        yield return PostAttack(Type);
        yield return SetIdle();
    }
    // Cancel is a total reset, should reset everything;

    public void Cancel()
    {
        if (IsActive())
        { 
            Debug.Log("Return to idle");
            dizzySound.Stop();
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
            lastRoutine = StartCoroutine(Trigger(AttackType.Normal));
        }
    }
    // If the attack was blocked, return true;
    // else return false;
    public void AttackEvent(AttackType Type)
    {
        Debug.Log("Player hit at " + Player.GetAttackPoint(Type));
        Debug.Log("Enemy hitbox position between " + Player.Enemy.Lborder() + ", " + Player.Enemy.Rborder());
        LastAttackblocked = false;
        if (Player.Enemy.InsideHitBox(Player.GetAttackPoint(Type)))
        {
            if (Player.Enemy.IsHit(Type == AttackType.Side))
            {
                // Attack success
                StartCoroutine(HitSucceedEffect());
            }
            else
                LastAttackblocked = true;
        }
    }

    private IEnumerator HitSucceedEffect()
    {
        hitSound.Play();
        hitImage.SetActive(true);
        hitImage.transform.SetParent(null);
        yield return new WaitForSeconds(hitEffectTime);
        hitImage.transform.SetParent(this.gameObject.transform);
        hitImage.SetActive(false);
        yield return null;
    }
}
