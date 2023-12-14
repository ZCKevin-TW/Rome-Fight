using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyV1 : EnemyStrategy
{
    // Start is called before the first frame update
    // private Player PlayerAPI;
    [SerializeField] private float MaxPatterTime = .8f;
    [SerializeField] private float CloseDis = .1f;
    [SerializeField] private float StrategySwitchTime = 5f;
    [SerializeField] private float SideAttackProb = .4f;
//    [SerializeField] private float OutterBlockProbability = .8f;
//    [SerializeField] 
//    private float DefendProb = .4f;
    private float dx;
    private int InVisionTime;
    private float ReactTime()
    {
        return NextGaussian(.5f, .2f, 0f, 2f);
//        if (Random.Range(0f, 1f) < DefendProb)
//        {
//            return NextGaussian(.2f, .1f, 0f, .3f);
//        }
//        return 100f;
    }
    enum Mode
    {
        Idle,
        Attack, 
        SideAttack,
        Escape
    };

    private Mode CurrentMode;

    override public void Start()
    {
        base.Start();
        InVisionTime = 0;
        CurrentMode = Mode.Idle;
        StartCoroutine("RandomMoving");
        StartCoroutine("Switching");
    } 
    private float Aimpoint
    {
        get => CurrentMode == Mode.SideAttack ? PlayerAPI.CurrentState.sideaimpoint : PlayerAPI.CurrentState.normalaimpoint;
    }

    // Update is called once per frame
    void Update()
    { 
        if (PlayerAPI.HitWall()) dx *= -1;
        PlayerAPI.pressMove(dx);

        Debug.Log("Enemy aimpoint: "+Aimpoint);
        if (!PlayerAPI.enemy.InsideHitBox(Aimpoint) || CurrentMode == Mode.Idle)
            InVisionTime = 0;
        else
            ++InVisionTime;

        if (InVisionTime > 20)
        {
            if (CurrentMode == Mode.Attack)
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
                if (Random.Range(0.0f, 1.0f) > SideAttackProb)
                    CurrentMode = Mode.Attack;
                else
                    CurrentMode = Mode.SideAttack;
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
        yield return new WaitForSeconds(ReactTime());

        if (PlayerAPI.DangerDistance() <= CloseDis)
        {
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
    public override void ReactToAttack()
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
