using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using System.Threading;

public class UDP_Server_Local : MonoBehaviour
{
    Socket newSocket;
    IPEndPoint ipep;
    byte[] data;
    int recv;
    EndPoint Client;

    EndPoint ClientList;

    [SerializeField]
    private GameObject Chat;

    bool updateText;
    string newText;

    // Start is called before the first frame update
    void Start()
    {
        newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6879); // PARA LOCAL, IP local
        newSocket.Bind(ipep);
        // ini cializar data
        data = new byte[255];

        updateText = false;

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
            Debug.Log("Modifica Texto");
            string text = Chat.GetComponent<TextMeshProUGUI>().text;
            text += "\n" + newText;

            Debug.Log(text);
            updateText = false;

            Chat.GetComponent<TextMeshProUGUI>().SetText(text);

            byte[] textChat;
            textChat = Encoding.ASCII.GetBytes(text);

            newSocket.SendTo(textChat, textChat.Length, SocketFlags.None, Client);

        }
    }

    private void RecieveClients()
    {
        while (true)
        {
            data = new byte[data.Length];
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
            
            if (newClient){

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
                Debug.Log("Nuevo mensage");
                newText = str + "\n";
                updateText = true;

            }
        }
    }



}
