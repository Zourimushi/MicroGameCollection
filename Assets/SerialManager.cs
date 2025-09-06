using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialManager : MonoBehaviour
{
    public static SerialManager Instance;

    SerialPort serialPort;
    Thread readThread;
    bool isRunning = false;

    public byte lastCommand = 0XFF;  // ���浥Ƭ�����������һ���ֽ�

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        serialPort = new SerialPort("COM11", 2400, Parity.None, 8, StopBits.One);
        serialPort.ReadTimeout = 100;

        try { serialPort.Open(); } catch { Debug.LogError("�����޷���"); }

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
                    int data = serialPort.ReadByte(); // ֱ�Ӷ�һ���ֽ�
                    lastCommand = (byte)data;        // ���浽 lastCommand
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
                Debug.Log("�ѷ���: " + data);
            }
            catch (System.Exception e)
            {
                Debug.LogError("���ڷ���ʧ��: " + e.Message);
            }
        }
    }

    public byte GetCommand()
    {
        byte cmd = lastCommand;
        lastCommand = 0XFF; // ���
        return cmd;
    }

    public void SendBytes(byte[] data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.Write(data, 0, data.Length);
                Debug.Log("�ѷ���: " + System.BitConverter.ToString(data));
            }
            catch (System.Exception e)
            {
                Debug.LogError("���ڷ���ʧ��: " + e.Message);
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
            Debug.Log("�����ѹر�");
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

        Debug.Log("�������߳���ֹͣ");
    }

}
