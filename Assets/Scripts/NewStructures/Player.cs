using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerState CurrentState;
    [SerializeField] private bool InputFromUser = true;
    [SerializeField] private HpBar HpManager;
    [SerializeField] private Flash flashEffect;
    private bool Active;
    private PlayerMovement MoveManager;
    private EnemyStrategy Brain;
    public Animator anim;
    public Player enemy;
    // Start is called before the first frame update
    void Start()
    {
        Active = false;
        CurrentState = GetComponent<PlayerState>(); 
        anim = GetComponentInChildren<Animator>(); 
        MoveManager = GetComponent<PlayerMovement>();
        Brain = GetComponent<EnemyStrategy>();
    } 
    // Update is called once per frame
    void Update()
    {
        if (!Active)
            return;
        if (InputFromUser)
        {
            if (Input.GetButton("Fire1")) pressAttack();
            if (Input.GetButton("Fire2")) pressDefend();
            float dx = Input.GetAxisRaw("Horizontal");
            pressMove(dx);
            if (Input.GetButton("Dash"))
                pressDash(dx);
        }

        if (!enemy.CurrentState.IsInvincible())
        {
            if (CurrentState.IsNormalAttacking())
            {
                if (enemy.CurrentState.IsDefending())
                {
                    CurrentState.ToNextStateOfbeingBlocked();
                }
                else
                {
                    enemy.CurrentState.ToNextStateOfbeingNormalAttack();
                } 
            }
            else if (CurrentState.IsSideAttacking())
            {
                enemy.CurrentState.ToNextStateOfbeingSideAttack();
            } 
        }
        CurrentState.ToNextState();
    }
    public void GameStart()
    {
        Active = true;
    }
    public void decreaseHP(int x)
    {
        Debug.Log("Decrease HP " + x);
        HpManager.DecreaseHP(x); 
    }
    public void teleportForDistance(float dx)
    {
        MoveManager.Move(dx);
    }
    public void pressAttack()
    {
        if (!Active) return;
        if (CurrentState.ToNextStateOfpressAttack())
            enemy.GetReadyForAttack();
    }
    public void pressSideAttack()
    {
        if (!Active) return;
        if (CurrentState.ToNextStateOfpressSideAttack())
            enemy.GetReadyForAttack();
    }
    public void pressDefend()
    {
        if (!Active) return;
        CurrentState.ToNextStateOfpressDefend();
    }
    public void pressMove(float dx)
    {
        if (!Active) return;
        MoveManager.SetInput(dx);
        MoveManager.SetFreeze(!CurrentState.Moveable()); 
    }
    public void pressDash(float dx)
    {
        if (!Active) return;
        CurrentState.ToNextStateOfpressDash(dx);
    }
    public float getOriginX()
    {
        return MoveManager.GetPos();
    }
    public bool InsideHitBox(float x) => CurrentState.PointInsidethis(x);
    public int Unify(float x)
    {
        const float eps = 1e-1f;
        if (Mathf.Abs(x) <= eps)
            return 0;
        return x < 0 ? -1 : 1;
    }
    public float AttackEnemyDirection()
    {
        return Unify(enemy.CurrentState.GetCenterPos() - CurrentState.Aimpoint);
    }
    // only -1, 1 will be returned
    public float EscapeEnemyDirection()
    {
        return -Unify(enemy.CurrentState.Aimpoint - CurrentState.GetCenterPos());
    }
    public float DangerDistance()
    {
        return Mathf.Abs(CurrentState.GetCenterPos() - enemy.CurrentState.Aimpoint);
    }
    public void GetReadyForAttack()
    {
        // Debug.Log("Enemy controller getting ready");
        if (Brain != null)
        {
            Brain.ReactToAttack();
        }
    } public float EnemyDelta()
    {
        if (enemy == null) return 0;
        return Unify(enemy.CurrentState.GetCenterPos() - CurrentState.GetCenterPos());
    }
    public bool HitWall()
    {
        return MoveManager.HitWall();
    }

}
