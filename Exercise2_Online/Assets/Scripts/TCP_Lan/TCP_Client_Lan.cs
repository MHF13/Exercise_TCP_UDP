using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using TMPro;
public class TCP_Client_Lan : MonoBehaviour
{
    byte[] data;
    private string input, stringData;
    int recv;
    IPEndPoint ipep;
    Socket server;
    Thread ReceiveThread;
    Thread CurrentThread;
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
    string sendMessage;
    List<string> messages;
    private string userName;

    void Start()
    {
        messages = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if(openChat)
        {
            OpenChat();
        }
        if(updateText){
            UpdateText();
        }
    }

    public void Receiver()
    {
        Debug.Log("Sending Username");
        server.Send(Encoding.ASCII.GetBytes(userName));

        openChat = true;
    }

    public void EnterServer()
    {
        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Parse(IpServerText.text), 6666);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ReceiveThread = new Thread(Receiver);

        try
        {
            userName = userNameText.text;
            server.Connect(ipep);
            ReceiveThread.Start();
        } catch (SocketException e)
        {
            Debug.Log("Unable to connect to server.");
            Debug.Log(e.ToString());
            return;
        }
    }

    private void OpenChat()
    {
        Debug.Log("");
        joinChatPanel.SetActive(false);
        theChatPanel.SetActive(true);
        ReceiveThread.Abort();
        CurrentThread = new Thread(InChat);
        CurrentThread.Start();
        openChat = false;
    }

    private void InChat()
    {
        while(true)
        {
            string newMessage2 = "";
            data = new byte[1024];
            recv = server.Receive(data);
            string newMessage = Encoding.ASCII.GetString(data, 0, recv);
            
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

            messages.Add(newMessage2);
            updateText = true;
        }
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

        server.Send(data, data.Length, SocketFlags.None);
    }

    private void UpdateText()
    {

        Debug.Log("Texto antes\n" + OnlineChat.GetComponent<TextMeshProUGUI>().text);

        Debug.Log("Este deveria ser el nuevo texto");
        Debug.Log(OnlineChat.GetComponent<TextMeshProUGUI>().text + messages[messages.Count - 1]);

        OnlineChat.GetComponent<TextMeshProUGUI>().text += messages[messages.Count - 1];

        Debug.Log("Texto despues\n" + OnlineChat.GetComponent<TextMeshProUGUI>().text);

        updateText = false;
    }
}

