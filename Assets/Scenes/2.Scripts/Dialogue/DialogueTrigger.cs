using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/07/28
 * 3D RPG Project
 * 강아름
 * 
 * DialogueTrigger Script
 * 대화 상호작용 발생 시 이를 관리
 */

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    Transform target;
    public float nearRadius;
    public GameObject canva;                //F Key UI
    public GameObject dialogue_canva;       //NPC UI
    public static bool isTalking = false;

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        target = PlayerManager.instance.player.transform; //캐릭터 정보 받아옴
        canva.SetActive(false);
        dialogue_canva.SetActive(false);
    }

    void Update()
    {
        if (NearTarget() && !isTalking)
        {
            canva.SetActive(true);
            isTalking = false;
        }
        else
        {
            canva.SetActive(false);
        }
        //대화 상호작용 시작, 정보 담음
        if (NearTarget() && Input.GetKeyDown(KeyCode.F) && !isTalking)
        {
            audioSource.PlayOneShot(audioSource.clip);
            isTalking = true;
            dialogue_canva.SetActive(true);
            DialogueManager.instance.CreatDialogue(dialogue);
        }
        //대화 중일 때
        if (isTalking && Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.instance.StartDialogue();

            //대화가 끝났는지 판단, 끝났으면 초기화
            if (DialogueManager.instance.IsTalkEnd())
            {
                dialogue_canva.SetActive(false);
                isTalking = false;
                //퀘스트 추가
                GameManager.MyInstance.MainQuestSelect();
            }
        }
    }

    private bool NearTarget()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= nearRadius)
        {
            return true;
        }
        else return false;
    }
}
