using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerState CurrentState;
    [SerializeField] private bool InputFromUser = true;
    [SerializeField] private HpBar HpManager;
    [SerializeField] private Flash flashEffect;
    private PlayerMovement MoveManager;
    private EnemyStrategy Brain;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>(); 
        MoveManager = GetComponent<PlayerMovement>();
        Brain = GetComponent<EnemyStrategy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputFromUser)
        {
            if (Input.GetButton("Fire1")) pressAttack();
            if (Input.GetButton("Fire2")) pressDefend();
            float dx = Input.GetAxisRaw("Horizontal");
            pressMove(dx);
            pressDash(dx);
        }
    }
    public void pressAttack()
    {
        CurrentState = CurrentState.GetNextStateOfpressAttack();
    }
    public void pressSideAttack()
    {
        CurrentState = CurrentState.GetNextStateOfpressSideAttack(); 
    }
    public void pressDefend()
    {
        CurrentState = CurrentState.GetNextStateOfpressDefend(); 
    }
    public void pressMove(float dx)
    {
        MoveManager.SetInput(dx);
        MoveManager.SetFreeze(!CurrentState.Moveable()); 
    }
    public void pressDash(float dx)
    { 
        CurrentState = CurrentState.GetNextStateOfpressDash(dx); 
    }
    public float getOriginX()
    {
        return MoveManager.GetPos();
    }

}
