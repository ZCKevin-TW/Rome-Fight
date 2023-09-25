using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    private float LeftEdge, RightEdge;
    private PlayerControl Player;
    [SerializeField] private float PreTime = .5f;
    [SerializeField] private float PostTime = .5f;
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
    }

    IEnumerator Trigger()
    {
        Debug.Log("Start attacking procedure");
        SetStatus(Status.PreStage);
        yield return new WaitForSeconds(PreTime);
        AttackEvent();
        yield return null;
        SetStatus(Status.PostStage);
        yield return new WaitForSeconds(PostTime); 
        SetStatus(Status.IdleStage);
        Player.ResetCancelCnt();
        Debug.Log("Attack end");
        yield return null; 
    }
    public void Cancel()
    {
        if (IsActive())
        {
            Debug.Log("Attack is cancelled");
            SetStatus(Status.IdleStage);
            StopCoroutine("Trigger");
        }
    }
    // Update is called once per frame
    public void Fire()
    {
        if (!IsActive())
            StartCoroutine(Trigger());
    }
    public void AttackEvent()
    {
        /*
        float SightX = Player.GetSightX();
        if (SightX >= LeftEdge && SightX <= RightEdge)
        {
            TheBar.DecreaseHP(false);
            Debug.Log("HIT");
        }
        else
            Debug.Log("MISS");

        Debug.Log(SightX);
        */
    }
}
