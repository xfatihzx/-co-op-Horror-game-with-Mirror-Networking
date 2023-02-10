using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public struct Notification : NetworkMessage
{
    public string content;
}

public class MessagesTest : CacheBehaviour<MessagesTest>
{
    //[SerializeField] private TMP_Text notificationsText = null;

    private void OnEnable()
    {
        if (!NetworkClient.active) return;

        NetworkClient.RegisterHandler<Notification>(OnNotification);
        NetworkServer.RegisterHandler<Notification>(OnNotification);
    }

    private void OnNotification(Notification msg)
    {
        Debug.Log("Server send this message : " + msg.content);
    }

    private void OnNotification(NetworkConnection conn, Notification msg)
    {
        Debug.Log("Client send this message : " + msg.content);

        Debug.Log("Message send all client...");

        NetworkServer.SendToAll(msg);
    }
}