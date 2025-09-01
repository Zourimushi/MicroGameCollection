using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialReader : MonoBehaviour
{
    SerialPort serialPort;
    Thread readThread;
    bool isRunning = false;

    // �������������յ����ֽ�
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
                    latestBytes = buffer; // ��������
                }
            }
            catch { }
        }
    }

    void Update()
    {
        if (latestBytes != null && latestBytes.Length > 0)
        {
            // ��ӡΪʮ������
            string hex = System.BitConverter.ToString(latestBytes).Replace("-", " ");
            Debug.Log("���յ�����: " + hex);

            // ��ӡΪ�ַ���������㷢���� ASCII��
            string text = System.Text.Encoding.ASCII.GetString(latestBytes);
            Debug.Log("���Խ�����ַ���: " + text);

            latestBytes = null; // ��գ������ظ���ӡ
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
