using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Threading;

public class UDP : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
    byte[] data;
    int recv;
    EndPoint Client;
    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6879); //IPAddress.Any
        newSocket.Bind(ipep);
        // ini cializar data
        data = new byte[10];

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Client = (EndPoint)(sender);

        Thread thread = new Thread(LoopFunction);

        thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        int a = 1;
        a = 2;
        int b = a;
    }

    private void LoopFunction()
    {
        //while (true)
        {
            Debug.Log("Thread");
            recv = newSocket.ReceiveFrom(data, ref Client);

            string str = Encoding.ASCII.GetString(data);

            Debug.Log(str);
        }
    }

    // En un acorrutina
    //recv = newSocket.ReceiveFrom(data, ref Remote);
    //
}
