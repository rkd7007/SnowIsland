using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/08/22
 * 3D RPG Project
 * 강아름
 * 
 * SphereAttack Script
 * 보스 공격 패턴 중 광역 공격의 실제 구현
 */

public class SphereAttack : MonoBehaviour
{
    #region Singleton

    private static SphereAttack instance;
    public static SphereAttack MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SphereAttack>();
            }
            return instance;
        }
    }

    #endregion

    [SerializeField]
    private Sphere spherePoint;

    [SerializeField]
    private GameObject spheregameObject;

    [SerializeField]
    private Sphere sphere;

    [SerializeField]
    private Transform pos;

    private bool isReset;

    void Start()
    {
        pos = transform.parent.Find("Createsphere");

        spherePoint.Oripos = transform.localScale;
        spherePoint.Point = Instantiate(spheregameObject, pos);
        spherePoint.Point.SetActive(true);

        MySphereScript isSphere = spherePoint.Point.GetComponent<MySphereScript>();
        isSphere.Mysphere = sphere;
        spherePoint.MyPointSphere = isSphere;

        StartCoroutine(GetRange());
    }

    private void Update()
    {
        if (gameObject.activeSelf && !isReset)
        {
            spherePoint.Point.SetActive(true);
            StartCoroutine(GetRange());
        }
        //if (!spherePoint.Point.activeSelf && isReset)
        //{
        //    StopCoroutine(GetRange());
        //    Reset();
        //}
    }

    IEnumerator GetRange()
    {
        while (spherePoint.MyPointSphere.transform.localScale.x < spherePoint.MyPointSphere.Mysphere.MaxSize.x)
        {
            isReset = true;

            spherePoint.ScaleValue += spherePoint.Speed;
            transform.localScale = spherePoint.Oripos * spherePoint.ScaleValue;

            spherePoint.MyPointSphere.Mysphere.ScaleValue += spherePoint.MyPointSphere.Mysphere.Speed;
            spherePoint.MyPointSphere.transform.localScale = spherePoint.MyPointSphere.Mysphere.Oripos * spherePoint.MyPointSphere.Mysphere.ScaleValue;

            if (transform.localScale.x >= spherePoint.MaxSize.x)
            {
                transform.localScale = spherePoint.MaxSize;
            }
            if (spherePoint.MyPointSphere.transform.localScale.x >= spherePoint.MyPointSphere.Mysphere.MaxSize.x)
            {
                Reset();
                break;
            }

            yield return null;
        }
    }

    public void Reset()
    {
        transform.localScale = spherePoint.Oripos;
        spherePoint.MyPointSphere.transform.localScale = spherePoint.MyPointSphere.Mysphere.Oripos;

        spherePoint.ScaleValue = 1.0f;
        spherePoint.MyPointSphere.Mysphere.ScaleValue = 1.0f;

        spherePoint.Point.SetActive(false);
        gameObject.SetActive(false);
        isReset = false;
    }
}
