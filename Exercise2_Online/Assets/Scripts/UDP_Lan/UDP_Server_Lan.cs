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
    EndPoint Client;

    EndPoint ClientList;

    private string userName;

    bool updateText;
    string allText; // Chat Actual
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
        ipep = new IPEndPoint(IPAddress.Any, 8000); // un puerto para el host, IPAddress.Any
        newSocket.Bind(ipep);

        // inicializar Client
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Client = (EndPoint)(sender);

        Thread thread = new Thread(ReceieveClients);

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
        
        byte[] data = new byte[255];
        data = Encoding.ASCII.GetBytes(newMessage);
        newSocket.SendTo(data, data.Length, SocketFlags.None, Client);

        Debug.Log("Texto Enviado a Cliente" + newMessage);

        OnlineChat.GetComponent<TextMeshProUGUI>().text += newMessage;

        updateText = false;
    }

    public void SendButton()
    {
        newMessage = "";
        newMessage = "\n[" + userName + "]:" + message.text;
        Debug.Log("Server envia mensaje: " + newMessage);

        updateText = true;
    }

    public void UserNameButton()
    {
        userName = userNameText.text;

        joinChatPanel.SetActive(false);
        theChatPanel.SetActive(true);

    }

    private void ReceieveClients()
    {
        while (true)
        {
            string newMessage2 = "";
            byte[] data = new byte[255];
            int recv = newSocket.ReceiveFrom(data, ref Client);

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

                Debug.Log("Recibe el usuario");

                //enviar confirmacion al cliente
                byte[] invitation;
                invitation = Encoding.ASCII.GetBytes("Can Join");
                newSocket.SendTo(invitation, invitation.Length, SocketFlags.None, Client);

            }
            else
            {
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

                //Nuevo mensaje
                Debug.Log("Nuevo mensaje Recibido: " + newMessage2);

                newMessage = newMessage2;
                Debug.Log("Nuevo mensaje Recibido: " + newMessage);

                updateText = true;

            }
        }
    }
}
