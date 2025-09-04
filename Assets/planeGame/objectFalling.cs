using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectFalling : MonoBehaviour
{
    public float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < -6f) Destroy(gameObject);
    }
}
