using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{
    #region Singleton

    public static Potal instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField] private float fadeSpeed;
    [SerializeField] private Image image;
    private Color color;

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        color = image.color;
        gameObject.SetActive(false);
    }

    public void PotalCreate()
    {
        gameObject.SetActive(true);
        audioSource.PlayOneShot(audioSource.clip);
        StartCoroutine(PotalRange());
    }

    IEnumerator PotalRange()
    {
        float ScaleValue = 0.01f;
        float MaxSize = 0.3f;
        Vector3 pos = transform.localScale;

        while (transform.localScale.x < MaxSize)
        {
            ScaleValue += 0.02f;
            transform.localScale = pos * ScaleValue;

            if (transform.localScale.x >= MaxSize)
                break;

            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        //a = 알파
        while (true)
        {
            color.a += Time.deltaTime * fadeSpeed;
            image.color = color;
            if (color.a > 1)
            {
                //씬 이동
                SceneManager.LoadScene(1);
                break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(FadeIn());
        }
    }
}
