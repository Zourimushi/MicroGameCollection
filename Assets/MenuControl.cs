using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �����л�����
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
                case 0x01: // ��Ƭ����0x10 -> ��ʼ��Ϸ
                    Debug.Log("��ʼ��Ϸ");
                    startButton.onClick.Invoke(); // ģ������ť
                    break;

                case 0x11: // ��Ƭ����0x11 -> �˳�
                    Debug.Log("�˳���Ϸ");
                    quitButton.onClick.Invoke();
                    break;
            }

            SerialManager.Instance.lastCommand = 0; // ��գ������ظ�����
        }
    }
}
