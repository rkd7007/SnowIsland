using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 2023/08/03
 * 3D RPG Project
 * 강아름
 * 
 * QuestManager Script
 * 퀘스트 생성 및 퀘스트 관리
 */

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private Quest[] quest;

    [SerializeField]
    private QuestLog questLog;

    private void Awake()
    {
        questLog.AcceptQuest(quest[0]);
        questLog.AcceptQuest(quest[1]);
    }

    public void NPCQuest()
    {
        if (!quest[2].isCheck)
        {
            questLog.AcceptQuest(quest[2]);
            QuestLog.instance.SelectText(quest[2]);
            CheckNextBossMode();
        }
    }

    public void AllQuestCurGoalUpdate()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            if (quest[i].isCheck && !quest[i].MyIsComplete)
                QuestLog.instance.ShowSelectText(quest[i]);
        }
    }

    public void CheckNextBossMode()
    {
        if (quest[0].MyIsComplete && quest[1].MyIsComplete && quest[2].isCheck)
        {
            GameManager.MyInstance.PlayBossDoorOpen();
        }
    }
}
