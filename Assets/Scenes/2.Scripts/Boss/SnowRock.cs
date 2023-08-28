using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/08/17
 * 3D RPG Project
 * 강아름
 * 
 * SnowRock Script
 * 보스의 공격인 눈덩이의 이동 및 Pool에서 삭제
 */

public class SnowRock : Poolable
{
    private Rigidbody rig;
    private float _speed;

    void Start()
    {
        _speed = Random.Range(2, 15);
        rig = GetComponent<Rigidbody>();
        StartCoroutine(DownSnowRock());
    }

    private void Update()
    {
        if (!CameraController.MyInstance.EnterBossRoom())
            Push();
    }

    IEnumerator DownSnowRock()
    {       
        rig.velocity = Vector3.down * _speed;
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2.5f);
        Push();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "boss")
        {
            Push();
        }
        if (collision.gameObject.tag == "Player")
        {
            TextManager.MyInstance.PopupPlayerTakeDmgText(MonsterManager.instance.Power);
            PlayerManager.instance.PlayerTakeDmg(MonsterManager.instance.Power);
            Push();
        }
        //보스, 캐릭터에 닿여 이미 사라진 경우(null)는 무시한다
        if (this.gameObject.activeSelf)
            StartCoroutine(Destroy());
    }
}
