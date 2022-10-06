using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class UDP : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
    byte[] data;
    int recv;
    EndPoint Remote;
    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Parse("10.0.103.40"), 0);
        newSocket.Bind(ipep);
    }

    // Update is called once per frame
    void Update()
    {
        recv = newSocket.ReceiveFrom(data, ref Remote);
        newSocket.SendTo(data, recv, SocketFlags.None, Remote);
    }
}
