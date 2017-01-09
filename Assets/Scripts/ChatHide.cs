using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class ChatHide : Photon.MonoBehaviour
{
    public GameObject chatButton;
    public GameObject chatPanel;
    public InputField textMessage;
    public Text textBoxForMessage;
    public InputField nickName;
    public Button nick;

    private List<string> messagesName;
    private List<string> messagesValue;
    private string ListMessages;

    private Animator animator;
    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }

    void Awake()
    {
        animator = chatPanel.GetComponent<Animator>();
        messagesName = new List<string>();
        messagesValue = new List<string>();
    }

    public void StartAnimationChatPanel()
    {
        chatPanel.SetActive(true);
        Animator.Play("Panel");
        chatButton.SetActive(false);
    }

    public void Chat()
    {
        if (textMessage.text != null || textMessage.text.Trim() != "")
        {
            this.photonView.RPC("SendMessage", PhotonTargets.All, textMessage.text);
            textMessage.text = "";
        }
    }

    public void CreateNick()
    {
        photonView.RPC("CreateNickName", PhotonTargets.All);
    }

    [PunRPC]
    public void SendMessage(string message, PhotonMessageInfo mi)
    {
        textBoxForMessage.text += mi.sender.name + ": " + message + "\n";
    }

    [PunRPC]
    public void CreateNickName(PhotonMessageInfo mi)
    {
        mi.sender.name = nickName.text;
        nickName.gameObject.SetActive(false);
        nick.gameObject.SetActive(false);
    }
}
