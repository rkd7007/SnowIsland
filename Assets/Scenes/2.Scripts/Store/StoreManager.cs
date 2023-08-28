using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    #region Singleton

    private static StoreManager instance;
    public static StoreManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StoreManager>();
            }
            return instance;
        }
    }

    #endregion

    [SerializeField] GameObject hpButton, PowerButton, RangeButton;

    private int uphp = 30;
    private int upPower = 10;
    private float upSpeed = 0.2f;

    private int[] upGold = { 10, 10, 10 };

    public void UpGoldSet(int num, int gold)
    {
        upGold[num] = gold;
    }

    public void UpgradeHP()
    {
        if (PlayerManager.instance.Player_Gold >= upGold[0])
        {
            PlayerManager.instance.Player_Gold -= upGold[0];
            upGold[0] += 10;
            PlayerManager.instance.HpUpgradeSet(uphp, upGold[0]);
            TextManager.MyInstance.PlayerGoldText(PlayerManager.instance.Player_Gold);
        }
    }

    public void UpgradePower()
    {
        if (PlayerManager.instance.Player_Gold >= upGold[1])
        {
            PlayerManager.instance.Player_Gold -= upGold[1];
            upGold[1] += 10;
            PlayerManager.instance.PowerUpgradeSet(upPower, upGold[1]);
            TextManager.MyInstance.PlayerGoldText(PlayerManager.instance.Player_Gold);
        }
    }

    public void UpgradeSpeed()
    {
        if (PlayerManager.instance.Player_Gold >= upGold[2])
        {
            PlayerManager.instance.Player_Gold -= upGold[2];
            upGold[2] += 10;
            PlayerManager.instance.SpeedUpgradeSet(upSpeed, upGold[2]);
            TextManager.MyInstance.PlayerGoldText(PlayerManager.instance.Player_Gold);
        }
    }
}
