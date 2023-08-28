using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoint : Poolable
{
    void Start()
    {
        StartCoroutine(Destroy());
    }
    private void Update()
    {
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.5f);
        Push();
    }
}
