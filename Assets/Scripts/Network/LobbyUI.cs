using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyUI : NetworkBehaviour
{
    private void OnGUI()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            DisplayLobbyInterface();

            GUILayout.EndArea();
        }
    }

    void DisplayLobbyInterface()
    {
        if ((LocalNetworkManager.Singleton?.WaitConnectionToServer() == false && LocalNetworkManager.Singleton?.IsConnectedToServer() == false) || IsServer)
        {
            return;
        }

        if (LocalNetworkManager.Singleton?.WaitConnectionToServer() == true && LocalNetworkManager.Singleton?.IsConnectedToServer() == false)
        {
            GUILayout.Label("En attente de connexion au serveur");
            return;
        }

        if (LocalNetworkManager.Singleton?.GetNetworkDeviceType() == NetworkDeviceType.GAMEMASTER)
        {
            GUILayout.Label("GameMaster Interface");
            return;
        }

        if (LocalNetworkManager.Singleton?.GetNetworkDeviceType() == NetworkDeviceType.CLIENT)
        {
            GUILayout.Label("Client en attente du GameMaster");
            return;
        }
    }
}
