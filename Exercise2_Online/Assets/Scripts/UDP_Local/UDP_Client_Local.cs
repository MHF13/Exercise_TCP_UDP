using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class UDP_Client_Local : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
    byte[] data;
    int recv;
    EndPoint Remote;
    public string message;

    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6799); // PARA LOCAL, puerto del cliente
        newSocket.Bind(ipep);
        Remote = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6879); // PARA LOCAL IP del servidor, puerto del servidor

        message = "hola";

        data = Encoding.ASCII.GetBytes(message);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Se envia");

            //data = new byte[data.Length];
            data = Encoding.ASCII.GetBytes(message);

            newSocket.SendTo(data, data.Length, SocketFlags.None, Remote);
        }

    }
}
