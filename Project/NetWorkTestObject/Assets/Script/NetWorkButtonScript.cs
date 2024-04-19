using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

using Unity.Netcode.Transports.UTP;

public class NetWorkButtonSripts : MonoBehaviour
{

    public InputField address, port;
    public Button hostBtn, clientBtn;
    public NetworkManager netWorkManager;
    public UnityTransport unityTransport;

    // Start is called before the first frame update
    private void Start()
    {
        hostBtn.onClick.AddListener(OnHostClick);
        clientBtn.onClick.AddListener(OnClientClick);
    }
    private void OnHostClick()
    {
        unityTransport.ConnectionData.ServerListenAddress = address.text;
        unityTransport.ConnectionData.Address = address.text;
        unityTransport.ConnectionData.Port = ushort.Parse(port.text);
        netWorkManager.StartHost();
        GameObject.Find("MapGenerator").SendMessage("SpawnMap");
    }

    private void OnClientClick()
    {
        unityTransport.ConnectionData.Address = address.text;
        unityTransport.ConnectionData.Port = ushort.Parse(port.text);
        netWorkManager.StartClient();
        GameObject.Find("MapManager").SendMessage("SpawnMap");
    }

}

