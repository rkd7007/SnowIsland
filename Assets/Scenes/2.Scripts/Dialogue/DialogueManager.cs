using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 2023/07/28
 * 3D RPG Project
 * 강아름
 * 
 * DialogueManager Script
 * 대화 시스템을 관리
 */

public class DialogueManager : MonoBehaviour
{
    #region Singleton

    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }


    #endregion

    public Text nameText;
    public Text dialogueText;

    private int dialogue_count;
    private List<string> ListDialogue;
    private List<string> ListName;

    void Start()
    {
        dialogue_count = 0;
        ListDialogue = new List<string>();
        ListName = new List<string>();
        dialogueText.text = "";
        nameText.text = "";
    }

    public void CreatDialogue(Dialogue dialogue)
    {
        //name
        for (int i = 0; i < dialogue.name.Length; i++)
        {
            ListName.Add(dialogue.name[i]);
            nameText.text = ListName[i];
        }
        //sentences
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {     
            ListDialogue.Add(dialogue.sentences[i]);
        }
    }

    public IEnumerator StartDialogueCoroutine()
    {
        //한 글자씩 출력
        dialogueText.text = "";

        for (int i = 0; i < ListDialogue[dialogue_count].Length; i++)
        {
            dialogueText.text += ListDialogue[dialogue_count][i];
            yield return new WaitForSeconds(0.03f);
        }
        //대화가 끝났을 경우 초기화
        if (ListDialogue.Count == dialogue_count)
        {
            dialogue_count = 0;
            ListName.Clear();
            ListDialogue.Clear();
        }
        else
        {
            //다음 대화로 넘어감
            dialogue_count++;
            StopAllCoroutines();
        }
    }

    public void StartDialogue()
    {
        StartCoroutine(StartDialogueCoroutine());
    }
    
    public bool IsTalkEnd()
    {
        if (ListDialogue.Count == dialogue_count)
            return true;
        else
            return false;
    }
}
