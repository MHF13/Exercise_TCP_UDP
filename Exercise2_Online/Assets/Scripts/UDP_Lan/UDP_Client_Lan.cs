using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using System.Threading;

public class UDP_Client_Lan : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
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
    bool updateText = false;
    Thread CurrentThread;
    Thread ReciveThread;

    string sendMessage;

    List<string> messages;
    //static string newMessage;

    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Any, 6799); // IPAddress.Any, puerto del cliente
        newSocket.Bind(ipep);

        //Server = new IPEndPoint(IPAddress.Parse(IP_Server), 6879); // IP del servidor, puerto del servidor
        Server = new IPEndPoint(IPAddress.Any, 0);

        messages = new List<string>();

        ReciveThread = new Thread(Receiver);
        ReciveThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (openChat)
        {
            OpenChat();
        }
        if(updateText){
            UpdateText();
        }

    }

    private void UpdateText(){

        Debug.Log("Texto antes\n" + OnlineChat.GetComponent<TextMeshProUGUI>().text);

        Debug.Log("Este deveria ser el nuevo texto");
        Debug.Log(OnlineChat.GetComponent<TextMeshProUGUI>().text + messages[messages.Count - 1]);

        OnlineChat.GetComponent<TextMeshProUGUI>().text += messages[messages.Count - 1];

        Debug.Log("Texto despues\n" + OnlineChat.GetComponent<TextMeshProUGUI>().text);

        updateText = false;
    }

    public void IPUserNameButton()
    {

        Server = new IPEndPoint(IPAddress.Parse(IpServerText.text), 8000);
        newSocket.Connect(Server);

        userName = userNameText.text;

        byte[] data = Encoding.ASCII.GetBytes(userName);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Server);

    }

    public void SendButton()
    {

        if(message.text == ""){
            return;
        }

        sendMessage = "\n[" + userName + "]:" + message.text;

        message.text = "";

        Debug.Log("Enviar Texto: "+ sendMessage);

        byte[] data = Encoding.ASCII.GetBytes(sendMessage);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Server);
    }

    private void Receiver()
    {
        byte[] recieve = new byte[255];
        int rev = newSocket.Receive(recieve);
        Debug.Log("Invitacion recibida");
        openChat = true;

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
            string newMessage2 = "";
            byte[] data = new byte[255];

            int rev = newSocket.ReceiveFrom(data,ref Server);

            string newMessage = Encoding.ASCII.GetString(data);

            for (int i = 0; i < newMessage.Length; i++)
            {
                if (newMessage[i] != 0)
                {
                    newMessage2 += newMessage[i];
                }
                else
                {
                    break;
                }
            }

            /*
            Debug.Log("Mensaje nuevo recibido: " + newMessage2);
            Debug.Log("caracteres recividos: ");
            Debug.Log(newMessage.Length);
            Debug.Log("caracteres nuevos: ");
            Debug.Log(newMessage2.Length);
            */

            messages.Add(newMessage2);
            updateText = true;
        }
    }



}
