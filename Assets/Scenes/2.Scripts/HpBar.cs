using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 2023/07/20
 * 3D RPG Project
 * 강아름
 * 
 * HpBar Script
 * 캐릭터와 몬스터의 체력 바를 관리
 */

public class HpBar : MonoBehaviour
{
    private Slider _hpSlider;

    public void Awake()
    {
        _hpSlider = GetComponent<Slider>();
    }

    public void SetMaxHp(int maxHp)
    {
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = maxHp;
    }

    public void SetHp(int maxHp)
    {
        _hpSlider.value = maxHp;
    }

    public void SetFalse()
    {
        _hpSlider.gameObject.SetActive(false);
    }

    public void SetTrue()
    {
        _hpSlider.gameObject.SetActive(true);
    }
}
