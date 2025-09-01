using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class moveBirdScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    public InputAction bird_control;
    public Vector2 movedirection = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        bird_control.Enable();
    }
    private void OnDisable()
    {
        bird_control.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        movedirection = bird_control.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movedirection.x * speed, movedirection.y * speed);
    }

}
