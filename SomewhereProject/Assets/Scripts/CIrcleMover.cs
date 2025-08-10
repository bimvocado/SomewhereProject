using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMover : MonoBehaviour
{
    public float speed = 55f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(1, 1).normalized * speed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y > Screen.height + 100 ||
            rectTransform.anchoredPosition.x > Screen.width + 100)
        {
            float randomX = Random.Range(-Screen.width / 2f, Screen.width / 2f);
            rectTransform.anchoredPosition = new Vector2(randomX, -Screen.height / 2f - 100);
        }
    }
}