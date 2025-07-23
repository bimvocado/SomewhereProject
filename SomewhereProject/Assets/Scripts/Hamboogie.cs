using UnityEngine;

public class Hamboogie : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트의 태그가 "Player"일 경우
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어와 부딪혔어요!");
            // 자기 자신(햄버거 오브젝트)을 파괴합니다.
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Ground"))
        {
            Debug.Log("플레이어와 부딪혔어요!");
            // 자기 자신(햄버거 오브젝트)을 파괴합니다.
            Destroy(gameObject);
        }
    }
}
