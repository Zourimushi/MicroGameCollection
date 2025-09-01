using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject Pipes;
    public float spawnRate = 2;
    private float timer = 0;
    public float heightOffset = 10;
    // Start is called before the first frame update
    void Start()
    {
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer<spawnRate)
        {
            timer+=Time.deltaTime;
           
        }
        else
        {
            spawnPipe();
            timer = 0;
        }
        
    }
    public void spawnPipe()
    {
        float lowestpoint = transform.position.y - heightOffset;
        float highestpoint = transform.position.y + heightOffset;
        Instantiate(Pipes, new Vector3(transform.position.x,Random.Range(lowestpoint,highestpoint),transform.position.z), transform.rotation);
    }
}
