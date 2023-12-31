using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    // Start is called before the first frame update
    private int OriginHP, HP;
    private float OriginWidth, OriginHeight, PositionX, PositionY;
    private bool Started = false;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private GameController GameController;
    [SerializeField] private bool isPlayer;
    RectTransform Rec;
    //[SerializeField] private PlayerControll playerControll;

    void Start()
    {
        OriginHP = 12;
        HP = OriginHP;
        
        Rec = HPBar.transform as RectTransform;
        OriginWidth = Rec.sizeDelta.x;
        OriginHeight = Rec.sizeDelta.y;
        PositionX = Rec.anchoredPosition.x;
        PositionY = Rec.anchoredPosition.y;

    }

    // Update is called once per frame
    public void DecreaseHP(int amount) 
    {
        if (!Started) return;

        HP = Mathf.Max(0, HP - amount);

        if (HP == 0)
        {
            GameController.GameFinished(isPlayer);
        }

        ChangeBar();
    } 
    private void ChangeBar()
    {
        Rec.sizeDelta = new Vector2(OriginWidth * ((float)HP / OriginHP), OriginHeight);
        //float offset = (float)(OriginHP - HP) / OriginHP / 2 * OriginWidth;
        Rec.anchoredPosition = new Vector2(PositionX, PositionY);
    }

    public void SetStarted(bool target)
    {
        Started = target;
    }
}
