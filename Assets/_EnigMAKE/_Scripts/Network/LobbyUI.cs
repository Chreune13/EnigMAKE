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
        if (IsServer)
            return;

        if (LocalNetworkManager.Singleton.WaitConnectionToServer() == false && LocalNetworkManager.Singleton.IsConnectedToServer() == false)
            return;

        if (LocalNetworkManager.Singleton.WaitConnectionToServer() == true && LocalNetworkManager.Singleton.IsConnectedToServer() == false)
        {
            GUILayout.Label("En attente de connexion au serveur");
            return;
        }

        if (NetworkManager.Singleton.LocalClient == null)
            return;

        PlayerType playerType = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetworkController>().GetPlayerType();

        if (playerType == PlayerType.GAMEMASTER)
        {
            GUILayout.Label("GameMaster Interface");
            return;
        }

        if (playerType == PlayerType.PLAYER)
        {
            GUILayout.Label("Client en attente du GameMaster");
            return;
        }
    }
}
