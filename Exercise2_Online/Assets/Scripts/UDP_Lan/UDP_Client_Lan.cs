using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using System.Threading;

public class UDP_Client_Lan : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
    byte[] data;
    int recv;
    EndPoint Server;
    private string userName;

    //Para el online
    public TMP_InputField userNameText;
    public TMP_InputField IpServerText;

    [SerializeField]
    private GameObject joinChatPanel;
    [SerializeField]
    private GameObject theChatPanel;
    [SerializeField]
    private GameObject OnlineChat;

    public TMP_InputField message;

    bool openChat = false;

    Thread CurrentThread;
    Thread ReciveThread;

    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Any, 6799); // IPAddress.Any, puerto del cliente

        newSocket.Bind(ipep);
        //Server = new IPEndPoint(IPAddress.Parse(IP_Server), 6879); // IP del servidor, puerto del servidor
        Server = new IPEndPoint(IPAddress.Any, 0);

        data = Encoding.ASCII.GetBytes("hola");
    }

    // Update is called once per frame
    void Update()
    {
        if (openChat)
        {
            OpenChat();
        }

    }




    public void Button()
    {

        Server = new IPEndPoint(IPAddress.Parse(IpServerText.text), 6879);
        userName = userNameText.text;

        data = new byte[data.Length];
        data = Encoding.ASCII.GetBytes(userNameText.text);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Server);

    }

    public void SEND()
    {

        Debug.Log("SEND");
        data = new byte[data.Length];

        string nameMessage = "[" + userName + "]:" + message.text;

        data = Encoding.ASCII.GetBytes(nameMessage);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Server);

    }

    private void Reciver()
    {
        data = new byte[255];
        data = new byte[data.Length];
        recv = newSocket.ReceiveFrom(data, ref Server);
        string str = Encoding.ASCII.GetString(data);

        Debug.Log("Invitacion recivida");

        openChat = true;

        Debug.Log(str);

    }

    private void OpenChat()
    {
        joinChatPanel.SetActive(false);
        theChatPanel.SetActive(true);
        ReciveThread.Abort();
        CurrentThread = new Thread(InChat);
        CurrentThread.Start();
        openChat = false;
    }

    private void InChat()
    {
        while (true)
        {
            Debug.Log("InChat");

            data = new byte[255];
            data = new byte[data.Length];
            recv = newSocket.ReceiveFrom(data, ref Server);
            string str = Encoding.ASCII.GetString(data);

            OnlineChat.GetComponent<TextMeshProUGUI>().SetText(str);

            Debug.Log(str);
        }
    }



}
