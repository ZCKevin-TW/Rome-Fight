using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    // Start is called before the first frame update
    private int EnemyHP, PlayerHP, OriginHP, BigHP, SmallHP;
    private float EnemyOriginWidth, EnemyOriginHeight, EnemyPositionX, EnemyPositionY;
    private float PlayerOriginWidth, PlayerOriginHeight, PlayerPositionX, PlayerPositionY;
    [SerializeField] private GameObject EnemyHPBar;
    [SerializeField] private GameObject PlayerHPBar;
    RectTransform EnemyRec, PlayerRec;


    void Start()
    {
        EnemyHP = 12;
        PlayerHP = EnemyHP;
        OriginHP = EnemyHP;
        BigHP = 3;
        SmallHP = 2;
        
        EnemyRec = EnemyHPBar.transform as RectTransform;
        EnemyOriginWidth = EnemyRec.sizeDelta.x;
        EnemyOriginHeight = EnemyRec.sizeDelta.y;
        EnemyPositionX = EnemyRec.anchoredPosition.x;
        EnemyPositionY = EnemyRec.anchoredPosition.y;

        PlayerRec = PlayerHPBar.transform as RectTransform;
        PlayerOriginWidth = PlayerRec.sizeDelta.x;
        PlayerOriginHeight = PlayerRec.sizeDelta.y;
        PlayerPositionX = PlayerRec.anchoredPosition.x;
        PlayerPositionY = PlayerRec.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDecreaseSmallHP();
        PlayerDecreaseBigHP();
        System.Threading.Thread.Sleep(1000);
    }

    public void EnemyDecreaseBigHP()
    {
        EnemyHP -= BigHP;
        EnemyChangeBar();
    }

    public void EnemyDecreaseSmallHP()
    {
        EnemyHP -= SmallHP;
        EnemyChangeBar();
    }

    private void EnemyChangeBar()
    {
        EnemyRec.sizeDelta = new Vector2(EnemyOriginWidth * ((float)EnemyHP / OriginHP), EnemyOriginHeight);
        float offset = (float)(OriginHP - EnemyHP) / OriginHP / 2 * EnemyOriginWidth;
        EnemyRec.anchoredPosition = new Vector2(EnemyPositionX-offset, EnemyPositionY);
    }

    public void PlayerDecreaseBigHP()
    {
        PlayerHP -= BigHP;
        PlayerChangeBar();
    }

    public void PlayerDecreaseSmallHP()
    {
        PlayerHP -= SmallHP;
        PlayerChangeBar();
    }

    private void PlayerChangeBar()
    {
        PlayerRec.sizeDelta = new Vector2(PlayerOriginWidth * ((float)PlayerHP / OriginHP), PlayerOriginHeight);
        float offset = (float)(OriginHP - PlayerHP) / OriginHP / 2 * PlayerOriginWidth;
        PlayerRec.anchoredPosition = new Vector2(PlayerPositionX - offset, PlayerPositionY);
    }
}
