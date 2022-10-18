using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TCP_Server : MonoBehaviour
{
    int recv;
    byte[] data;
    IPEndPoint ipep;
    IPEndPoint clientep;
    Socket newSocket;
    Socket client;

    Thread receive;
    Thread listen;
    // Start is called before the first frame update
    void Start()
    {
        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Any, 9050);
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        receive = new Thread(Receive);
        listen = new Thread(Listen);

        newSocket.Bind(ipep);
        listen.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Receive()
    {
        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);
        client.Send(data, data.Length, SocketFlags.None);

        while (true)
        {
            data = new byte[1024];
            recv = client.Receive(data);

            if (recv == 0)
                break;

            Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
            client.Send(data, recv, SocketFlags.None);
        }

        Debug.Log("Disconnected from " + clientep.Address);

        client.Close();
        newSocket.Close();
    }

    public void Listen()
    {
        newSocket.Listen(10);

        Debug.Log("Waiting for a client...");

        client = newSocket.Accept();
        clientep = (IPEndPoint)client.RemoteEndPoint;

        if (client.Connected)
            receive.Start();
    }
}

