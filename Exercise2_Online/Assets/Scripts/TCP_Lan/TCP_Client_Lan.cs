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

    int recv;
    IPEndPoint ipep;
    Socket server;

    Thread ReceiveThread;
    Thread CurrentThread;

    public TMP_InputField userNameText;
    public TMP_InputField IpServerText;

    //Panels
    [SerializeField]
    private GameObject joinChatPanel;
    [SerializeField]
    private GameObject theChatPanel;
    [SerializeField]
    private GameObject OnlineChat;

    public TMP_InputField messageField;

    bool openChat = false;
    bool updateText = false;

    private string userName, newMessage;

    void Start()
    {
        newMessage = "";
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

    private void UpdateText()
    {
        Debug.Log("Modified text");

        OnlineChat.GetComponent<TextMeshProUGUI>().text += newMessage;

        updateText = false;
    }

    public void EnterServer()
    {
        byte[] data = new byte[1024];
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
        joinChatPanel.SetActive(false);
        theChatPanel.SetActive(true);
        ReceiveThread.Abort();
        CurrentThread = new Thread(InChat);
        CurrentThread.Start();
        openChat = false;
    }

    public void Receiver()
    {
        Debug.Log("Sending Username");
        server.Send(Encoding.ASCII.GetBytes(userName));

        openChat = true;
    }

    public void SendButton()
    {
        if(messageField.text == "") return;

        newMessage = "\n[" + userName + "]:" + messageField.text;

        messageField.text = "";

        Debug.Log("Message has been sent");

        byte[] data = Encoding.ASCII.GetBytes(newMessage);

        server.Send(data, data.Length, SocketFlags.None);
    }

    private void InChat()
    {
        while(true)
        {

            byte[] data = new byte[1024];
            recv = server.Receive(data);

            newMessage = "";
            string reciveMessage = Encoding.ASCII.GetString(data, 0, recv);

            for (int i = 0; i < reciveMessage.Length; i++)
            {
                if (reciveMessage[i] != 0) { newMessage += reciveMessage[i]; }
                else break;
            }
            updateText = true;
        }
    }

}

