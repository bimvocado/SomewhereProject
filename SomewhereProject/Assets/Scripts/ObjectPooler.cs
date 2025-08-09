using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning($"Pool with tag {tag} is empty. Creating new object and expanding pool.");

            Pool targetPool = pools.FirstOrDefault(p => p.tag == tag);
            if (targetPool == null || targetPool.prefab == null)
            {
                Debug.LogError($"Prefab for pool tag {tag} not found or is null.");
                return null;
            }

            objectToSpawn = Instantiate(targetPool.prefab);
            objectToSpawn.transform.SetParent(this.transform);

        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist. Destroying object {objectToReturn.name}.");
            Destroy(objectToReturn);
            return;
        }


        objectToReturn.SetActive(false);

        objectToReturn.transform.SetParent(this.transform);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}