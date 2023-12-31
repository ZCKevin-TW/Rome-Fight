using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class PlayerState : MonoBehaviour
{
    protected Player player;

    // Visual Effects
    [SerializeField] private GameObject normalHitImage;
    [SerializeField] private GameObject sideHitImage;
    [SerializeField] private float hitEffectTime = .2f;
    [SerializeField] private GameObject star;
    [SerializeField] private Flash flashEffect;

    public int cancelcnt = 0;
    private bool isHitting;
    private Coroutine lastrountine = null;
    [SerializeField] private Transform lbd, rbd, dlbd, drbd, nap, sap, winpos, losepos;
    private float leftBorder
    {
        get => lbd.position.x; 
    }
    private float rightBorder
    {
        get => rbd.position.x;
    }
    private float dizzyLeftBorder
    {
        get => dlbd.position.x;
    }
    private float dizzyRightBorder
    {
        get => drbd.position.x;
    }
    public float normalaimpoint
    {
        get => nap.position.x;
    }
    public float sideaimpoint
    {
        get => sap.position.x;
    }
    protected float starttime;
    private AudioSource audioplayer;
    [SerializedDictionary("Statetype", "Duration(sec)")] 
    public SerializedDictionary<StateType, float> durationOfState = new SerializedDictionary<StateType, float>
    {
        { StateType.natkpre, 0 },
        { StateType.natkin, 0 },
        { StateType.natkpost, 0 },
        { StateType.natkblocked, 0 },
        { StateType.satkpre, 0 },
        { StateType.satkin, 0 },
        { StateType.satkpost, 0 },
        { StateType.defin, 0 },
        { StateType.defpost, 0 },
        { StateType.dashleft, 0 },
        { StateType.dashright, 0 },
        { StateType.dashpost, 0 },
        { StateType.smallhurt, 0 },
        { StateType.bighurt, 0 },
        { StateType.idle, 0 }
    };
    public SerializedDictionary<StateType, StateType> DefaultNextStateOfState = new SerializedDictionary<StateType, StateType>
    {
        { StateType.natkpre, StateType.natkin },
        { StateType.natkin, StateType.natkpost },
        { StateType.natkpost, StateType.idle},
        { StateType.natkblocked, StateType.idle },
        { StateType.satkpre, StateType.satkin },
        { StateType.satkin, StateType.satkpost },
        { StateType.satkpost, StateType.idle },
        { StateType.defin, StateType.defpost },
        { StateType.defpost, StateType.idle },
        { StateType.dashleft, StateType.dashpost },
        { StateType.dashright, StateType.dashpost },
        { StateType.dashpost, StateType.idle },
        { StateType.smallhurt, StateType.idle },
        { StateType.bighurt, StateType.idle },
        { StateType.idle, StateType.idle }
    };
    // the string is triggered when entering a new state
    public SerializedDictionary<StateType, string> AnimTriggerNameOfState = new SerializedDictionary<StateType, string>
    {
        // TODO: Hook up with animators
        // edit in inspector
        { StateType.natkpre, "natkpre" },
        { StateType.natkin, "natkin" },
        { StateType.natkpost, "natkpost" },
        { StateType.natkblocked, "natkblocked" },
        { StateType.satkpre, "satkpre" },
        { StateType.satkin, "satkin" },
        { StateType.satkpost, "satkpost" },
        { StateType.defin, "defin" },
        { StateType.defpost, "defpost" },
        { StateType.dashleft, "dashleft" },
        { StateType.dashright, "dashright" },
        { StateType.dashpost, "dashpost" },
        { StateType.smallhurt, "smallhurt" },
        { StateType.bighurt, "bighurt" },
        { StateType.idle, "idle" }
    };
    public SerializedDictionary<StateType, AudioClip> AudioToPlayerOfState = new SerializedDictionary<StateType, AudioClip>
    {
        // TODO
        { StateType.natkpre, null},
        { StateType.natkin, null},
        { StateType.natkpost, null},
        { StateType.natkblocked, null},
        { StateType.satkpre, null},
        { StateType.satkin, null},
        { StateType.satkpost, null},
        { StateType.defin, null},
        { StateType.defpost, null},
        { StateType.dashleft, null},
        { StateType.dashright, null},
        { StateType.dashpost, null},
        { StateType.smallhurt, null},
        { StateType.bighurt, null},
        { StateType.idle, null}
    };
    [SerializeField] protected float dashoffset = 4;
    [SerializeField] protected int bighurtdamage = 3, smallhurtdamage = 2;
    public float LeftBorder
    {
        get => (IsDizzy() ? dizzyLeftBorder: leftBorder);
        set { }
    }
    public float Rightborder
    {
        get => (IsDizzy() ? dizzyRightBorder: rightBorder);
        set { }
    }
    public float Aimpoint
    {
        get 
        {
            var SideAttacks = new List<StateType>{ StateType.satkpre, StateType.satkin, StateType.satkpost };
            return (SideAttacks.Contains(curState) ? sideaimpoint: normalaimpoint);
        }
        set { }
    }
    public float Duration
    {
        get => durationOfState[curState];
    }
    public enum StateType
    {
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
        dashpost,
        smallhurt,
        bighurt,
        idle,
        win,
        lose
    };
    StateType curState = StateType.idle;
    [SerializeField] private List<StateType> MoveableStates = new List<StateType>() {
        StateType.dashleft,
        StateType.dashright,
        StateType.dashpost,
        StateType.idle
    };
    [SerializeField]
    private List<StateType> DefendingStates = new List<StateType>() {
        StateType.defin
    };
    [SerializeField] private List<StateType> InvincibleStates = new List<StateType>() {
        StateType.smallhurt,
        StateType.bighurt
    }; 
    private List<StateType> AllDefendStates = new List<StateType>() {
        StateType.defin,
        StateType.defpost
    }; 
    [SerializeField] private List<StateType> AttackingStates = new List<StateType>() {
        StateType.natkin,
        StateType.satkin
    }; 
    private List<StateType> AllAttackStates = new List<StateType>() {
        StateType.natkpre,
        StateType.natkin,
        StateType.natkpost,
        StateType.natkblocked,
        StateType.satkpre,
        StateType.satkin,
        StateType.satkpost,
    }; 
    [SerializeField] private List<StateType> WeakStates = new List<StateType>() {
        StateType.natkpost,
        StateType.natkblocked,
        StateType.satkpost,
    }; 
    private void Start()
    {
        player = GetComponent<Player>();
        audioplayer = GetComponent<AudioSource>();
    }
    public void Refresh(StateType newState)
    {
        var last_anim_trigger_name = AnimTriggerNameOfState[curState];

        if (curState == StateType.natkblocked)
            audioplayer.Stop();
        if (lastrountine != null)
        { 
            StopCoroutine(lastrountine);
            lastrountine = null;
            isHitting = false;
        }
        // dizzy star animation
        if (curState == StateType.natkblocked)
            star.SetActive(false);
        else if (newState == StateType.natkblocked)
            StartCoroutine(ShowStarAfter(.2f));


        Debug.Log("From old state" + curState);
        starttime = Time.time;
        curState = newState;
        Debug.Log("to new state " + curState);
        if (!IsAttackOrDefend())
            cancelcnt = 0;
        if (curState == StateType.natkin || curState == StateType.satkin)
            lastrountine = StartCoroutine(StartHitAfter(Duration/2));

        // damaged
        if (curState == StateType.smallhurt || curState == StateType.bighurt)
        {
            if (curState == StateType.smallhurt)
                player.decreaseHP(smallhurtdamage);
            if (curState == StateType.bighurt)
                player.decreaseHP(bighurtdamage);

            flashEffect.StartFlash();
        }
        // TODO: need enemy.isHit()

        // the set bool is acting really weird;
        if (last_anim_trigger_name != null)
            player.anim.SetBool(last_anim_trigger_name, false); 
        if (AnimTriggerNameOfState[curState] != null)
            player.anim.SetBool(AnimTriggerNameOfState[curState], true);

        if (AudioToPlayerOfState.ContainsKey(curState))
        {
            // TODO should not be null
            if (AudioToPlayerOfState[curState])
                audioplayer.PlayOneShot(AudioToPlayerOfState[curState]);
        }

        if (curState == StateType.win)
        {
            Debug.Log("win type set position");
            player.SetPosition(winpos);
        }
        if (curState == StateType.lose)
        {
            Debug.Log("lose type set position");

            player.SetPosition(losepos);
        }
    }
    private IEnumerator ShowStarAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        star.SetActive(true);
    }
    private IEnumerator HitSucceedEffect()
    {
        GameObject hitImage = (curState == StateType.natkin) ? normalHitImage : sideHitImage;
        hitImage.SetActive(true);
        hitImage.transform.SetParent(null);
        yield return new WaitForSeconds(hitEffectTime);
        hitImage.transform.SetParent(this.gameObject.transform);
        hitImage.SetActive(false);
        yield return null;
    }
    public void HitSuccess()
    {
        StartCoroutine(HitSucceedEffect());
    }
    private IEnumerator StartHitAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        isHitting = true;
        yield return new WaitForSeconds(.1f);
        isHitting = false; 
    } 
    private void FixedUpdate()
    {
        if (curState == StateType.dashleft)
            player.teleportForDistance(-dashoffset / Duration * Time.deltaTime);
        else if (curState == StateType.dashright)
            player.teleportForDistance(dashoffset / Duration * Time.deltaTime);
    }

    public bool Moveable()
    {
        return MoveableStates.Contains(curState);
    }
    public bool IsDizzy()
    {
        return curState == StateType.natkblocked;
    }
    public float GetCenterPos()
    {
        return (LeftBorder + Rightborder) / 2;
    }
    public bool PointInsidethis(float x)
    {
      // Debug.Log("LB RB " + LeftBorder + " " + Rightborder);
        return LeftBorder <= x && x <= Rightborder;
    }
    public bool IsDefending()
    {
        return DefendingStates.Contains(curState);
    }
    public bool IsInvincible()
    {
        return InvincibleStates.Contains(curState);
    } 
    public bool IsNormalAttacking()
    {
        return curState == StateType.natkin && isHitting;
    }
    public bool IsSideAttacking()
    {
        return curState == StateType.satkin && isHitting;
    }
    private bool IsAttackOrDefend()
    {
        return AllAttackStates.Contains(curState) || AllDefendStates.Contains(curState);
    }
    private bool Cancelable()
    {
        if (cancelcnt > 0 
            || curState == StateType.smallhurt 
            || curState == StateType.bighurt
            || curState == StateType.win
            || curState == StateType.lose
            || curState == StateType.dashleft
            || curState == StateType.dashright
            || IsDizzy())
            return false;
        return true;
    }
    public bool ToNextStateOfpressAttack()
    { 
        if (AllAttackStates.Contains(curState)) return false;
        if (Cancelable()) { 
            if (IsAttackOrDefend())
              ++cancelcnt;
            Refresh(StateType.natkpre);
            return true;
        }
        return false;
    }
    public bool ToNextStateOfpressSideAttack()
    {
        if (AllAttackStates.Contains(curState)) return false;
        if (Cancelable()) {
            if (IsAttackOrDefend())
              ++cancelcnt;
            Refresh(StateType.satkpre);
            return true;
        }
        return false;
    }
    public bool ToNextStateOfpressDefend()
    {
        if (AllDefendStates.Contains(curState)) return false;
        if (Cancelable()) {
            if (IsAttackOrDefend())
              ++cancelcnt;
            Refresh(StateType.defin);
            return true;
        }
        return false;
    }
    public void ToNextStateOfpressDash(float dx)
    {
        if (curState == StateType.idle && dx != 0 || AllDefendStates.Contains(curState))
            Refresh(dx < 0 ? StateType.dashleft : StateType.dashright);
    }
    public void ToNextStateOfbeingBlocked()
    {
     //   Debug.Assert(curState == StateType.natkin);
        Refresh(StateType.natkblocked); 
    }
    public void ToNextStateOfbeingNormalAttack()
    {
        Debug.Assert(curState != StateType.defin);
        Refresh(WeakStates.Contains(curState) ? StateType.bighurt : StateType.smallhurt);
    }
    public void ToNextStateOfbeingSideAttack()
    {
        Refresh(WeakStates.Contains(curState) ? StateType.bighurt : StateType.smallhurt);
    }
    public void ToNextStateOfWin()
    {
        Refresh(StateType.win);
    }
    public void ToNextStateOfLose()
    {
        Refresh(StateType.lose);
    } 
    public void ToNextState()
    {
        if (Time.time - starttime < Duration || Duration == 0)
            return;
        Refresh(DefaultNextStateOfState[curState]);
    }
}
