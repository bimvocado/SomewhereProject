using UnityEngine;

using System.Collections;
public class Spawner : MonoBehaviour
{
    private Collider spawnArea;
    public GameObject[] fruitPrefabs;

    public GameObject bombPrefab;

    [Range(0f, 1f)]
    public float bombChance = 0.05f;

    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;
    public float minAngle = -16f;
    public float maxAngle = 16f;

    public float minForce = 8f;
    public float maxForce = 15f;

    public float maxLifetime = 5f;

    private Coroutine spawnRoutine;

    private void Awake()
    {  
        spawnArea = GetComponent<Collider>(); 
    }
    //private void StartSpawn()
    //{
    //    StartCoroutine(Spawn());
    //}

    public void StartSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
        spawnRoutine = StartCoroutine(Spawn());
    }
    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f); //2s delay before game start
        while (enabled)
        {
            GameObject prefab = fruitPrefabs[Random.Range(0,fruitPrefabs.Length)];
            
            if (Random.value < bombChance)
            {
                prefab = bombPrefab;
            }
            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            Quaternion rotation = Quaternion.Euler(0f,0f,Random.Range(minAngle,maxAngle));
            GameObject fruit = Instantiate(prefab, position, rotation);
            Destroy(fruit, maxLifetime);

            float force = Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}
