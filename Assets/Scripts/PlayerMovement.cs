using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rd;
    [SerializeField] private float speed = 20;
    [Range(0f, 20f)]
    [SerializeField] private float dashDistance = 4f;
    [Range(0f, 10f)]
    [SerializeField] private float DashCDTime = 2f;
    // private float SightXOffset = 3, SightYOffset = 5;
    private float MoveLimit = 8;
    private Coroutine LastDash = null;
    private bool InDash = false;
    private bool Frozen = false;
    private int LastDirection = 0;

    [SerializeField] private AudioSource dashSound;
    void Awake()
    { 
        rd = GetComponent<Rigidbody2D>();
        Frozen = false;
    }
    public float GetPos()
    {
        return rd.position.x;
    }
    public bool HitWall()
    {
        return Mathf.Abs(rd.position.x) == MoveLimit;
    }
    public void Cancel()
    {
        if (LastDash != null)
        {
            StopCoroutine(LastDash);
            InDash = false;
        }
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
    void ClampPosition()
    {
        rd.position = new Vector2(Mathf.Clamp(rd.position.x, -MoveLimit, MoveLimit), rd.position.y); 
    }
    // Update is called once per frame 
    public void SetInput(float dx)
    {
        LastDirection = dx < 0f ? -1 : dx > 0f ? 1 : 0;
        rd.velocity = new Vector2(ClampSpeed(dx), 0) * speed;
        ClampPosition();
    }
    IEnumerator DashCD()
    {
        InDash = true;
        yield return new WaitForSeconds(DashCDTime);
        InDash = false;
        LastDash = null;
    }
    public void Dash(float dx)
    {
        if (InDash || Frozen) return;
        // accept range: -1, 0, 1
        if (dx == -1)
        {
            // Left Dash
            // TODO: Left Dash Animation
        }
        if (dx == 1)
        {
            // Right Dash
            // TODO: Right Dash Animation
        }
        dashSound.Play();
        rd.position = new Vector2(rd.position.x + dx * dashDistance, rd.position.y);
        ClampPosition();
        if (dx != 0)
            LastDash = StartCoroutine(DashCD());
    }
    public void SetFreeze(bool val)
    {
        Frozen = val;
    }
}
