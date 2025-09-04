using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialManager : MonoBehaviour
{
    public static SerialManager Instance;

    SerialPort serialPort;
    Thread readThread;
    bool isRunning = false;

    public byte lastCommand = 0;  // ���浥Ƭ�����������һ���ֽ�

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
       // UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");

    }

    void ReadSerial()
    {
        while (isRunning && serialPort.IsOpen)
        {
            try
            {
                int bytesToRead = serialPort.BytesToRead;
                if (bytesToRead > 0)
                {
                    byte[] buffer = new byte[bytesToRead];
                    serialPort.Read(buffer, 0, bytesToRead);
                    lastCommand = buffer[0]; // ���赥Ƭ��һ�η�1�ֽ�
                }
            }
            catch { }
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
}
