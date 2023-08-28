using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/07/21
 * 3D RPG Project
 * 강아름
 * 
 * FloatingText Script
 * 생성 된 Text 클론을 일정 시간 이후 제거
 */

public class FloatingText : MonoBehaviour
{
    public float DestroyTime;

    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
}
