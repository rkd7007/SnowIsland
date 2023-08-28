using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject QuestUI;
    private bool isOn = false;

    void Start()
    {
        isOn = false;
        QuestUI.SetActive(false);
    }

    public void QuestUIOnOff()
    {
        if (isOn)
        {
            QuestUI.SetActive(false);
            isOn = false;
        }
        else
        {
            QuestUI.SetActive(true);
            isOn = true;
        }
    }
}
