using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TCP_Client : MonoBehaviour
{
    byte[] data;
    public string input, stringData;
    IPEndPoint ipep;
    Socket server;
    Thread send;
    // Start is called before the first frame update
    void Start()
    {
        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        send = new Thread(Send);

        try
        {
            server.Connect(ipep);
            send.Start();
        } catch (SocketException e)
        {
            Debug.Log("Unable to connect to server.");
            Debug.Log(e.ToString());
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Disconnecting from server...");
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }

    public void Send()
    {
        int recv = server.Receive(data);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Debug.Log(stringData);

        while (true)
        {
            if (input == "exit")
                break;

            server.Send(Encoding.ASCII.GetBytes(input));
            data = new byte[1024];
            recv = server.Receive(data);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
           Debug.Log(stringData);
        }
    }
}
