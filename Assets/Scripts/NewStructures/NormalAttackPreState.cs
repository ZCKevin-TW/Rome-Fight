using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackPreState : PlayerState
{ 
    public override bool Moveable() => false;
    public override PlayerState Refresh()
    {
        starttime = Time.time;
        cancelcnt = 0;
        // animator starts playing
        // anim.setTrigger("idle");
        return this;
    }
    public override PlayerState GetNextStateOfpressDefend()
    {
        if (cancelcnt == 0)
            return defin.Refresh().SetCancelCount(cancelcnt + 1);
        else
            return this;
    }
    public override PlayerState GetNextStateOfbeingNormalAttack()
    {
        return smallhurt.Refresh();
    }
    public override PlayerState GetNextStateOfbeingSideAttack()
    {
        return smallhurt.Refresh();
    }
    public override PlayerState GetNextState()
    {
        if (Time.time - starttime < duration)
            return this;
        return natkin.Refresh().SetCancelCount(cancelcnt);
    }


    // Update is called once per frame
}
