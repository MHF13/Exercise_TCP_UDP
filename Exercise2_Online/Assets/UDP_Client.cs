using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class UDP_Client : MonoBehaviour
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
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6878); // IPAddress.Any
        newSocket.Bind(ipep);
        Remote = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6879); // IP del servidor, puerto del servidor

        data = Encoding.ASCII.GetBytes("hola");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Se envia");
            newSocket.SendTo(data, data.Length, SocketFlags.None, Remote);
        }

    }
}
