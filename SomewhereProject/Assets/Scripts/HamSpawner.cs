using UnityEngine;

public class HamSpawner : MonoBehaviour
{
    public GameObject Hamboogie; 
    public bool enableSpawn = true; // 생성 여부를 인스펙터에서 제어할 수 있도록 public으로 유지합니다.

    void Start()
    {
        // 3초 후부터 1초마다 SpawnHamboogie 함수를 반복 호출합니다.
        InvokeRepeating("SpawnHamboogie", 3f, 2f);
    }

    void SpawnHamboogie()
    {
        // enableSpawn이 true일 때만 햄버거를 생성합니다.
        if (enableSpawn)
        {
            Debug.Log("햄부기 생성");
            float randomX = Random.Range(-8.0f, 8.0f);
            Instantiate(Hamboogie, new Vector3(randomX, 6.0f, 0f), Quaternion.identity);
        }
    }
}