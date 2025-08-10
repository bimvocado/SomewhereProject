using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{

    public GameObject circlePrefab;
    public int numberOfCircles = 50;
    public RectTransform parentCanvas;

    void Start()
    {
        for (int i = 0; i < numberOfCircles; i++)
        {
            GameObject circle = Instantiate(circlePrefab, parentCanvas);
            RectTransform rt = circle.GetComponent<RectTransform>();

            float randomX = Random.Range(-Screen.width / 2f, Screen.width / 2f);
            float randomY = Random.Range(-Screen.height / 2f, Screen.height / 2f);

            rt.anchoredPosition = new Vector2(randomX, randomY);
            circle.AddComponent<CircleMover>();
        }
    }
}