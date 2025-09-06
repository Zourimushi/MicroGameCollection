using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class moveBirdScript : MonoBehaviour
{
    public LogicScript logic;
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
        movedirection = Vector2.zero; // 默认不动

        if (SerialManager.Instance != null)
        {
            byte cmd = SerialManager.Instance.lastCommand;

            // 打印接收到的串口数据
           // Debug.Log("Received Serial Command: 0x" + cmd.ToString("X2"));
            switch (cmd)
            {
                case 0x01: movedirection = Vector2.up; break;
                case 0x03: movedirection = Vector2.down; break;
                case 0x04: movedirection = Vector2.left; break;
                case 0x02: movedirection = Vector2.right; break;
                case 0x08: movedirection = new Vector2(-1, 1).normalized; break; // 左上
                case 0x05: movedirection = new Vector2(1, 1).normalized; break;  // 右上
                case 0x07: movedirection = new Vector2(-1, -1).normalized; break;// 左下
                case 0x06: movedirection = new Vector2(1, -1).normalized; break; // 右下
                case 0x00: movedirection = Vector2.zero; break; // 不动
                default: break;
            }
        }
       // movedirection = bird_control.ReadValue<Vector2>();

    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movedirection.x * speed, movedirection.y * speed);
    }

   
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Game!");

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
            logic.gameOver();
            SerialManager.Instance?.SendBytes(new byte[] { 0xFF, 0xFF });
        }
        else if (other.CompareTag("Reward"))
        {
            Debug.Log("Got Reward!");
            Destroy(other.gameObject);
            logic.addScore(1);

            int score = logic.playerScore;
            byte scoreByte = (byte)(score & 0xFF);

            byte[] packet = new byte[] { 0x01, scoreByte };

            SerialManager.Instance?.SendBytes(packet);
        }
    }

    

}
