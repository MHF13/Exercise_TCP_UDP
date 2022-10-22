using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using TMPro;

public class TCP_Server_Lan : MonoBehaviour
{

    IPEndPoint ipep;
    IPEndPoint clientep;
    Socket newSocket;
    Socket client;

    Thread receive;
    Thread listen;

    public TMP_InputField userNameText;

    [SerializeField]
    private GameObject joinChatPanel;
    [SerializeField]
    private GameObject theChatPanel;
    [SerializeField]
    private GameObject OnlineChat;

    public TMP_InputField message;

    bool updateText;

    private string userName, newMessage;

    // Start is called before the first frame update
    void Start()
    {
        newMessage = "";
        byte[] data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Any, 6666);
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        receive = new Thread(ReceiveClients);
        listen = new Thread(Listen);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateText)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        Debug.Log("Modified text");
        
        byte[] data = new byte[255];
        data = Encoding.ASCII.GetBytes(newMessage);
        client.Send(data, data.Length, SocketFlags.None);

        OnlineChat.GetComponent<TextMeshProUGUI>().text += newMessage;

        updateText = false;
    }

    public void Listen()
    {
        newSocket.Listen(10);

        Debug.Log("Waiting for a client...");

        client = newSocket.Accept();
        clientep = (IPEndPoint)client.RemoteEndPoint;

        receive.Start();
    }

    public void SendButton()
    {
        if (message.text == "") return;

        newMessage = "\n[" + userName + "]:" + message.text;

        message.text = "";

        updateText = true;
    }

    public void CreateServer()
    {
        newSocket.Bind(ipep);
        listen.Start();

        userName = userNameText.text;

        //Open Chat
        joinChatPanel.SetActive(false);
        theChatPanel.SetActive(true);
    }

    public void ReceiveClients()
    {
        while (true)
        {
            byte[] data = new byte[255]; 
            int recv = client.Receive(data);

            string str = Encoding.ASCII.GetString(data);

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != 0) { newMessage += str[i]; }
                else break;

            }

            bool newClient = true;

            for (int i = 0; i < newMessage.Length; i++)
            {
                if (newMessage[i] == '\n')
                {
                    newClient = false;
                    break;
                }
            }

            if (newClient)
            {
                Debug.Log("Receives a user");

                newMessage = "\n>> " + newMessage + " joined the chat";

            }

            updateText = true;
        }
    }

}

