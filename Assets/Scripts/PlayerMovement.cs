using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float SightXOffset = 3, SightYOffset = 5;
    private float MoveLimit = 9;
    [SerializeField] private GameObject Sight;
    Transform SightTrans;
    // Start is called before the first frame update
    void Start()
    { 
        rd = GetComponent<Rigidbody2D>();
        SightTrans = Sight.transform;
    }
    private float GetX(float x)
    {
        if (x > MoveLimit) x = MoveLimit;
        if (x < -MoveLimit + SightXOffset) x = -MoveLimit + SightXOffset;
        return x;
    }
    // Update is called once per frame
    
    void Update()
    {
        float ns = Input.GetAxisRaw("Horizontal");
        rd.velocity = new Vector2(ns, 0) * speed;
        rd.position = new Vector2(GetX(rd.position.x), rd.position.y);
        SightTrans.position = new Vector2(rd.position.x - SightXOffset, rd.position.y + SightYOffset);
        //Debug.Log(SightTrans.position.x);
    }

    public float GetSightX()
    {
        return SightTrans.position.x;
    }
}
