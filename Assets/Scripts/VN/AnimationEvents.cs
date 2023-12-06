using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    private void mydisable(Transform Collection)
    {
        foreach (Transform child in transform)
        {
            if (child != Collection)
                child.gameObject.SetActive(false);
        }
    }
    public void SideDoor()
    {
        mydisable(transform.Find("SideDoorCollect"));
    }
    public void FrontDoor()
    {
        mydisable(transform.Find("FrontDoorCollect"));
    }

    public void Inside()
    {
        mydisable(transform.Find("InsideCollect"));
    }
    public void Basement()
    {
        mydisable(transform.Find("BasementCollect"));
    }

    public void Middle()
    {
        mydisable(transform.Find("MiddleCollect"));
    }
}
