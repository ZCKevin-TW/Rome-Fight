using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float PreTime = .1f;
    [SerializeField] private float DefendTime = .5f;
    [SerializeField] private float PostTime = .5f;
    private Coroutine lastRoutine = null;
    private PlayerControl Player;
    [SerializeField] private Animator anim;

    public enum Status { 
        IdleStage,
        PreStage,
        DefendingStage,
        PostStage,
        CooldownStage
    };
    public Status CurrentStatus; 
    void SetStatus(Status status)
    {
        CurrentStatus = status;
    }
    void Start()
    {
        lastRoutine = null;
        SetStatus(Status.IdleStage);
        Player = GetComponent<PlayerControl>();
    }
    public bool Moveable()
    {
        return !IsActive();
    }
    public bool IsActive()
    {
        return lastRoutine != null;
    // Means I am in a life cycle of blocking, doesn't mean I am blocking!
        // return CurrentStatus != Status.IdleStage;
    }
    public bool IsBlocking()
    {
        return CurrentStatus == Status.DefendingStage;
    }

    IEnumerator Trigger()
    {
        anim.SetTrigger("defense");
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
        SetStatus(Status.DefendingStage);
        Debug.Log("Start blocking");
        yield return new WaitForSeconds(DefendTime);
        SetStatus(Status.PostStage);
        Debug.Log("Blocking end");
        yield return new WaitForSeconds(PostTime);
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        anim.SetTrigger("idle");
        lastRoutine = null;
        yield return null; 
    }
    public void Cancel()
    {
        if (IsActive())
        {
            StopCoroutine(lastRoutine);
            lastRoutine = null;
            SetStatus(Status.IdleStage);
            Debug.Log("Block is cancelled");
        }
    }

    public void Block()
    {
        if (!IsActive())
        {
            Debug.Log("Defend is triggered");
            lastRoutine = StartCoroutine(Trigger());
        }
    }
}
