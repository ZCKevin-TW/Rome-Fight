using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrategy : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerControl PlayerAPI;
    [SerializeField] private float MaxPatterTime = .8f;
    [SerializeField] private float ReactTime = .1f;
    [SerializeField] private float CloseDis = .1f;
    private float dx;
    private int InVisionTime;
    enum Mode
    {
        Idle,
        Attack,
        Escape
    };
    private Mode CurrentMode;
    void Start()
    {
        PlayerAPI = GetComponent<PlayerControl>();
        Debug.Log("awaking");
        //StartCoroutine("RandomMoving");
        InVisionTime = 0;
        CurrentMode = Mode.Idle;
        StartCoroutine("Switching");
    } 

    // Update is called once per frame
    void Update()
    {
// Debug.Log("Enemy strategy update");
        if (PlayerAPI.HitWall()) dx *= -1;
        PlayerAPI.pressMove(dx);
        if (!PlayerAPI.EnemyClose())
            InVisionTime = 0;
        else
            ++InVisionTime;
        if (InVisionTime > 20 && CurrentMode == Mode.Attack)
            PlayerAPI.pressAttack();


        switch (CurrentMode)
        {
            case Mode.Idle:
                break;
            case Mode.Attack:
                PlayerAPI.pressMove(PlayerAPI.EnemyDelta());
                break;
            case Mode.Escape:
                PlayerAPI.pressMove(-PlayerAPI.EnemyDelta());
                break;
        }
        
    }
    IEnumerator Switching()
    {
        while (true)
        { 
            if (Random.Range(0, 2) == 1)
            {
                CurrentMode = Mode.Idle;
            }
            else
            {
                CurrentMode = Mode.Attack;
            }
            float RemainTime = Random.Range(0f, MaxPatterTime);
            yield return new WaitForSeconds(RemainTime); 
        } 
    }
    IEnumerator _ReactToAttack()
    {
        yield return new WaitForSeconds(ReactTime);
        if (Mathf.Abs(PlayerAPI.EnemyDelta()) <= CloseDis)
        {
            Debug.Log("Close so dangerous");
            if (Random.Range(0, 2) == 1) PlayerAPI.pressDefend();
        }
        else if (PlayerAPI.EnemyClose())
        {
            if (Random.Range(0, 10) < 2) PlayerAPI.pressDefend();
        } 
        else
        { 
            //
        } 
    }
    public void ReactToAttack()
    {
        StartCoroutine(_ReactToAttack()); 
    }
    IEnumerator RandomMoving()
    {
        while (true)
        {
            if (CurrentMode != Mode.Idle) yield return null;

            dx = Random.Range(0, 2) * 2 - 1;
            // dx = -1 or 1
            float RemainTime = Random.Range(0f, MaxPatterTime);
            yield return new WaitForSeconds(RemainTime); 
        } 
    }
}
