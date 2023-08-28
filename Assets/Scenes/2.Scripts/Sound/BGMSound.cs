using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSound : MonoBehaviour
{
    #region Singleton

    public static BGMSound instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    //0 : 기본 배경 음악, 1 : 보스 배경 음악, 2 : 클리어 배경 음악
    [SerializeField] private AudioSource[] audioSource;

    void Start()
    {
        audioSource[0].Play();
    }

    public void ChangeBGM(int i)
    {
        switch (i)
        {
            case 1:
                {
                    audioSource[0].Stop();

                    if (!audioSource[i].isPlaying)
                        audioSource[i].Play();
                }
                break;
            case 2:
                {
                    audioSource[1].Stop();

                    if (!audioSource[i].isPlaying)
                        audioSource[i].Play();
                }
                break;
        }

    }
}
