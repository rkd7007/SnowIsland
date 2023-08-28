using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    #region Singleton

    public static ObjectPoolManager instance;
    public ObjectPool Swordpool;
    public ObjectPool Snowpool;
    public ObjectPool SnowPointpool;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #endregion
}
