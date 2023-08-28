using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    protected ObjectPool pool;

    public virtual void Create(ObjectPool _pool)
    {
        this.pool = _pool;
        gameObject.SetActive(false);
    }

    public virtual void Push()
    {
        pool.Push(this);
    }
}
