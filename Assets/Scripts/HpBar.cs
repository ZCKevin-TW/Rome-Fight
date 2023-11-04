using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    // Start is called before the first frame update
    private int OriginHP, BigHP, SmallHP, HP;
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
        BigHP = 3;
        SmallHP = 2;
        
        Rec = HPBar.transform as RectTransform;
        OriginWidth = Rec.sizeDelta.x;
        OriginHeight = Rec.sizeDelta.y;
        PositionX = Rec.anchoredPosition.x;
        PositionY = Rec.anchoredPosition.y;

    }

    // Update is called once per frame
    public void DecreaseHP(bool IsBig)
    {
        if (!Started) return;
        if (IsBig)
            HP -= BigHP;
        else
            HP -= SmallHP;

        if (HP <= 0)
        {
            HP = 0;
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
