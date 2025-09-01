using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialReader : MonoBehaviour
{
    SerialPort serialPort;
    Thread readThread;
    bool isRunning = false;

    // 用来保存最新收到的字节
    byte[] latestBytes;

    void Start()
    {
        serialPort = new SerialPort("COM11", 2400, Parity.None, 8, StopBits.One);
        serialPort.ReadTimeout = 100;
        serialPort.Open();

        isRunning = true;
        readThread = new Thread(ReadSerial);
        readThread.Start();
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
                    latestBytes = buffer; // 保存数据
                }
            }
            catch { }
        }
    }

    void Update()
    {
        if (latestBytes != null && latestBytes.Length > 0)
        {
            // 打印为十六进制
            string hex = System.BitConverter.ToString(latestBytes).Replace("-", " ");
            Debug.Log("接收到数据: " + hex);

            // 打印为字符串（如果你发的是 ASCII）
            string text = System.Text.Encoding.ASCII.GetString(latestBytes);
            Debug.Log("尝试解码成字符串: " + text);

            latestBytes = null; // 清空，避免重复打印
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (readThread != null && readThread.IsAlive)
            readThread.Join();
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}
