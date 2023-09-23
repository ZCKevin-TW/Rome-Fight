using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rd;
    [SerializeField] private float speed = 90;
    // private float SightXOffset = 3, SightYOffset = 5;
    private float MoveLimit = 9;
    private int Frozen = 0;
    [SerializeField] private GameObject Sight;
    Transform SightTrans;
    // Start is called before the first frame update
    void Start()
    { 
        rd = GetComponent<Rigidbody2D>();
        SightTrans = Sight.transform;
        Frozen = 0;
    }
    float ClampSpeed(float dx)
    {
        if (rd.position.x <= -MoveLimit)
            return Mathf.Max(dx, 0f);
        if (rd.position.x >= MoveLimit)
            return Mathf.Min(dx, 0f);
        if (Frozen > 0) dx = 0f;
        return dx;
    }
    // Update is called once per frame 
    void Update()
    {
        float dx = Input.GetAxisRaw("Horizontal");
        rd.velocity = new Vector2(ClampSpeed(dx), 0) * speed;
        rd.position = new Vector2(Mathf.Clamp(rd.position.x, -MoveLimit, MoveLimit), rd.position.y);
        Debug.Log(new Vector2(ClampSpeed(dx), 0) * speed);
        Debug.Log(rd.position.x);
        // SightTrans.position = new Vector2(rd.position.x - SightXOffset, rd.position.y + SightYOffset);
        //Debug.Log(SightTrans.position.x);
    }

    public float GetSightX()
    {
        return SightTrans.position.x;
    }
    public void Freeze()
    {
        ++Frozen;
    }
    public void UnFreeze()
    {
        --Frozen;
    }
}
