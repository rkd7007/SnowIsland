using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    public bool isCheck;

    [SerializeField]
    private string title;

    [SerializeField]
    [TextArea(3, 5)]
    private string description;

    [SerializeField]
    private KillObjective[] KillObjectives;

    private bool IsComplete;

    public SelectQuest MySelectQuest { get; set; }

    public GameObject MyquestTitlePrefab;
    public GameObject MyquestDescriptionPrefab;
    public GameObject Mybutton;

    public string MyTitle { get => title; set => title = value; }
    public string MyDescription { get => description; set => description = value; }
    public KillObjective[] MyKillObjectives { get => KillObjectives; set => KillObjectives = value; }
    public bool MyIsComplete { get => IsComplete; set => IsComplete = value; }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int goal;

    [SerializeField]
    private int currentGoal;

    [SerializeField]
    private int reward;

    [SerializeField]
    private bool givereward;

    [SerializeField]
    private string title;

    [SerializeField]
    private string type;

    public int MyGoal { get => goal; set => goal = value; }
    public string MyTitle { get => title; set => title = value; }
    public string MyType { get => type; set => type = value; }
    public int MyCurrentGoal { get => currentGoal; set => currentGoal = value; }
    public int MyReward { get => reward; set => reward = value; }
    public bool MyGivereward { get => givereward; set => givereward = value; }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(MonsterManager monster)
    {
        if (MyType == monster.Name && QuestLog.instance.isCheckbool())
        {
            MyCurrentGoal++;
            GameManager.MyInstance.QuestCurGoalUpdate();
            GameManager.MyInstance.CheckQuestBoss();
        }
    }
}
