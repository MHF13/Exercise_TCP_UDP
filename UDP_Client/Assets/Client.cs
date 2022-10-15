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
    int recv;
    public string input, stringData;
    IPEndPoint ipep;
    IPEndPoint sender;
    Socket server;
    EndPoint remote;

    public GameObject adressInputText;
    public GameObject userNameText;
    public GameObject serverTalk;

    Thread ReceiveThread;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Stopping client");
            server.Close();
        }
    }

    private void Receiver()
    {
        data = new byte[1024];
        recv = server.ReceiveFrom(data, ref remote);

        SendMessageToServer(input);

        Debug.Log("Message received from " + remote.ToString() + ":");
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
    }

    public void ReadStringText(string s)
    {
        input = s;
        userNameText.SetActive(false);
        serverTalk.SetActive(true);

        ReceiveThread = new Thread(Receiver);
        ReceiveThread.Start();
    }

    public void ReadIPAdress(string IPA)
    {
        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Parse(IPA), 9050);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        string welcome = "Hello, are you there?";
        data = Encoding.ASCII.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        sender = new IPEndPoint(IPAddress.Any, 0);
        remote = (EndPoint)sender;

        adressInputText.SetActive(false);
        userNameText.SetActive(true);
    }

    public void SendMessageToServer(string msg)
    {
        server.SendTo(Encoding.ASCII.GetBytes(msg), remote);
        data = new byte[1024];
        recv = server.ReceiveFrom(data, ref remote);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Debug.Log(stringData);
    }
}
