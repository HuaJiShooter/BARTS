using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkPanel : MonoBehaviour
{

    public LinkRequest linkRequest;
    public InputField playerID;
    public Button linkBtn;
    private void Start()
    {
        linkBtn.onClick.AddListener(OnLinkClick);
    }

    private void OnLinkClick()
    {
        if (playerID.text == "")
        {
            Debug.LogWarning("PlayerID����Ϊ��");
            return;
        }
        try
        {
            linkRequest.SendRequest(int.Parse(playerID.text));
            return;
        }
        catch(System.Exception e)
        {
            Debug.Log("�����playerID����" + e.Message);
            return;
        }
    }
}
