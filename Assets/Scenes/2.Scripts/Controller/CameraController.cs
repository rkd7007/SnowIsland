using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    #region Singleton

    private static CameraController instance;

    public static CameraController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraController>();
            }
            return instance;
        }
    }

    #endregion

    public Transform target;

    private float xVelocity = 0.0f;
    private float zVelocity = 0.0f;
    private float yVelocity = 0.0f;

    private float smoothTime = 0.3f;

    private float xMove;
    private float yMove;
    private float xRot;
    private float yRot = -90f;

    private bool isBossDoor, isBossRoom, isNotMove = false;

    [SerializeField]
    private GameObject bossDoor, bossScene;

    private void Awake()
    {
        bossDoor.SetActive(false);
        bossScene.SetActive(false);
    }

    public bool EnterBossRoom()
    {
        if (!isBossRoom)
            return false;
        else
            return true;
    }

    public bool OpenBossRoom()
    {
        if (!isBossDoor)
            return false;
        else
            return true;
    }

    public bool MoveBoolCheck()
    {
        if (!isNotMove)
            return false;
        else
            return true;
    }

    public void BossDieChangeCamera()
    {
        isBossRoom = false;
    }

    //캐릭터의 움직임이 Update에서 처리되기에 캐릭터 움직임 이후 카메라를 움직이기 위해 LateUpdate 사용
    private void LateUpdate()
    {
        if (!isBossRoom)
        {
            xMove = 6.12f;
            yMove = 3.29f;
            xRot = 23.7f;
            this.GetComponent<CinemachineVirtualCamera>().m_Lens.FarClipPlane = 20f;
        }
        else
        {
            xMove = 8.7f;
            yMove = 12.13f;
            xRot = 53.89f;
            this.GetComponent<CinemachineVirtualCamera>().m_Lens.FarClipPlane = 50f;
        }

        float TargetWidth = Mathf.SmoothDamp(transform.position.x, target.position.x + xMove, ref xVelocity, smoothTime);
        float TargetHeight = Mathf.SmoothDamp(transform.position.y, target.position.y + yMove, ref yVelocity, smoothTime);
        float TargetDepth = Mathf.SmoothDamp(transform.position.z, target.position.z, ref zVelocity, smoothTime);

        transform.position = new Vector3(TargetWidth, TargetHeight, TargetDepth);

        float TargetXRot = transform.rotation.x + xRot;
        float TargetYRot = transform.rotation.y + yRot;

        transform.rotation = Quaternion.Euler(TargetXRot, TargetYRot, 0f);
    }

    public void PlaySceneBossDoor()
    {
        isNotMove = true;
        bossDoor.SetActive(true);
        ParticleManager.instance.DustCreate();
        Invoke("DoorDeley", 3.5f);
    }

    public void PlaySceneBoss()
    {
        isNotMove = true;
        bossScene.SetActive(true);
        Invoke("BossDeley", 4.2f);
    }

    public void DoorDeley()
    {
        bossDoor.SetActive(false);
        isBossDoor = true;
        isNotMove = false;
    }

    public void BossDeley()
    {
        bossScene.SetActive(false);
        isBossRoom = true;
        isNotMove = false;
    }
}
