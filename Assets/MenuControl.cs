using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 用于切换场景
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    void Update()
    {
        if (SerialManager.Instance != null)
        {
            byte cmd = SerialManager.Instance.lastCommand;

            switch (cmd)
            {
                case 0x01: // 单片机发0x10 -> 开始游戏
                    Debug.Log("开始游戏");
                    startButton.onClick.Invoke(); // 模拟点击按钮
                    break;

                case 0x11: // 单片机发0x11 -> 退出
                    Debug.Log("退出游戏");
                    quitButton.onClick.Invoke();
                    break;
            }

            SerialManager.Instance.lastCommand = 0; // 清空，避免重复触发
        }
    }
}
