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
    bool updateText = false;
    Thread CurrentThread;
    Thread ReciveThread;

    string allText;
    string sendMessage;

    List<string> messages;

    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Any, 6799); // IPAddress.Any, puerto del cliente

        byte[] data = new byte[255];
        allText = new string("");
        
        newSocket.Bind(ipep);
        //Server = new IPEndPoint(IPAddress.Parse(IP_Server), 6879); // IP del servidor, puerto del servidor
        Server = new IPEndPoint(IPAddress.Any, 0);

        data = Encoding.ASCII.GetBytes("hola");

        messages = new List<string>();

        ReciveThread = new Thread(Reciver);

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

        string newText = allText + messages[messages.Count-1];

        OnlineChat.GetComponent<TextMeshProUGUI>().SetText(newText);

        allText = newText;

        Debug.Log("Update del texto\n" + newText);

        Debug.Log("Update del texto\n" + allText);

        updateText = false;
    }

    public void Button()
    {

        Server = new IPEndPoint(IPAddress.Parse(IpServerText.text), 6879);
        userName = userNameText.text;

        byte[] data = new byte[255];
        data = Encoding.ASCII.GetBytes(userNameText.text);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Server);

    }

    public void SEND()
    {

        byte[] data = new byte[255];
        sendMessage = "\n[" + userName + "]:" + message.text;
        
        Debug.Log("Enviar Texto: "+ sendMessage);

        data = Encoding.ASCII.GetBytes(sendMessage);

        newSocket.SendTo(data, data.Length, SocketFlags.None, Server);

    }

    private void Reciver()
    {

        byte[] data = new byte[255];
        recv = newSocket.ReceiveFrom(data, ref Server);
        string str = Encoding.ASCII.GetString(data);

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
            Debug.Log("Thread esperando recivir mensage");

            byte[] data = new byte[255];
            recv = newSocket.ReceiveFrom(data, ref Server);

            string newMessage = Encoding.ASCII.GetString(data);
            Debug.Log("mensage nuevo recibido: " + newMessage);
            
            //TODO: Contar los caracteres de los mensages recibidos
            //Si Hay 1 mas, borrar el ultimo caracter 


            messages.Add(newMessage);

            updateText = true;
        }
    }



}
