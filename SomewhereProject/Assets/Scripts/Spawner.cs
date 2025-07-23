using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Hamboogie;
    public GameObject Gamza;
    public GameObject Coke;
    public GameObject Trash01;
    public GameObject Trash02;
    
    public bool enableSpawn = true; // 생성 여부를 인스펙터에서 제어할 수 있도록 public으로 유지합니다.
    
    void Start()
    {
        // 음식
        InvokeRepeating("SpawnHamboogie", 2f, 6f);    // 2, 8, 14...
        InvokeRepeating("SpawnGamza",     4f, 6.5f);  // 4, 10.5, 17...
        InvokeRepeating("SpawnCoke",      6f, 7f);    // 6, 13, 20...

        // 쓰레기 (초기)
        InvokeRepeating("SpawnTrash01", 9f, 9f);      // 9, 18, 27...
        InvokeRepeating("SpawnTrash02", 13f, 10f);    // 13, 23, 33...
    }
    
    void SpawnHamboogie()
    {
        if (enableSpawn)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            Instantiate(Hamboogie, new Vector3(randomX, 6.0f, 0f), Quaternion.identity);
        }
    }
    
    void SpawnGamza()
    {
        if (enableSpawn)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            Instantiate(Gamza, new Vector3(randomX, 6.0f, 0f), Quaternion.identity);
        }
    }
    
    void SpawnCoke()
    {
        if (enableSpawn)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            Instantiate(Coke, new Vector3(randomX, 6.0f, 0f), Quaternion.identity);
        }
    }
    
    void SpawnTrash01()
    {
        if (enableSpawn)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            Instantiate(Trash01, new Vector3(randomX, 6.0f, 0f), Quaternion.identity);
        }
    }
    
    void SpawnTrash02()
    {
        if (enableSpawn)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            Instantiate(Trash02, new Vector3(randomX, 6.0f, 0f), Quaternion.identity);
        }
    }
}
