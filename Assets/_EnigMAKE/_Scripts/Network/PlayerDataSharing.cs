using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public struct TransformSync : INetworkSerializable, System.IEquatable<TransformSync>
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out Position);
            reader.ReadValueSafe(out Rotation);
            reader.ReadValueSafe(out Scale);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(Position);
            writer.WriteValueSafe(Rotation);
            writer.WriteValueSafe(Scale);
        }
    }

    public bool Equals(TransformSync other)
    {
        return Position == other.Position && Rotation == other.Rotation && Scale == other.Scale;
    }
}

public struct PlayerDataToSync : INetworkSerializable, System.IEquatable<PlayerDataToSync>
{
    public int PlayerId;

    public TransformSync Body;
    public TransformSync Head;
    public TransformSync LeftHand;
    public TransformSync RightHand;

    public float TargetTriggerLeft;
    public float TargetTriggerRight;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out PlayerId);
            reader.ReadValueSafe(out Body);
            reader.ReadValueSafe(out Head);
            reader.ReadValueSafe(out LeftHand);
            reader.ReadValueSafe(out RightHand);
            reader.ReadValueSafe(out TargetTriggerLeft);
            reader.ReadValueSafe(out TargetTriggerRight);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(PlayerId);
            writer.WriteValueSafe(Body);
            writer.WriteValueSafe(Head);
            writer.WriteValueSafe(LeftHand);
            writer.WriteValueSafe(RightHand);
            writer.WriteValueSafe(TargetTriggerLeft);
            writer.WriteValueSafe(TargetTriggerRight);
        }
    }

    public bool Equals(PlayerDataToSync other)
    {
        return PlayerId == other.PlayerId;
    }
}

public class PlayerDataSharing : NetworkBehaviour
{
    public static PlayerDataSharing Singleton;

    XROriginNetworkSync LocalPlayer;

    [SerializeField]
    NetworkList<PlayerDataToSync> PlayersDataList;

    Dictionary<int, XROriginNetworkSync> RemotePlayersToSyncronize = new Dictionary<int, XROriginNetworkSync>();

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Debug.LogError("Multiple instances of PlayerDataSharing Singleton!");

        PlayersDataList = new NetworkList<PlayerDataToSync>();
    }

    // Update is called once per frame
    void Update()
    {
        SyncroniseLocalPlayerData();

        SyncronizeRemotePlayersData();
    }

    void SyncroniseLocalPlayerData()
    {
        if (LocalPlayer)
        {
            PlayerDataToSync localPlayerData;

            localPlayerData.PlayerId = LocalPlayer.GetPlayerId();

            localPlayerData.Body = new TransformSync();
            localPlayerData.Head = new TransformSync();
            localPlayerData.LeftHand = new TransformSync();
            localPlayerData.RightHand = new TransformSync();
            localPlayerData.TargetTriggerLeft = 0.0f;
            localPlayerData.TargetTriggerRight = 0.0f;

            WriteToTransformSync(ref localPlayerData.Body, LocalPlayer.gameObject);
            WriteToTransformSync(ref localPlayerData.Head, LocalPlayer.HeadOffset);
            WriteToTransformSync(ref localPlayerData.LeftHand, LocalPlayer.LeftHandOffset);
            WriteToTransformSync(ref localPlayerData.RightHand, LocalPlayer.RightHandOffset);

            localPlayerData.TargetTriggerLeft = LocalPlayer.LeftHandAnimatorScript.GetTrigger();
            localPlayerData.TargetTriggerRight = LocalPlayer.RightHandAnimatorScript.GetTrigger();

            UpdateLocalPlayerDataServerRpc(localPlayerData);
        }
    }

    void WriteToTransformSync(ref TransformSync destination, GameObject source)
    {
        destination.Position = source.transform.localPosition;
        destination.Rotation = source.transform.localRotation;
        destination.Scale = source.transform.localScale;
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateLocalPlayerDataServerRpc(PlayerDataToSync playerDataToSync)
    {
        int i;
        for (i = 0; i < PlayersDataList.Count; i++)
        {
            if (PlayersDataList[i].PlayerId == playerDataToSync.PlayerId)
            {
                 break;
            }
        }
        
        PlayersDataList.RemoveAt(i);

        PlayersDataList.Add(playerDataToSync);
    }

    void SyncronizeRemotePlayersData()
    {
        foreach (PlayerDataToSync playerData in PlayersDataList)
        {
            if (LocalPlayer && playerData.PlayerId == LocalPlayer.GetPlayerId())
                continue;

            if(RemotePlayersToSyncronize.ContainsKey(playerData.PlayerId))
            {
                WriteFromTransformSync(RemotePlayersToSyncronize[playerData.PlayerId].gameObject, playerData.Body);
                WriteFromTransformSync(RemotePlayersToSyncronize[playerData.PlayerId].HeadOffset, playerData.Head);
                WriteFromTransformSync(RemotePlayersToSyncronize[playerData.PlayerId].LeftHandOffset, playerData.LeftHand);
                WriteFromTransformSync(RemotePlayersToSyncronize[playerData.PlayerId].RightHandOffset, playerData.RightHand);

                RemotePlayersToSyncronize[playerData.PlayerId].LeftHandAnimatorScript.SetTrigger(playerData.TargetTriggerLeft);
                RemotePlayersToSyncronize[playerData.PlayerId].RightHandAnimatorScript.SetTrigger(playerData.TargetTriggerRight);
            }
        }
    }

    void WriteFromTransformSync(GameObject destination, TransformSync source)
    {
        if (destination == null)
            return;

        destination.transform.localPosition = source.Position;
        destination.transform.localRotation = source.Rotation;
        destination.transform.localScale = source.Scale;
    }

    public void SetLocalPlayer(XROriginNetworkSync p_LocalPlayer)
    {
        LocalPlayer = p_LocalPlayer;

        NewSyncronizedPlayerDataServerRpc(LocalPlayer.GetPlayerId());
    }

    [ServerRpc(RequireOwnership = false)]
    void NewSyncronizedPlayerDataServerRpc(int p_playerId)
    {
        PlayerDataToSync localPlayerData;

        localPlayerData.PlayerId = p_playerId;

        localPlayerData.Body = new TransformSync();
        localPlayerData.Head = new TransformSync();
        localPlayerData.LeftHand = new TransformSync();
        localPlayerData.RightHand = new TransformSync();
        localPlayerData.TargetTriggerLeft = 0.0f;
        localPlayerData.TargetTriggerRight = 0.0f;

        PlayersDataList.Add(localPlayerData);
    }

    public void SyncronizeRemotePlayer(XROriginNetworkSync p_remotePlayer)
    {
        RemotePlayersToSyncronize.Add(p_remotePlayer.GetPlayerId(), p_remotePlayer);
    }

    public void UnsyncronizeLocalPlayer()
    {
        if(LocalPlayer)
        {
            int i;
            for(i = 0; i < PlayersDataList.Count; i++)
            {
                if (PlayersDataList[i].PlayerId == LocalPlayer.GetPlayerId())
                    break;
            }

            if(i != PlayersDataList.Count)
            {
                Debug.Log(i + "  " + PlayersDataList.Count);
                PlayersDataList.RemoveAt(i);
            }
        }
    }

    public void UnsyncronizeRemotePlayer(XROriginNetworkSync p_remotePlayer)
    {
        RemotePlayersToSyncronize.Remove(p_remotePlayer.GetPlayerId());
    }
}
