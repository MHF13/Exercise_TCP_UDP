using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using System.Threading;

public class UDP_Client_Local : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
    byte[] data;
    int recv;
    EndPoint Remote;
    private string userName;

    //Para el online
    public string MyIP;
    public TMP_InputField userNameText;

    [SerializeField]
    private GameObject joinChatPanel;
    [SerializeField]
    private GameObject theChatPanel;


    public TMP_InputField message;

    bool openChat = false;

    Thread CurrentThread;

    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6799); // PARA LOCAL, puerto del cliente
        newSocket.Bind(ipep);
        Remote = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6879); // PARA LOCAL IP del servidor, puerto del servidor

        data = new byte[255];

        CurrentThread = new Thread(Reciver);
        CurrentThread.Start();

        message = theChatPanel.GetComponentInChildren<TMP_InputField>();


    }


    // Update is called once per frame
    void Update()
    {
        if (openChat)
        {
            OpenChat();
            openChat = false;
        }


    }

    public void Button()
    {
        userName = userNameText.text;

        data = new byte[data.Length];
        data = Encoding.ASCII.GetBytes(userNameText.text);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Remote);

    }

    public void SEND()
    {
        data = new byte[data.Length];
        data = Encoding.ASCII.GetBytes(message.text);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Remote);

    }

    private void Reciver()
    {

        data = new byte[data.Length];
        recv = newSocket.ReceiveFrom(data, ref Remote);
        string str = Encoding.ASCII.GetString(data);

        Debug.Log("Invitacion recivida");

        CurrentThread.Abort();


        CurrentThread = new Thread(InChat);
        CurrentThread.Start();


        openChat = true;
        
        Debug.Log(str);
    }

    private void OpenChat()
    {
        joinChatPanel.SetActive(false);
        theChatPanel.SetActive(true);
    }

    private void InChat()
    {
        while (true)
        {

        }
    }



}
