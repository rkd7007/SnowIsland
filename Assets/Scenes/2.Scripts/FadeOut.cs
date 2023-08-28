using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Color color;
    private float fadeSpeed = 0.4f;
    [SerializeField] private Image image;

    void Start()
    {
        color = image.color;
        StartCoroutine(FadeOutStart());
    }

    IEnumerator FadeOutStart()
    {
        while (true)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            image.color = color;
            if (color.a <= 0)
            {
                break;
            }
            yield return null;
        }
    }
}
