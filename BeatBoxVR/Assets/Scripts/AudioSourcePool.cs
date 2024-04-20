using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance;

    [SerializeField]
    private GameObject audioSourcePrefab;
    [SerializeField]
    private int poolSize = 10;

    private Queue<AudioSource> pool = new Queue<AudioSource>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = CreateNewSource();
            source.gameObject.SetActive(false);
            pool.Enqueue(source);
        }
    }

    private AudioSource CreateNewSource()
    {
        GameObject obj = Instantiate(audioSourcePrefab, transform);
        AudioSource source = obj.GetComponent<AudioSource>();
        return source;
    }

    public AudioSource GetSource()
    {
        if (pool.Count > 0)
        {
            AudioSource source = pool.Dequeue();
            source.gameObject.SetActive(true);
            return source;
        }
        else
        {
            // Optionally expand the pool here if needed
            return CreateNewSource();
        }
    }

    public void ReturnSource(AudioSource source)
    {
        source.gameObject.SetActive(false);
        pool.Enqueue(source);
    }
}
