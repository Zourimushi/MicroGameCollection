using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choose_script : MonoBehaviour
{
    public Button Button1;
    public Button Button2;
    public Button Button3;
    int GameChoice=0;
    public Vector2[] positions; // 在 Inspector 面板里可以直接填

    // Start is called before the first frame update
    void Start()
    {
        if (positions != null && positions.Length > 0)
        {
            // 把 Image 移动到数组里的第一个位置
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchoredPosition = positions[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SerialManager.Instance != null)
        {
            byte cmd = SerialManager.Instance.lastCommand;
            //Debug.Log("Received Serial Command: 0x" + cmd.ToString("X2"));

            if (cmd == 0x03 )  
            {
                GameChoice = (GameChoice + 1) % 3;
                SerialManager.Instance.lastCommand = 0XFF; // 消费掉，避免重复触发
            }
            else if(cmd==0x01)
            {
                GameChoice--;
                if (GameChoice < 0) GameChoice = GameChoice + 3;
                SerialManager.Instance.lastCommand = 0XFF; // 消费掉，避免重复触发

            }
            else if(cmd==0x11)
            {
                switch(GameChoice)
                {
                    case 0:Button1.onClick.Invoke(); break;
                    case 1: Button2.onClick.Invoke(); break;
                    case 2: Button3.onClick.Invoke(); break;
                    default: break;

                }
                SerialManager.Instance.lastCommand = 0XFF; // 消费掉，避免重复触发

            }
        }
        if (positions != null && positions.Length > 2)
        {
            // 把 Image 移动到数组里的第一个位置
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchoredPosition = positions[GameChoice];
        }

    }
}
