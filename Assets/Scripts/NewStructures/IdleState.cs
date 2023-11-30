using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    // Start is called before the first frame update
    // Update is called once per frame
    public override PlayerState Refresh()
    {
        cancelcnt = 0; 
        player.anim.SetTrigger("idle");
        return this;
    }
}
