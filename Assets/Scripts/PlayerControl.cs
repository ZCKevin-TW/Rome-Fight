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
    [SerializeField] float Hitted_panelty = 1.0f;
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
        // Means I pressed Attack button
        if (FromUser)
        {
            if (Input.GetButton("Fire1")) pressAttack();
        // Means I pressed Defend button
            if (Input.GetButton("Fire2")) pressDefend(); 
            pressMove(Input.GetAxisRaw("Horizontal"));
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
    public bool EnemyClose()
    {
        return Mathf.Abs(EnemyDelta()) <= AttackManager.AttackRange; 
    }
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
        if (Frozen) return;
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


    public bool IsHit()
    {
        if (!DefendManager.IsBlocking())
        {
            if (HpManager != null)
                HpManager.DecreaseHP(AttackManager.Vulnerable());
            // Oh no, I am hit, all my current activities are cancelled
            DefendManager.Cancel();
            AttackManager.Cancel();
            ResetCancelCnt();
            StartCoroutine(BanMovement(Hitted_panelty));
            return true;
        }
        return false;
    }
    private IEnumerator BanMovement(float last_time)
    {
        Frozen = true;
        yield return new WaitForSeconds(last_time);
        Frozen = false;

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
