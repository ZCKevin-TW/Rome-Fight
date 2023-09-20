using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    // Start is called before the first frame update
    private int HP, OriginHP, BigHP, SmallHP;
    private float OriginWidth, OriginHeight, PositionX, PositionY;
    [SerializeField] private GameObject theBar;
    RectTransform rec;


    void Start()
    {
        HP = 12;
        OriginHP = HP;
        BigHP = 3;
        SmallHP = 2;
        rec = theBar.transform as RectTransform;
        OriginWidth = rec.sizeDelta.x;
        OriginHeight = rec.sizeDelta.y;
        PositionX = rec.anchoredPosition.x;
        PositionY = rec.anchoredPosition.y;
        Debug.Log(PositionX);
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseSmallHP();
        System.Threading.Thread.Sleep(1000);
    }

    public void DecreaseBigHP()
    {
        HP -= BigHP;
        ChangeBar();
    }

    public void DecreaseSmallHP()
    {
        HP -= SmallHP;
        ChangeBar();
    }

    private void ChangeBar()
    {
        rec.sizeDelta = new Vector2(OriginWidth * ((float)HP / OriginHP), OriginHeight);
        float offset = (float)(OriginHP - HP) / OriginHP / 2 * OriginWidth;
        rec.anchoredPosition = new Vector2(PositionX-offset, PositionY);
    }
}
