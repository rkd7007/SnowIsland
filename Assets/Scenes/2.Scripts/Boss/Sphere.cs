using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/08/22
 * 3D RPG Project
 * 강아름
 * 
 * Sphere Script
 * 보스 공격 패턴 중 광역 공격에 대한 클래스
 */

[System.Serializable]
public class Sphere
{
    [SerializeField]
    private float scaleValue;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 oripos;
    [SerializeField]
    private Vector3 maxSize;
    [SerializeField]
    private GameObject point;

    public MySphereScript MyPointSphere { get; set; }

    public float ScaleValue { get => scaleValue; set => scaleValue = value; }
    public float Speed { get => speed; set => speed = value; }
    public Vector3 Oripos { get => oripos; set => oripos = value; }
    public Vector3 MaxSize { get => maxSize; set => maxSize = value; }
    public GameObject Point { get => point; set => point = value; }
}
