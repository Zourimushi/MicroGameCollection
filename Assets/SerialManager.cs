using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialManager : MonoBehaviour
{
    public static SerialManager Instance;

    SerialPort serialPort;
    Thread readThread;
    bool isRunning = false;

    public byte lastCommand = 0XFF;  // 保存单片机发来的最后一个字节

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        serialPort = new SerialPort("COM11", 2400, Parity.None, 8, StopBits.One);
        serialPort.ReadTimeout = 100;

        try { serialPort.Open(); } catch { Debug.LogError("串口无法打开"); }

        isRunning = true;
        readThread = new Thread(ReadSerial);
        readThread.Start();
        UnityEngine.SceneManagement.SceneManager.LoadScene("startScene");

    }

    void ReadSerial()
    {
        while (isRunning && serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead > 0)
                {
                    int data = serialPort.ReadByte(); // 直接读一个字节
                    lastCommand = (byte)data;        // 保存到 lastCommand
                    Debug.Log("Serial Data Received: 0x" + data.ToString("X2"));
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Serial read error: " + ex.Message);
            }
        }
    }

    public void SendByte(byte data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.Write(new byte[] { data }, 0, 1);
                Debug.Log("已发送: " + data);
            }
            catch (System.Exception e)
            {
                Debug.LogError("串口发送失败: " + e.Message);
            }
        }
    }

    public byte GetCommand()
    {
        byte cmd = lastCommand;
        lastCommand = 0XFF; // 清空
        return cmd;
    }

    public void SendBytes(byte[] data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.Write(data, 0, data.Length);
                Debug.Log("已发送: " + System.BitConverter.ToString(data));
            }
            catch (System.Exception e)
            {
                Debug.LogError("串口发送失败: " + e.Message);
            }
        }
    }
    void OnApplicationQuit()
    {
        isRunning = false;
        if (readThread != null && readThread.IsAlive)
            readThread.Join();

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("串口已关闭");
        }
    }

    void OnDisable()
    {
        StopSerial();
    }

    void OnDestroy()
    {
        StopSerial();
    }

    private void StopSerial()
    {
        isRunning = false;
        if (readThread != null && readThread.IsAlive)
            readThread.Join();

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();

        Debug.Log("串口与线程已停止");
    }

}
