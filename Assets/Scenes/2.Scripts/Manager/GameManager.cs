using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void KillConfirmed(MonsterManager monster);

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance;

    public static GameManager MyInstance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    #endregion

    public event KillConfirmed KillConfirmedEvent;

    public UnityEvent MainQuest;
    public UnityEvent CurGoalUpdate;
    public UnityEvent CheckQuestforBoss;

    public UnityEvent BossDoorOpenScene;
    public UnityEvent BossScene;

    public void KillConfirmed(MonsterManager monster)
    {
        if (KillConfirmedEvent != null)
        {
            KillConfirmedEvent(monster);
        }
    }

    public void MainQuestSelect()
    {
        MainQuest.Invoke();
    }

    public void QuestCurGoalUpdate()
    {
        CurGoalUpdate.Invoke();
    }

    public void CheckQuestBoss()
    {
        CheckQuestforBoss.Invoke();
    }

    public void PlayBossDoorOpen()
    {
        BossDoorOpenScene.Invoke();
    }

    public void PlayBossScene()
    {
        BossScene.Invoke();
    }
}
