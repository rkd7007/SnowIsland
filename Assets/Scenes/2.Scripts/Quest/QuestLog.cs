using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 2023/08/03
 * 3D RPG Project
 * 강아름
 * 
 * QuestLog Script
 * 퀘스트 Prefab 생성 및 Text, UI 등 처리
 */

public class QuestLog : MonoBehaviour
{
    #region Singleton

    public static QuestLog instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField]
    private GameObject questTitlePrefab;

    [SerializeField]
    private GameObject questDescriptionPrefab;

    [SerializeField]
    private GameObject questbutton;

    [SerializeField]
    private Transform questParent;

    private Quest selected;

    [SerializeField]
    private AudioSource[] audioSource;      //0 : 수락, 1 : 보상 획득

    public void AcceptQuest(Quest quest)
    {
        foreach (KillObjective obj in quest.MyKillObjectives)
        {
            GameManager.MyInstance.KillConfirmedEvent += new KillConfirmed(obj.UpdateKillCount);
        }

        quest.MyquestTitlePrefab = Instantiate(questTitlePrefab, questParent);
        quest.MyquestTitlePrefab.GetComponent<Text>().text = quest.MyTitle;

        quest.MyquestDescriptionPrefab = Instantiate(questDescriptionPrefab, questParent);
        quest.MyquestDescriptionPrefab.GetComponent<Text>().text = quest.MyDescription;

        quest.MyquestDescriptionPrefab.SetActive(false);

        quest.Mybutton = Instantiate(questbutton, questParent);
        quest.Mybutton.SetActive(false);

        SelectQuest select = quest.MyquestTitlePrefab.GetComponent<SelectQuest>();
        SelectQuest ButtonSelect = quest.Mybutton.GetComponent<SelectQuest>();

        select.Myquest = quest;
        ButtonSelect.Myquest = quest;

        quest.MySelectQuest = select;
        quest.MySelectQuest = ButtonSelect;
    }

    public void ShowDescription(Quest quest)
    {
        audioSource[0].PlayOneShot(audioSource[0].clip);
        //이미 누른 퀘스트가 있음
        if (selected != null)
        {
            //같은 퀘스트
            if (selected == quest)
            {
                //이미 펼쳐져 있던 퀘스트
                if (quest.MyquestDescriptionPrefab.activeSelf)
                {
                    selected.MySelectQuest.DeSelect();
                    quest.MyquestDescriptionPrefab.SetActive(false);
                    quest.Mybutton.SetActive(false);
                }
                else
                {
                    //같은 퀘스트 다시 엶
                    selected = quest;
                    quest.MyquestDescriptionPrefab.SetActive(true);
                    quest.Mybutton.SetActive(true);
                }
            }
            else
            {
                selected = quest;
                quest.MyquestDescriptionPrefab.SetActive(true);
                quest.Mybutton.SetActive(true);
            }
        }
        //누른 퀘스트가 없음
        else
        {
            selected = quest;
            quest.MyquestDescriptionPrefab.SetActive(true);
            quest.Mybutton.SetActive(true);
        }
    }

    public void SelectText(Quest quest)
    {
        selected = quest;
        selected.isCheck = true;
        ShowSelectText(quest);

        audioSource[0].PlayOneShot(audioSource[0].clip);
    }

    public bool isCheckbool()
    {
        if (selected != null && selected.MySelectQuest.Checkbool())
            return true;
        else
            return false;
    }

    public void CompleteSelect(Quest quest)
    {
        if (selected != null)
        {
            selected = quest;

            foreach (Objective obj in quest.MyKillObjectives)
            {
                if (selected.MySelectQuest.CheckCompletebool() && !obj.MyGivereward)
                {
                    audioSource[1].PlayOneShot(audioSource[1].clip);

                    PlayerManager.instance.GoldSet(obj.MyReward);
                    obj.MyGivereward = true;
                    quest.Mybutton.GetComponent<Text>().text = "보상 획득 완료!";
                }
            }
        }
    }

    public void ShowSelectText(Quest quest)
    {
        selected = quest;

        if (selected != null)
        {
            quest.Mybutton.GetComponent<Text>().text = "수락되었습니다.";

            string MyText = quest.MyDescription;
            string objective = string.Empty;

            foreach (Objective obj in quest.MyKillObjectives)
            {
                if (obj.MyCurrentGoal >= obj.MyGoal)
                {
                    if (!obj.MyGivereward)
                    {
                        quest.MyIsComplete = true;
                        isClear(quest);
                    }
                }

                objective += obj.MyTitle + " : " + obj.MyCurrentGoal + " / " + obj.MyGoal + "\n";
            }

            quest.MyquestDescriptionPrefab.GetComponent<Text>().text = string.Format("{0}\n{1}", MyText, objective);
        }
    }

    public void isClear(Quest quest)
    {
        string MyText = quest.MyTitle;
        string objective = string.Empty;
        objective += " - 완료";

        quest.MyquestTitlePrefab.GetComponent<Text>().color = Color.red;
        quest.MyquestTitlePrefab.GetComponent<Text>().text = string.Format("{0}{1}", MyText, objective);
        quest.MyquestDescriptionPrefab.SetActive(false);
        quest.Mybutton.GetComponent<Text>().text = "보상을 획득하세요.";
    }
}
