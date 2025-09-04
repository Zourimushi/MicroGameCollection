using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject rewardPrefab;
    public float spawnRate = 1.7f;
    public float minstRate = 0.3f;
    public float SppedUpRate = 0.05f;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;

        }
        else
        {
            //Spawn();
            Spawn();
            if(spawnRate>=minstRate) spawnRate -= SppedUpRate;
            timer = 0;
        }

    }

    void Spawn()
    {
        float randomX = Random.Range(-8f, 8f);
        Vector3 pos = new Vector3(randomX, 6f, 0);

        if (Random.value < 0.7f) // 70% ¸ÅÂÊÕÏ°­Îï
            Instantiate(obstaclePrefab, pos, Quaternion.identity);
        else
            Instantiate(rewardPrefab, pos, Quaternion.identity);
    }
}

