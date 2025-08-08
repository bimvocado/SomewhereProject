using System;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    private ScoreCount ScoreCount;
    private Heart Heart;

    private void Start()
    {
        ScoreCount = FindFirstObjectByType<ScoreCount>();
        Heart = FindFirstObjectByType<Heart>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreCount.GetScore();
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Ground"))
        {
            Heart.turnOffHeart();
            Destroy(gameObject);
        }
    }
}
