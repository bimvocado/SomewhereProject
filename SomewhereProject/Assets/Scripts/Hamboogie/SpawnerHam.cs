using UnityEngine;

public class SpawnerHam : MonoBehaviour
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
        InvokeRepeating("SpawnHamboogie", 2f, 12f);    
        InvokeRepeating("SpawnGamza",     6f, 12f);  
        InvokeRepeating("SpawnCoke",      10f, 12f);    

        // 쓰레기 (초기)
        InvokeRepeating("SpawnTrash01", 9f, 13f);      
        InvokeRepeating("SpawnTrash02", 13f, 13f);   
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
