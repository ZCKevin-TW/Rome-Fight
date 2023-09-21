using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum Mode
    {
        AttackPre,
        AttackActive,
        AttackBackSwing,
        AttackCoolDown,
        DefendPre,
        DefendActive,
        DefendBackSwing,
        DefendCoolDown,
    };
    private Rigidbody2D rd;
    private float speed = 4;
    // Start is called before the first frame update
    void Start()
    { 
        rd = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float ns = Input.GetAxisRaw("Horizontal");
        rd.velocity = new Vector2(ns, 0) * speed; 


    }
}
