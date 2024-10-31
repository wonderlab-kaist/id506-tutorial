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
    public string ip;
    public int port;


    private IPEndPoint remoteEndPoint;
    private UdpClient client;

    private float value;


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
        byte[] message = EncodeMessageUnityToArduino();
        client.Send(message, message.Length, remoteEndPoint);

        Debug.Log("Sended");

        GetComponentInChildren<TextMeshPro>().text = string.Format("{0:0.00}", value);
    }


    private void OnDestroy()
    {
        client.Close();
    }


    private void OnUdpReceive(IAsyncResult asyncResult)
    {
        Debug.Log("Received");

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
        return new byte[1];
    }


    private void DecodeMessageArduinoToUnity(byte[] message)
    {
        string decodedString = Encoding.ASCII.GetString(message);

        value = float.Parse(decodedString);
    }
}
