using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using System.Threading;

public class UDP_Server_Lan : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
    byte[] data;
    int recv;
    EndPoint Client;

    EndPoint ClientList;

    private string userName;

    bool updateText;
    string allText; // Chat Actual
    string newText; // Chat con el nuevo mensaje
    string newMessage; // Mensage que llega o se escribe

    //Para el online
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
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Any, 6879); // un puerto para el host, IPAddress.Any
        newSocket.Bind(ipep);

        // inicializar data
        data = new byte[255];
        // inicializar Client
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Client = (EndPoint)(sender);

        Thread thread = new Thread(RecieveClients);

        thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateText)
        {
            UpdateText();
        }
    }

    private void UpdateText(){

        Debug.Log("Texto Modificado");
        
        data = new byte[255];
        data = Encoding.ASCII.GetBytes(newMessage);
        newSocket.SendTo(data, data.Length, SocketFlags.None, Client);

        allText = newText;

        OnlineChat.GetComponent<TextMeshProUGUI>().SetText(allText);
        updateText = false;
    }

    public void SEND()
    {

        Debug.Log("Server envia mensage");
        data = new byte[255];

        newMessage = "/n[" + userName + "]:" + message.text;

        allText = OnlineChat.GetComponent<TextMeshProUGUI>().text;

        newText = allText + newMessage;

        updateText = true;

    }

    public void Button()
    {
        userName = userNameText.text;

        joinChatPanel.SetActive(false);
        theChatPanel.SetActive(true);

    }

    private void RecieveClients()
    {
        while (true)
        {
            data = new byte[255];
            recv = newSocket.ReceiveFrom(data, ref Client);

            string str = Encoding.ASCII.GetString(data);

            bool newClient = true;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ':')
                {
                    newClient = false;
                }
            }

            if (newClient)
            {

                ClientList = Client;

                Debug.Log("Recive el usuario");

                //enviar confirmacion al cliente
                byte[] invitation;
                invitation = Encoding.ASCII.GetBytes("Can Join");
                newSocket.SendTo(invitation, invitation.Length, SocketFlags.None, Client);

            }
            else
            {
                //Nuevo mensage
                Debug.Log("Nuevo mensage Recivido");

                newMessage = str;

                newText = allText + newMessage;
                updateText = true;

            }
        }
    }
}
