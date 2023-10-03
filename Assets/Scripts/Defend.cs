using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float PreTime = .5f;
    [SerializeField] private float DefendTime = .5f;
    [SerializeField] private float PostTime = .5f;
    private PlayerControl Player;
    private Animator anim;
    enum Status { 
        IdleStage,
        PreStage,
        DefendingStage,
        PostStage,
        CooldownStage
    };
    private Status CurrentStatus; 
    void SetStatus(Status status)
    {
        CurrentStatus = status;
    }
    void Start()
    {
        SetStatus(Status.IdleStage);
        Player = GetComponent<PlayerControl>();
        anim = GetComponent<Animator>();
    }
    public bool IsActive()
    {
    // Means I am in a life cycle of blocking, doesn't mean I am blocking!
        return CurrentStatus != Status.IdleStage;
    }
    public bool IsBlocking()
    {
        return CurrentStatus == Status.DefendingStage;
    }
    IEnumerator Trigger()
    {
        anim.SetBool("Defense", true);
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
        SetStatus(Status.DefendingStage);
        Debug.Log("Start blocking");
        yield return new WaitForSeconds(DefendTime);
        SetStatus(Status.PostStage);
        Debug.Log("Blocking end");
        anim.SetBool("Defense", false);
        yield return new WaitForSeconds(PostTime); 
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        yield return null; 
    }
    public void Cancel()
    {
        if (IsActive())
        {
            SetStatus(Status.IdleStage);
            Debug.Log("Block is cancelled");
            StopCoroutine("Trigger");
        }
    }
    // Update is called once per frame
    public void Block()
    {
        if (!IsActive())
            StartCoroutine("Trigger");
    }

    // Update is called once per frame
}
