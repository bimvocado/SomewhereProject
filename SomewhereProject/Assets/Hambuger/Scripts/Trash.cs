using UnityEngine;

public class Trash : MonoBehaviour
{
    private Heart Heart;

    private void Start()
    {
        Heart = FindFirstObjectByType<Heart>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Heart.turnOffHeart();
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
