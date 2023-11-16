using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerControl Enemy;
    private PlayerMovement MoveManager;
    private Attack AttackManager;
    private Defend DefendManager;
    private int CancelCnt;
    private bool Frozen;
    [SerializeField] private bool FromUser = true;
    [SerializeField] private HpBar HpManager;
    private EnemyStrategy Brain;
    [SerializeField] float WoundedPenalty = 1.0f;
    [SerializeField] private GameController gameController;

    // Flash Effect
    [SerializeField] private Flash flashEffect;

    [SerializeField] private float LbodyOffset, RbodyOffset, AttackPointOffset;
    public float Lborder() => LbodyOffset + MoveManager.GetPos();
    public float Rborder() => RbodyOffset + MoveManager.GetPos();
    public float CenterPos() => (Lborder() + Rborder()) / 2;

    // TODO: return attack point according to type
    public float GetAttackPoint(Attack.AttackType Type) => AttackPointOffset + MoveManager.GetPos();
    public bool InsideHitBox(float x) => Lborder() <= x && x <= Rborder();

    // only -1, 0, 1 will be returned
    public float AttackEnemyDirection(Attack.AttackType Type)
    {
        const float eps = 1e-1f;
        if (Mathf.Abs(GetAttackPoint(Type) - Enemy.CenterPos()) <= eps)
            return 0;
        return GetAttackPoint(Type) < Enemy.CenterPos() ? 1 : -1;
    }
    // only -1, 1 will be returned
    public float EscapeEnemyDirection()
    {
        return CenterPos() < Enemy.GetAttackPoint(Attack.AttackType.Normal) ? -1 : 1;
    }
    public float DangerDistance()
    {
        return Mathf.Abs(CenterPos() - Enemy.GetAttackPoint(Attack.AttackType.Normal));
    }

    void Awake()
    {
        MoveManager = GetComponent<PlayerMovement>();
        AttackManager = GetComponent<Attack>();
        DefendManager = GetComponent<Defend>();
        Brain = GetComponent<EnemyStrategy>();
        CancelCnt = 0;
        Frozen = false;
    }
    public void ResetCancelCnt()
    {
        CancelCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (FromUser)
        {
            if (Input.GetButton("Fire1")) pressAttack();
            if (Input.GetButton("Fire2")) pressDefend(); 
            pressMove(Input.GetAxisRaw("Horizontal"));
            if (Input.GetButton("Dash"))
                MoveManager.Dash(Input.GetAxisRaw("Horizontal"));
        }
        MoveManager.SetFreeze(!AttackManager.Moveable() || Frozen);
    }
    public void GetReadyForAttack()
    {
        // Debug.Log("Enemy controller getting ready");
        if (Brain != null)
        {
            Brain.ReactToAttack();
            //Debug.Log("Brain Not Null");
        }
    }
    // Enemy Is within attack range;
    /*
    public bool EnemyClose()
    {
        return Mathf.Abs(EnemyDelta()) <= AttackManager.AttackRange; 
    }
    */
    public void NoteAttack()
    {
        Enemy.GetReadyForAttack();
    }
    public void pressMove(float dx)
    {
        dx = Mathf.Clamp(dx, -1f, 1f);
        MoveManager.SetInput(dx); 
    }
    public void pressAttack()
    {
        if (Frozen || !gameController.battling) return;
        if (DefendManager.IsActive()) 
        {
            if (CancelCnt == 0)
            {
                DefendManager.Cancel();
                ++CancelCnt;
                AttackManager.Fire();
            }
        } else
        {
            AttackManager.Fire();
        } 
    }
    public void pressDefend()
    {
        if (Frozen || !gameController.battling) return;
        if (AttackManager.IsActive())
        {
            if (CancelCnt == 0)
            {
                AttackManager.Cancel();
                ++CancelCnt;
                DefendManager.Block();
            }
        }
        else
        {
            DefendManager.Block();
        }
    }
    // The enemy hit me, return whether I am hit.


    public bool IsHit(bool ArmorPenetration)
    {
        if (!DefendManager.IsBlocking() || ArmorPenetration)
        {
            if (HpManager != null)
                HpManager.DecreaseHP(AttackManager.Vulnerable());
            // Oh no, I am hit, all my current activities are cancelled
            flashEffect.StartFlash();
            DefendManager.Cancel();
            AttackManager.Cancel();
            MoveManager.Cancel();
            ResetCancelCnt();
            BanMovement(WoundedPenalty);
            return true;
        }
        return false;
    } 
    private IEnumerator _BanMovement(float duration)
    {
        Frozen = true;
        yield return new WaitForSeconds(duration);
        Frozen = false;

    } 
    public void BanMovement(float last_time)
    {
        StartCoroutine(_BanMovement(last_time));
    }
    // the direction of enemy to me with length
    public float EnemyDelta()
    {
        if (Enemy == null) return 1000000000f;
        return Enemy.transform.position.x - transform.position.x;
    }
    public bool HitWall()
    {
        return MoveManager.HitWall();
    }
}
