using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;

public class UdpCommunication : MonoBehaviour
{
    public const int TARGET_FREQUENCY = 10;


    public string ip;
    public int port;


    private IPEndPoint remoteEndPoint;
    private UdpClient client;

    private float timeFromLastSend;

    private bool isMousePressed;
    private float sensorValue;


    void Start()
    {
        if (ip.Length == 0 || port == 0)
        {
            Debug.Log("IP or Port is empty.");
        }

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        client = new UdpClient();
        client.BeginReceive(OnUdpReceive, null);
    }


    void Update()
    {
        isMousePressed = Input.GetMouseButton(0);

        GetComponentInChildren<TextMeshPro>().text = string.Format("{0:0.00}", sensorValue);

        timeFromLastSend += Time.deltaTime;
        if (timeFromLastSend >= 1.0 / TARGET_FREQUENCY)
        {
            byte[] message = EncodeMessageUnityToArduino();
            client.Send(message, message.Length, remoteEndPoint);

            timeFromLastSend = 0;
        }
    }


    private void OnDestroy()
    {
        client.Close();
    }


    private void OnUdpReceive(IAsyncResult asyncResult)
    {
        IPEndPoint ipEndPoint = null;
        byte[] message = client.EndReceive(asyncResult, ref ipEndPoint);

        try
        {
            DecodeMessageArduinoToUnity(message);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        client.BeginReceive(OnUdpReceive, null);
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
