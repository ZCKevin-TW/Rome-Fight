using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement MoveManager;
    private Attack AttackManager;
    [SerializeField] private HpBar HPManager;
    void Start()
    {
        MoveManager = GetComponent<PlayerMovement>();
        AttackManager = GetComponent<Attack>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            AttackManager.Fire();
        MoveManager.SetFreeze(!AttackManager.Moveable());
    }
    public float GetSightX()
    {
        return MoveManager.GetSightX();
    }
}
