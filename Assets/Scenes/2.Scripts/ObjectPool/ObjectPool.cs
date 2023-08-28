using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/07/23
 * 3D RPG Project
 * 강아름
 * 
 * ObjectPool Script
 * Object Pool 기법. Stack을 이용하여 검 Object를 활성화 및 비활성화로 관리
 */

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Poolable poolObj;
    [SerializeField] private int _curCount;

    [SerializeField] private GameObject isarea;
    private BoxCollider _area;

    private Stack<Poolable> poolStack = new Stack<Poolable>();

    void Start()
    {
        CreateStack();
       _area = isarea.GetComponent<BoxCollider>();
    }

    public void CreateStack()
    {
        for (int i = 0; i < _curCount; i++)
        {
            Poolable _curObj = Instantiate(poolObj, this.gameObject.transform);
            _curObj.Create(this);

            poolStack.Push(_curObj);
        }
    }

    public GameObject Pop(Vector3 pos, Quaternion rot)
    {
        Poolable obj = poolStack.Pop();
        if (this.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = pos;
            obj.transform.rotation = rot;
        }
        return obj.gameObject;
    }

    public void Push(Poolable obj)
    {
        if (this.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(false);
            poolStack.Push(obj);
        }
    }

    public void Allpop()
    {
        for (int i = 0; i < _curCount; i++)
        {
            Vector3 _pos = isarea.transform.position;
            float range_X = _area.bounds.size.x;
            float range_Z = _area.bounds.size.z;
            range_X = Random.Range((range_X / 2) * -1, range_X / 2);
            range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
            Vector3 RandomPostion = new Vector3(range_X, 10f, range_Z);
            Vector3 respawnPosition = _pos + RandomPostion;

            Pop(respawnPosition, transform.rotation);

            ObjectPoolManager.instance.SnowPointpool.Pop(new Vector3(respawnPosition.x,
                2.284f, respawnPosition.z), transform.rotation);
        }
    }
}
