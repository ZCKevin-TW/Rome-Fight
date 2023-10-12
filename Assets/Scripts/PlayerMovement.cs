using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rd;
    [SerializeField] private float speed = 20;
    // private float SightXOffset = 3, SightYOffset = 5;
    private float MoveLimit = 8;
    private bool Frozen = false;
    // Start is called before the first frame update
    void Awake()
    { 
        rd = GetComponent<Rigidbody2D>();
        Frozen = false;
    }
    public bool HitWall()
    {
        return Mathf.Abs(rd.position.x) == MoveLimit;
    }

    float ClampSpeed(float dx)
    {
        if (rd.position.x <= -MoveLimit)
            return Mathf.Max(dx, 0f);
        if (rd.position.x >= MoveLimit)
            return Mathf.Min(dx, 0f);
        if (Frozen) dx = 0f;
        return dx;
    } 
    // Update is called once per frame 
    public void SetInput(float dx)
    {
        rd.velocity = new Vector2(ClampSpeed(dx), 0) * speed;
        rd.position = new Vector2(Mathf.Clamp(rd.position.x, -MoveLimit, MoveLimit), rd.position.y); 
    }

    public void SetFreeze(bool val)
    {
        Frozen = val;
    }
}
