using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/07/23
 * 3D RPG Project
 * 강아름
 * 
 * Sword Script
 * 캐릭터의 공격인 검의 이동 및 Pool에서 삭제
 */

public class Sword : Poolable
{
    [SerializeField] private float _speed;

    void Update()
    {
        Vector3 dir = transform.forward;
        transform.position += dir * _speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Push();
    }
}
