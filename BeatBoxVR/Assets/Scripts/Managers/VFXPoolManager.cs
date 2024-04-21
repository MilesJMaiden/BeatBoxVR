using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPoolManager : MonoBehaviour
{
    public static VFXPoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, float duration = 0f)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Automatically disable and enqueue after duration if duration is greater than 0
        if (duration > 0)
        {
            StartCoroutine(DisableAndEnqueueAfterDuration(objectToSpawn, tag, duration));
        }
        else
        {
            poolDictionary[tag].Enqueue(objectToSpawn);
        }

        return objectToSpawn;
    }

    IEnumerator DisableAndEnqueueAfterDuration(GameObject objectToSpawn, string tag, float duration)
    {
        yield return new WaitForSeconds(duration);
        objectToSpawn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToSpawn);
    }
}
