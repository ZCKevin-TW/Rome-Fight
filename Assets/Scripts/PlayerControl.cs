using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement MoveManager;
    private Attack AttackManager;
    private Defend DefendManager;
    private int CancelCnt;
    [SerializeField] private HpBar HpManager;
    private Animator anim;
    void Start()
    {
        MoveManager = GetComponent<PlayerMovement>();
        AttackManager = GetComponent<Attack>();
        DefendManager = GetComponent<Defend>();
        CancelCnt = 0;
    }
    public void ResetCancelCnt()
    {
        CancelCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Means I pressed Attack button
        if (Input.GetButtonDown("Fire1"))
        {
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
        // Means I pressed Defend button
        if (Input.GetButton("Fire2")) 
        {
            if (AttackManager.IsActive())
            {
                if (CancelCnt == 0)
                {
                    AttackManager.Cancel();
                    ++CancelCnt;
                    DefendManager.Block();
                }
            } else
            {
                DefendManager.Block();
            }
        }
        MoveManager.SetFreeze(!AttackManager.Moveable());
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
            return true;
        }
        return false;
    }
}
