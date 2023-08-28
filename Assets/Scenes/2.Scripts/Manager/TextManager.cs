using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 2023/07/21
 * 3D RPG Project
 * 강아름
 * 
 * TextManager Script
 * Text를 생성
 */

public class TextManager : MonoBehaviour
{
    #region Singleton

    private static TextManager instance;
    public static TextManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TextManager>();
            }
            return instance;
        }
    }

    #endregion

    [SerializeField] GameObject TextPrefab, p_TextPrefab;

    [SerializeField] GameObject GoldText;
    [SerializeField] GameObject HpText, PowerText, SpeedText;
    [SerializeField] GameObject HpGoldText, PowerGoldText, SpeedGoldText;

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        GoldText.GetComponent<Text>().text = string.Format("{0}", PlayerManager.instance.Player_Gold);
        HpText.GetComponent<Text>().text = string.Format("현재 : {0}", PlayerManager.instance.Player_HP);
        PowerText.GetComponent<Text>().text = string.Format("현재 : {0}", PlayerManager.instance.Player_Power);
        SpeedText.GetComponent<Text>().text = string.Format("현재 : {0}", PlayerManager.instance.Player_Speed);

        HpGoldText.GetComponent<Text>().text = string.Format("필요 골드 : {0}", 10);
        PowerGoldText.GetComponent<Text>().text = string.Format("필요 골드 : {0}", 10);
        SpeedGoldText.GetComponent<Text>().text = string.Format("필요 골드 : {0}", 10);
    }

    public void PopupPlayerTakeDmgText(int _massage)
    {
        if (p_TextPrefab) // != null
        {
            //생성 할 오브젝트, 생성 위치, 회전, 클론을 생성하여 담을 부모 객체
            //카메라가 바라보는 방향을 가져와 화면에 비춰지도록 함
            var _text = Instantiate(p_TextPrefab,
                new Vector3(PlayerManager.instance.player.transform.position.x,
                PlayerManager.instance.player.transform.position.y + 1.4f, PlayerManager.instance.player.transform.position.z),
                 CameraController.MyInstance.transform.rotation, transform);
            _text.GetComponent<TextMesh>().text = _massage.ToString();

            HpText.GetComponent<Text>().text = string.Format("현재 : {0}", PlayerManager.instance.Player_HP);
        }
    }

    public void PopupMonsterTakeDmgText(int _massage, Vector3 pos)
    {
        if (TextPrefab)
        {
            audioSource.PlayOneShot(audioSource.clip);

            var _text = Instantiate(TextPrefab,
                new Vector3(pos.x, pos.y + 1.4f, pos.z), CameraController.MyInstance.transform.rotation, transform);
            _text.GetComponent<TextMesh>().text = _massage.ToString();
        }
    }

    public void PlayerGoldText(int gold)
    {
        if (GoldText)
        {
            GoldText.GetComponent<Text>().text = string.Format("{0}", gold);
        }
    }

    public void StackSet(string name ,int i, int gold)
    {
        switch(name)
        {
            case "hp":
                {
                    HpText.GetComponent<Text>().text = string.Format("현재 : {0}", i);
                    HpGoldText.GetComponent<Text>().text = string.Format("필요 골드 : {0}", gold);
                }
                break;
            case "power":
                {
                    PowerText.GetComponent<Text>().text = string.Format("현재 : {0}", i);
                    PowerGoldText.GetComponent<Text>().text = string.Format("필요 골드 : {0}", gold);
                }
                break;
            case "speed":
                {
                    SpeedText.GetComponent<Text>().text = string.Format("현재 : {0}", i);
                    SpeedGoldText.GetComponent<Text>().text = string.Format("필요 골드 : {0}", gold);
                }
                break;
        }
    }
}
