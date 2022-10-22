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
    int recv;
    byte[] data;
    IPEndPoint ipep;
    IPEndPoint clientep;
    Socket newSocket;
    Socket client;

    Thread receive;
    Thread listen;

    private string userName;

    bool updateText = false;
    string allText; // Chat Actual
    string newMessage; // Mensaje que llega o se escribe

    public TMP_InputField userNameText;

    [SerializeField]
    private GameObject joinChatPanel;
    [SerializeField]
    private GameObject theChatPanel;
    [SerializeField]
    private GameObject OnlineChat;
    public TMP_InputField message;

    // Start is called before the first frame update
    void Start()
    {
        data = new byte[1024];
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

    public void ReceiveClients()
    {
        while (true)
        {
            string newMessage2 = "";
            byte[] data = new byte[1024];
            int recv = client.Receive(data);

            string str = Encoding.ASCII.GetString(data);

            for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] != 0)
                    {
                        newMessage2 += str[i];
                    }
                    else
                    {
                        break;
                    }
                }

            bool newClient = true;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\n')
                {
                    newClient = false;
                    break;
                }
            }

            if (newClient)
            {
                Debug.Log("Recibe el usuario");

                newMessage = "\n>> " + newMessage2 + " joined the chat";

                updateText = true;
            }
            else
            {
                newMessage = newMessage2;
                updateText = true;
            }
        }
    }

    public void Listen()
    {
        newSocket.Listen(10);

        Debug.Log("Waiting for a client...");

        client = newSocket.Accept();
        clientep = (IPEndPoint)client.RemoteEndPoint;

        receive.Start();
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

    private void UpdateText()
    {
        Debug.Log("Texto Modificado");
        
        byte[] data = new byte[255];
        data = Encoding.ASCII.GetBytes(newMessage);
        client.Send(data, data.Length, SocketFlags.None);

        Debug.Log("Texto Enviado a Cliente" + newMessage);

        OnlineChat.GetComponent<TextMeshProUGUI>().text += newMessage;

        updateText = false;
    }

    public void SendButton()
    {
        if(message.text == ""){
            return;
        }
        newMessage = "";
        newMessage = "\n[" + userName + "]:" + message.text;
        Debug.Log("Server envia mensaje: " + newMessage);

        message.text = "";

        updateText = true;
    }
}

