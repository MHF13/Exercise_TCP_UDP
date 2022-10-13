using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class TCP : MonoBehaviour
{
    public Socket newSocket;
    public Socket client;

    byte[] data;
    int recv;
    EndPoint endPoint;

    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("10.0.103.30"), 0); 

        newSocket.Bind(ipep);

        newSocket.Listen(10);

        client = newSocket.Accept();

        newSocket.Connect(ipep);


    }

    // Update is called once per frame
    void Update()
    {

        client.Send(data,recv,SocketFlags.None);
        recv = client.Receive(data);
    }
}
