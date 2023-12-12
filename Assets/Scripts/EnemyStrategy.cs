using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrategy : MonoBehaviour
{
    // Start is called before the first frame update
    private Player PlayerAPI;
    [SerializeField] private float MaxPatterTime = .8f;
    private float ReactTime() => NextGaussian(.5f, .2f, 0f, 2f);
    [SerializeField] private float CloseDis = .1f;
    [SerializeField] private float StrategySwitchTime = 5f;
//    [SerializeField] private float OutterBlockProbability = .8f;
    [SerializeField] private float SideAttackProb = .25f;
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
        //PlayerAPI = GetComponent<PlayerControl>();
        PlayerAPI = GetComponent<Player>(); 
    //    Debug.Log("awaking");
        InVisionTime = 0;
        CurrentMode = Mode.Idle;

        return;
        // for testing 

        StartCoroutine("RandomMoving");
        StartCoroutine("Switching");
    } 

    // Update is called once per frame
    void Update()
    {
        // PlayerAPI.BanMovement(1000f, () => { });
        PlayerAPI.pressSideAttack();
        return;

// Debug.Log("Enemy strategy update");
        if (PlayerAPI.HitWall()) dx *= -1;
        PlayerAPI.pressMove(dx);
        if (!PlayerAPI.enemy.InsideHitBox(PlayerAPI.CurrentState.Aimpoint))
            InVisionTime = 0;
        else
            ++InVisionTime;
        if (InVisionTime > 20 && CurrentMode == Mode.Attack) 
        {
            if (Random.Range(0f, 1f) > SideAttackProb)
                PlayerAPI.pressAttack();
            else
                PlayerAPI.pressSideAttack();
        }

        switch (CurrentMode)
        {
            case Mode.Idle:
                break;
            case Mode.Attack:
                PlayerAPI.pressMove(PlayerAPI.AttackEnemyDirection());
                break;
            case Mode.Escape:
                PlayerAPI.pressMove(PlayerAPI.EscapeEnemyDirection());
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
            float RemainTime = Random.Range(0f, StrategySwitchTime);
            yield return new WaitForSeconds(RemainTime); 
        } 
    }
    public static float NextGaussian() {
        float v1, v2, s;
        do {
            v1 = 2.0f * Random.Range(0f,1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f,1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);
        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s); 
        return v1 * s;
    }
    public static float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }
    public static float NextGaussian (float mean, float standard_deviation, float min, float max) {
        float x;
        do {
            x = NextGaussian(mean, standard_deviation);
        } while (x < min || x > max);
        return x;
    }
    IEnumerator _ReactToAttack()
    {
        if (PlayerAPI == null)
            yield break;
        yield return new WaitForSeconds(ReactTime());

        if (PlayerAPI.DangerDistance() <= CloseDis)
        {
            Debug.Log("Close so dangerous");
            PlayerAPI.pressDefend();
        }
        else if (PlayerAPI.InsideHitBox(PlayerAPI.enemy.CurrentState.Aimpoint))
        {
            PlayerAPI.pressDefend();
        } 
        else
        { 
            //
        } 
    }
    public void ReactToAttack()
    {
        Debug.Log("trigger reacting");
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
