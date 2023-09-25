using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    private float LeftEdge, RightEdge;
    [SerializeField] private PlayerMovement Player;
    [SerializeField] private HpBar TheBar;

    void Start()
    {
        RightEdge = (float)Random.Range(2, 4);
        LeftEdge = (float)Random.Range(-4, -2);
        Debug.Log(LeftEdge);
        Debug.Log(RightEdge);
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            AttackEvent();
        }
    }

    public void AttackEvent()
    {
        float SightX = Player.GetSightX();
        if (SightX >= LeftEdge && SightX <= RightEdge)
        {
            TheBar.DecreaseBigHP();
            Debug.Log("HIT");
        }
        else
            Debug.Log("MISS");

        Debug.Log(SightX);
    }
}
