using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetWorkText : NetworkBehaviour
{
    [SerializeField] private Text textField;
    private NetworkVariable<int> connectPlayerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.IsServer)
        {
            connectPlayerCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
        }
        textField.text = "Íæ¼ÒÊýÁ¿£º" + connectPlayerCount.Value.ToString();
    }
}
