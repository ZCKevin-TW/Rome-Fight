using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    protected Player player;
    [SerializeField] private float leftBorderoffset, rightborderoffset;
    [SerializeField] private float aimpointoffset;
    [SerializeField] protected float starttime, duration;
    [SerializeField]
    protected PlayerState
        natkpre,
        natkin,
        natkpost,
        natkblocked,
        satkpre,
        satkin,
        satkpost,
        defin,
        defpost,
        dashleft,
        dashright,
        smallhurt,
        bighurt,
        idle;
        
    protected int cancelcnt = 0;
    private void Start()
    {
        player = GetComponent<Player>();
    }
    public virtual bool Moveable()
    {
        return true;
    }
    public float GetCenterPos()
    {
        return player.getOriginX() + (leftBorderoffset + rightborderoffset) / 2;
    }
    public bool PointInsidethis(float x)
    {
        x -= player.getOriginX();
        return leftBorderoffset <= x && x <= rightborderoffset;
    }
    public virtual bool DefendWorking()
    {
        return false;
    }
    public virtual bool AttackWorking()
    {
        return false;
    }
    public float GetAttackPoint()
    {
        return player.getOriginX() + aimpointoffset;
    }
    public abstract PlayerState Refresh();
    public PlayerState SetCancelCount(int x)
    {
        cancelcnt = x;
        return this;
    }
    public virtual PlayerState GetNextStateOfpressAttack()
    {
        return this;
    }
    public virtual PlayerState GetNextStateOfpressSideAttack()
    {
        return this;
    }
    public virtual PlayerState GetNextStateOfpressDefend()
    {
        return this;
    }
    public virtual PlayerState GetNextStateOfpressDash(float dx)
    {
        return this;
    }
    public virtual PlayerState GetNextStateOfbeingBlocked()
    {
        return this;
    }
    public virtual PlayerState GetNextStateOfbeingNormalAttack()
    {
        return this;
    }
    public virtual PlayerState GetNextStateOfbeingSideAttack()
    {
        return this;
    }
    public virtual PlayerState GetNextState()
    {
        return this;
    }
}
