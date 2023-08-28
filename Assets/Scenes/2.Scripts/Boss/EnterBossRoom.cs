using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossRoom : MonoBehaviour
{
    public GameObject[] ui;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.MyInstance.PlayBossScene();
        this.gameObject.SetActive(false);
        BGMSound.instance.ChangeBGM(1);

        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].SetActive(false);
        }
    }
}
