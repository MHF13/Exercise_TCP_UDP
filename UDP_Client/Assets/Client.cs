using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;


public class Client : MonoBehaviour
{
    byte[] data;
    int recv;
    public string input, stringData;
    IPEndPoint ipep;
    IPEndPoint sender;
    Socket server;
    EndPoint remote;

    public GameObject adressInputGO;
    public GameObject userNameGO;
    public GameObject IPUButton;
    public GameObject chat;
    public GameObject chatButton;

    public TMP_InputField adressInput;
    public TMP_InputField usernameInput;
    public TMP_InputField chatInput;


    Thread SendThread;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Stopping client");
            server.Close();
        }
    }

    private void Send()
    {
        data = new byte[1024];
        recv = server.ReceiveFrom(data, ref remote);

        SendMessageToServer(input);

        Debug.Log("Message received from " + remote.ToString() + ":");
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
    }

    public void SendUserName(string s)
    {
        string userName = s;
        data = Encoding.ASCII.GetBytes(s);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
    }

    public void SendIPAdress(string IPA)
    {
        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Parse(IPA), 9050);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        sender = new IPEndPoint(IPAddress.Any, 0);
        remote = (EndPoint)sender;
    }

    public void SendMessageToServer(string msg)
    {
        server.SendTo(Encoding.ASCII.GetBytes(msg), remote);
        data = new byte[1024];
        recv = server.ReceiveFrom(data, ref remote);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Debug.Log(stringData);
    }

    public void SendUserAndIP()
    {
        SendIPAdress(adressInput.text);
        SendUserName(usernameInput.text);

        adressInputGO.SetActive(false);
        userNameGO.SetActive(false);
        IPUButton.SetActive(false);

        SendThread = new Thread(Send);
        SendThread.Start();

        chat.SetActive(true);
        chatButton.SetActive(true);
    }

    public void SendMessage()
    {
        SendMessageToServer(chatInput.text);
    }
}
