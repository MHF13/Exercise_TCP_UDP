using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class Client : MonoBehaviour
{
    byte[] data;
    public string input, stringData;
    IPEndPoint ipep;
    IPEndPoint sender;
    Socket server;
    EndPoint remote;

    Thread ReceiveThread;
    // Start is called before the first frame update
    void Start()
    {
        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        string welcome = "Hello, are you there?";
        data = Encoding.ASCII.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        sender = new IPEndPoint(IPAddress.Any, 0);
        remote = (EndPoint)sender;

        ReceiveThread = new Thread(Receiver);
        ReceiveThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Receiver()
    {
        data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref remote);

        Debug.Log("Message received from " + remote.ToString() + ":");
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

        while(true)
        {
            if(input == "exit")
                break;
            
        server.SendTo(Encoding.ASCII.GetBytes(input), remote);
        data = new byte[1024];
        recv = server.ReceiveFrom(data, ref remote);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Debug.Log(stringData);
        }

        Debug.Log("Stopping client");
        server.Close();
    }
}
