using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어와 부딪혔어요!");
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Ground"))
        {
            Debug.Log("플레이어와 부딪혔어요!");
            Destroy(gameObject);
        }
    }
}
