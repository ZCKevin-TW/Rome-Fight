using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStrategy : MonoBehaviour
{
    // Start is called before the first frame update
    protected Player PlayerAPI;

    public virtual void Start()
    {
        PlayerAPI = GetComponent<Player>();
    } 
    public abstract void ReactToAttack();
}
