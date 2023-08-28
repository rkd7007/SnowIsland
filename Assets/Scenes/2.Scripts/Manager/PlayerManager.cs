using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/07/21
 * 3D RPG Project
 * 강아름
 * 
 * PlayerManager Script
 * 캐릭터의 정보, 체력, 공격력 등을 관리
 */

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;

    [SerializeField]
    private int HP;
    [SerializeField]
    private int Power;
    [SerializeField]
    private int Speed;
    [SerializeField]
    private int Gold;
    [SerializeField]
    private float attackDelay;

    [SerializeField]
    public HpBar _hpbar;

    public int Player_HP { get => HP; set => HP = value; }
    public int Player_Power { get => Power; set => Power = value; }
    public int Player_Speed { get => Speed; set => Speed = value; }
    public int Player_Gold { get => Gold; set => Gold = value; }
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }

    public void PlayerTakeDmg(int dmg)
    {
        HP -= dmg;
        _hpbar.SetHp(HP);
    }

    public void GoldSet(int gold)
    {
        Gold += gold;
        TextManager.MyInstance.PlayerGoldText(Gold);
    }
   
    public void HpUpgradeSet(int hp, int gold)
    {
        HP += hp;
        TextManager.MyInstance.StackSet("hp", HP, gold);
        _hpbar.SetHp(HP);
    }

    public void PowerUpgradeSet(int power, int gold)
    {
        Power += power;
        TextManager.MyInstance.StackSet("power", Power, gold);
    }

    public void SpeedUpgradeSet(float speed, int gold)
    {
        attackDelay -= speed;
        Speed += 1;
        TextManager.MyInstance.StackSet("speed", Speed, gold);
    }
}
