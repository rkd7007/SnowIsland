using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region Singleton

    public static ParticleManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField]
    private GameObject DustObj;

    [SerializeField]
    private ParticleSystem Dustparticle;

    private void Start()
    {
        DustObj.SetActive(false);
    }

    private void Update()
    {
        if (CameraController.MyInstance.OpenBossRoom())
            DustObj.SetActive(false);
    }

    public void DustCreate()
    {
        Dustparticle.Play();
    }

}
