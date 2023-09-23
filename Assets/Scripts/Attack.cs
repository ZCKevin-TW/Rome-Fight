using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    private float LeftEdge, RightEdge;
    [SerializeField] private PlayerMovement Player;
    [SerializeField] private float PreTime = .5f;
    [SerializeField] private float PostTime = .5f; 
    enum Status { 
        IdleStage,
        PreStage,
        PostStage,
        CooldownStage
    };
    private Status CurrentStatus; 
    void SetStatus(Status status)
    {
        CurrentStatus = status;
    }
    bool IsActive()
    {
        return CurrentStatus != Status.IdleStage;
    }
    void Start()
    {
        SetStatus(Status.IdleStage);
        RightEdge = (float)Random.Range(2, 4);
        LeftEdge = (float)Random.Range(-4, -2);
        Debug.Log(LeftEdge);
        Debug.Log(RightEdge);
    }

    IEnumerator Trigger()
    {
        SetStatus(Status.PreStage);
        Player.Freeze();
        yield return new WaitForSeconds(PreTime);
        AttackEvent();
        yield return null;
        Player.UnFreeze();
        SetStatus(Status.PostStage);
        yield return new WaitForSeconds(PostTime); 
        SetStatus(Status.IdleStage);
        yield return null; 
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Debug.Log("Fire 1 button is triggered");
            if (!IsActive())
                StartCoroutine(Trigger());
        }
    }
    public void AttackEvent()
    {
        float SightX = Player.GetSightX();
        if (SightX >= LeftEdge && SightX <= RightEdge)
            Debug.Log("HIT");
        else
            Debug.Log("MISS");

        Debug.Log(SightX);
    }
}
