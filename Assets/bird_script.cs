using UnityEngine;
using UnityEngine.InputSystem;

public class bird_script : MonoBehaviour
{
    public Rigidbody2D myrb;
    public float flapStrength = 5f;
    public LogicScript logic;
    public bool birdIsAlive = true;
    public Input birdControls;

    private void Awake()
    {
        birdControls = new Input();
    }

    private void OnEnable()
    {
        birdControls.Enable();
        birdControls.bird.jump.performed += Onjump;
    }

    private void OnDisable()
    {
        birdControls.Disable();
        birdControls.bird.jump.performed -= Onjump;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        //     Flap();

        // 单片机输入
        if (SerialManager.Instance != null)
        {
            byte cmd = SerialManager.Instance.lastCommand;
            if (cmd == 0x01 && birdIsAlive)  // 假设单片机发0x01表示跳跃
            {
                Flap();
                SerialManager.Instance.lastCommand = 0; // 消费掉，避免重复触发
            }
        }

        // 超出边界死亡
        if (transform.position.y > 14.8 || transform.position.y < -14.8)
            Dead();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Dead();
    }

    private void Onjump(InputAction.CallbackContext context)
    {
        if (birdIsAlive)
            Flap();
    }

    private void Flap()
    {
        myrb.velocity = Vector2.up * flapStrength;
    }

    public void Dead()
    {
        birdIsAlive = false;
        birdControls.bird.jump.performed -= Onjump;
        logic.gameOver();
    }
}
