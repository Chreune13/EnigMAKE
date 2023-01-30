using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Windows;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using ParrelSync;
#endif

public struct PlayerNetworkData
{
    public bool PlayerNetworkDataIsSet;
    public ulong PlayerNetworkId;
    public PlayerType PlayerNetworkType;
}

public class NetworkGameManager : NetworkBehaviour
{
    [NonSerialized]
    public static NetworkGameManager Singleton;

    [NonSerialized]
    public static int MAX_PLAYER = 4;

    PlayerNetworkData LastConnectedPlayer;

    PlayerNetworkData GameMasterNetworkData;

    PlayerNetworkData[] PlayersNetworkData = new PlayerNetworkData[MAX_PLAYER];

    [SerializeField]
    private bool StartAsAServer = false;

    int fc = 0;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;

            LastConnectedPlayer.PlayerNetworkDataIsSet = false;
            GameMasterNetworkData.PlayerNetworkDataIsSet = false;

            for (int i = 0; i < MAX_PLAYER; i++)
                PlayersNetworkData[i].PlayerNetworkDataIsSet = false;
        }
        else
        {
            Debug.LogError("Multiple instances of Singleton NetworkGameManager !");
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        if(ClonesManager.IsClone())
        {
            string arg = ClonesManager.GetArgument();

            if(arg == "server")
            {
                StartAsAServer = true;
            }
        }
#endif

        if (StartAsAServer)
        {
            StartServer();

            StartCoroutine(ServerIsAlive());
        }
        else
        {
            StartClient();
        }

        fc = Time.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessLastConnectedPlayer();

        if(IsClient)
        {
            if((Time.frameCount - fc) * Time.deltaTime > 2.0f)
            {
                BackToWaitingRoom();
            }
        }
    }

    private void ProcessLastConnectedPlayer()
    {
        if (!IsServer)
            return;

        if (!LastConnectedPlayer.PlayerNetworkDataIsSet)
            return;

        if (LastConnectedPlayer.PlayerNetworkType == PlayerType.GAMEMASTER)
        {
            if (GameMasterNetworkData.PlayerNetworkDataIsSet)
            {
                Debug.LogWarning("A GameMaster is already connected");

                DisconnectLastConnectedPlayer();
            }
            else
            {
                GameMasterNetworkData.PlayerNetworkDataIsSet = true;
                GameMasterNetworkData.PlayerNetworkId = LastConnectedPlayer.PlayerNetworkId;
                GameMasterNetworkData.PlayerNetworkType = LastConnectedPlayer.PlayerNetworkType;

                LastConnectedPlayer.PlayerNetworkDataIsSet = false;
            }
        }
        else if (LastConnectedPlayer.PlayerNetworkType == PlayerType.PLAYER)
        {
            int i;

            for (i = 0; i < MAX_PLAYER; i++)
            {
                if(!PlayersNetworkData[i].PlayerNetworkDataIsSet)
                {
                    PlayersNetworkData[i].PlayerNetworkDataIsSet = true;
                    PlayersNetworkData[i].PlayerNetworkId = LastConnectedPlayer.PlayerNetworkId;
                    PlayersNetworkData[i].PlayerNetworkType = LastConnectedPlayer.PlayerNetworkType;

                    LastConnectedPlayer.PlayerNetworkDataIsSet = false;

                    break;
                }
            }

            if(i == MAX_PLAYER)
            {
                Debug.LogWarning("Cannot connect a new Player, limit reach");

                DisconnectLastConnectedPlayer();
            }
        }

        LastConnectedPlayer.PlayerNetworkDataIsSet = false;
    }

    public void NewPlayerConnect(ulong playerId)
    {
        NewPlayerConnectServerRpc(playerId, PlayerState.Singleton.playerState);
    }

    private void DisconnectLastConnectedPlayer()
    {
        if (!IsServer)
            return;

        LastConnectedPlayer.PlayerNetworkDataIsSet = false;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { LastConnectedPlayer.PlayerNetworkId }
            }
        };

        DisconnectLastConnectedPlayerClientRpc(clientRpcParams);
    }

    [ClientRpc]
    private void DisconnectLastConnectedPlayerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        BackToWaitingRoom();
    }

    void BackToWaitingRoom()
    {
        Destroy(NetworkManagerSingleton.instance.gameObject);

        SceneManager.LoadScene("EnigMakeWaitingRoom");
    }

    public void DisconnectConnectedPlayer(ulong playerId)
    {
        if (!IsServer)
            return;

        if(GameMasterNetworkData.PlayerNetworkDataIsSet)
        {
            if(GameMasterNetworkData.PlayerNetworkId == playerId)
            {
                GameMasterNetworkData.PlayerNetworkDataIsSet = false;

                Debug.Log("Gamemaster disconnected");

                return;
            }
        }

        for (int i = 0; i < MAX_PLAYER; i++)
        {
            if (!PlayersNetworkData[i].PlayerNetworkDataIsSet)
                continue;

            if (PlayersNetworkData[i].PlayerNetworkId != playerId)
                continue;

            PlayersNetworkData[i].PlayerNetworkDataIsSet = false;

            Debug.Log("Client disconnected");

            break;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void NewPlayerConnectServerRpc(ulong playerId, PlayerType playerType)
    {
        LastConnectedPlayer.PlayerNetworkDataIsSet = true;
        LastConnectedPlayer.PlayerNetworkId = playerId;
        LastConnectedPlayer.PlayerNetworkType = playerType;
    }

    public bool IsOnNetwork()
    {
        return NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
    }

    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    IEnumerator ServerIsAlive()
    {
        while(true)
        {
            ServerIsAliveClientRpc();

            yield return new WaitForSeconds(1.0f);
        }
    }

    [ClientRpc]
    void ServerIsAliveClientRpc()
    {
        fc = Time.frameCount;
    }
}
