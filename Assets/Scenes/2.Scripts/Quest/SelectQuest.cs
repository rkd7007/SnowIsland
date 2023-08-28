using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 2023/08/03
 * 3D RPG Project
 * 강아름
 * 
 * SelectQuest Script
 * 선택한 퀘스트에 대한 관리 및 처리
 */

public class SelectQuest : MonoBehaviour
{
    public Quest Myquest;

    public void Select()
    {
        if (!Myquest.MyIsComplete)
        {
            GetComponent<Text>().color = Color.blue;
            QuestLog.instance.ShowDescription(Myquest);
        }
    }

    public void DeSelect()
    {
        Myquest.MyquestTitlePrefab.GetComponent<Text>().color = Color.black;
    }

    public void QuestSelectText()
    {
        if (!Myquest.MyIsComplete)
        {
            QuestLog.instance.SelectText(Myquest);
        }
        else
        {
            QuestLog.instance.CompleteSelect(Myquest);
        }
    }

    public bool CheckCompletebool()
    {
        if (Myquest.MyIsComplete)
            return true;
        else
            return false;
    }

    public bool Checkbool()
    {
        if (Myquest.isCheck)
            return true;
        else
            return false;
    }
}
