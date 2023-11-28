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
    private bool LastAttackblocked;

    // Sound Effects
    [SerializeField] private AudioSource swingSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource dizzySound;

    // Visual Effects
    [SerializeField] private GameObject hitImage;
    [SerializeField] private float hitEffectTime = .2f;
    [SerializeField] private Animator anim;

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
    public bool IsDizzy()
    {
        return CurrentStatus == Status.BlockedStage;
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
    }

    IEnumerator PreAttack(AttackType Type)
    {
        if (Type == AttackType.Normal) anim.SetTrigger("normalAttack");
        else if (Type == AttackType.Side) anim.SetTrigger("sideAttack");
        SetStatus(Status.PreStage);
        Player.NoteAttack();
        yield return new WaitForSeconds(PreTime);
    }
    // return true

    IEnumerator BlockedAttack()
    {
        SetStatus(Status.BlockedStage);
        dizzySound.Play();
        anim.SetTrigger("dizzy");
        anim.SetBool("inAttack", false);
        Debug.Log("Being Dizzy for " + (BlockedPenalty));
        Player.BanMovement(BlockedPenalty, () => { }); // set animation idle when become movable
        //Player.BanMovementOut();
        yield return new WaitForSeconds(BlockedPenalty);
        dizzySound.Stop();
        // anim.SetTrigger("idle");
    }
    // In Attack flow
    // trigger attack at time {Intime/2}
    // If blocked, switch to Blocked effect
    // else normal
    IEnumerator InAttack(AttackType Type)
    {
        swingSound.Play();
        anim.SetBool("inAttack", true);
        yield return new WaitForSeconds(InTime/2);
        AttackEvent(Type);
        if (LastAttackblocked && Type == AttackType.Normal)
            yield return BlockedAttack();
        else
        {
            yield return new WaitForSeconds(InTime / 2);
            anim.SetBool("inAttack", false);
        }
    }
    IEnumerator PostAttack(AttackType Type)
    {
        if (LastAttackblocked)
            yield break;

        SetStatus(Status.PostStage);
        // Debug.Log("Cannot attack for " + PostTime + " sec(s)");
        yield return new WaitForSeconds(PostTime);
    }
    public IEnumerator SetIdle()
    {
        // Debug.Log("Go back to idle");
        SetStatus(Status.IdleStage);
        anim.SetTrigger("idle");
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
            // Debug.Log("Cancel current movement");
            dizzySound.Stop();
            StopCoroutine(lastRoutine);
            lastRoutine = null;
            SetStatus(Status.IdleStage);
        }
    }
    // Update is called once per frame
    public void Fire(AttackType type)
    {
        if (!IsActive())
            lastRoutine = StartCoroutine(Trigger(type));
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
