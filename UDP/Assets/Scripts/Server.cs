using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Todo esto hay que añadirlo kekw
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server : MonoBehaviour
{
    int recv;
    byte[] data;
    IPEndPoint ipep;
    IPEndPoint sender;
    Socket newSocket;
    EndPoint remote;
    Thread ReceiveThread;

    // Start is called before the first frame update
    void Start()
    {
        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Any, 9050);

        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        newSocket.Bind(ipep);

        Debug.Log("Waiting for a Client...");

        sender = new IPEndPoint(IPAddress.Any, 0);
        remote = (EndPoint)(sender);

        ReceiveThread = new Thread(Receiver);
        ReceiveThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Receiver()
    {
        recv = newSocket.ReceiveFrom(data, ref remote);

        Debug.Log("Message received from " + remote.ToString() + ":");
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);

        newSocket.SendTo(data, data.Length, SocketFlags.None, remote);

        while(true)
        {
            data = new byte[1024];
            recv = newSocket.ReceiveFrom(data, ref remote);

            Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
            newSocket.SendTo(data, recv, SocketFlags.None, remote);
        }
    }
}
