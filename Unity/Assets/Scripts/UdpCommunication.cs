using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        /*
        byte[] data = new byte[1];
        client.Send(data, data.Length, remoteEndPoint);
        */
    }


    private void OnDestroy()
    {
        client.Close();
    }


    private void OnUdpReceive(IAsyncResult asyncResult)
    {
        IPEndPoint ipEndPoint = null;
        byte[] receivedBytes = client.EndReceive(asyncResult, ref ipEndPoint);
        string receivedString = Encoding.ASCII.GetString(receivedBytes);

        value = float.Parse(receivedString);

        Debug.Log(value);

        client.BeginReceive(OnUdpReceive, null);
    }
}
