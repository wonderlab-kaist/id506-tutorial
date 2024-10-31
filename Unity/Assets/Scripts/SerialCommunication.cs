using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using TMPro;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
    public const int TARGET_FREQUENCY = 10;


    public string portName;
    public int baudRate;


    private SerialPort serialPort;

    private float timeFromLastSend;

    private bool isMousePressed;
    private float sensorValue;


    void Start()
    {
        if (portName.Length == 0 || baudRate == 0)
        {
            Debug.Log("Port Name or Baud Rate is empty.");
        }

        serialPort = new SerialPort(portName, baudRate);
        serialPort.DtrEnable = true;
        serialPort.RtsEnable = true;
        serialPort.Open();
    }


    void Update()
    {
        isMousePressed = Input.GetMouseButton(0);

        GetComponentInChildren<TextMeshPro>().text = string.Format("{0:0.00}", sensorValue);

        int bytesToRead = serialPort.BytesToRead;
        if (bytesToRead > 0)
        {
            byte[] message = new byte[bytesToRead];
            serialPort.Read(message, 0, bytesToRead);

            DecodeMessageArduinoToUnity(message);
        }

        timeFromLastSend += Time.deltaTime;
        if (timeFromLastSend >= 1.0 / TARGET_FREQUENCY)
        {
            byte[] message = EncodeMessageUnityToArduino();
            serialPort.Write(message, 0, message.Length);

            timeFromLastSend = 0;
        }
    }


    private void OnDestroy()
    {
        serialPort.Close();
    }


    private byte[] EncodeMessageUnityToArduino()
    {
        byte[] message = new byte[1];

        message[0] = isMousePressed ? (byte)1 : (byte)0;

        return message;
    }


    private void DecodeMessageArduinoToUnity(byte[] message)
    {
        string decodedString = Encoding.ASCII.GetString(message);

        sensorValue = float.Parse(decodedString);
    }
}
