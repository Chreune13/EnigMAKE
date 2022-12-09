using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public struct TransformSync/* : INetworkSerializable, System.IEquatable<TransformSync>*/
{
    public Vector3 Position;
    public Quaternion Rotation;
    //public Vector3 Scale;

    /*public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out Position);
            reader.ReadValueSafe(out Rotation);
            //reader.ReadValueSafe(out Scale);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(Position);
            writer.WriteValueSafe(Rotation);
            //writer.WriteValueSafe(Scale);
        }
    }

    public bool Equals(TransformSync other)
    {
        return Position == other.Position && Rotation == other.Rotation;// && Scale == other.Scale;
    }*/
}

public struct PlayerDataToSync/* : INetworkSerializable, System.IEquatable<PlayerDataToSync>*/
{
    public int PlayerId;

    /*public TransformSync Body;
    public TransformSync Head;
    public TransformSync LeftHand;
    public TransformSync RightHand;*/

    public Transform Body;
    public Transform Head;
    public Transform LeftHand;
    public Transform RightHand;

    public float TargetTriggerLeft;
    public float TargetTriggerRight;

    /*public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
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
    }*/
}

public class PlayerDataSharing : NetworkBehaviour
{
    public static PlayerDataSharing Singleton;

    XROriginNetworkSync LocalPlayerModel;

    /*[SerializeField]
    NetworkList<PlayerDataToSync> NetworkPlayersData;*/

    Dictionary<int, XROriginNetworkSync> NetworkPlayersModel = new Dictionary<int, XROriginNetworkSync>();

    [SerializeField]
    float TimeBeforeRemoveUnsichronized = 2.0f;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Debug.LogError("Multiple instances of PlayerDataSharing Singleton!");

        //NetworkPlayersData = new NetworkList<PlayerDataToSync>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LocalPlayerModel)
            SyncroniseLocalPlayerData();

        //SyncronizeRemotePlayersData();
    }

    void SyncroniseLocalPlayerData()
    {
        PlayerDataToSync localPlayerData;

        localPlayerData.PlayerId = LocalPlayerModel.GetPlayerId();

        /*localPlayerData.Body = new TransformSync();
        localPlayerData.Head = new TransformSync();
        localPlayerData.LeftHand = new TransformSync();
        localPlayerData.RightHand = new TransformSync();
        localPlayerData.TargetTriggerLeft = 0.0f;
        localPlayerData.TargetTriggerRight = 0.0f;*/

        localPlayerData.Body = LocalPlayerModel.gameObject.transform;
        localPlayerData.Head = LocalPlayerModel.HeadOffset.transform;
        localPlayerData.LeftHand = LocalPlayerModel.LeftHandOffset.transform;
        localPlayerData.RightHand = LocalPlayerModel.RightHandOffset.transform;

        /*WriteToTransformSync(ref localPlayerData.Body, LocalPlayer.gameObject);
        WriteToTransformSync(ref localPlayerData.Head, LocalPlayer.HeadOffset);
        WriteToTransformSync(ref localPlayerData.LeftHand, LocalPlayer.LeftHandOffset);
        WriteToTransformSync(ref localPlayerData.RightHand, LocalPlayer.RightHandOffset);*/

        localPlayerData.TargetTriggerLeft = LocalPlayerModel.LeftHandAnimatorScript.GetTrigger();
        localPlayerData.TargetTriggerRight = LocalPlayerModel.RightHandAnimatorScript.GetTrigger();

        UpdateLocalPlayerDataServerRpc(localPlayerData);
    }

    /*void WriteToTransformSync(ref TransformSync destination, GameObject source)
    {
        destination.Position = source.transform.localPosition;
        destination.Rotation = source.transform.localRotation;
        //destination.Scale = source.transform.localScale;
    }*/

    [ServerRpc(RequireOwnership = false)]
    void UpdateLocalPlayerDataServerRpc(PlayerDataToSync playerDataToSync)
    {
        /*if (!NetworkPlayersModel.ContainsKey(playerDataToSync.PlayerId))
            return;

        int i;
        for (i = 0; i < NetworkPlayersData.Count; i++)
        {
            if (NetworkPlayersData[i].PlayerId == playerDataToSync.PlayerId)
            {
                break;
            }
        }

        if (i == NetworkPlayersData.Count)
            return;

        NetworkPlayersData.RemoveAt(i);

        NetworkPlayersData.Add(playerDataToSync);*/
    }

    /*void SyncronizeRemotePlayersData()
    {
        //Debug.Log(PlayersDataList.Count);
        //Debug.Log(RemotePlayersToSyncronize.Count);

        foreach (PlayerDataToSync playerData in NetworkPlayersData)
        {
            if (LocalPlayer && playerData.PlayerId == LocalPlayer.GetPlayerId())
                continue;

            if (NetworkPlayersModel.ContainsKey(playerData.PlayerId))
            {
                if (NetworkPlayersModel[playerData.PlayerId] && NetworkPlayersModel[playerData.PlayerId].gameObject)
                    WriteFromTransformSync(NetworkPlayersModel[playerData.PlayerId].gameObject, playerData.Body);

                if (NetworkPlayersModel[playerData.PlayerId] && NetworkPlayersModel[playerData.PlayerId].HeadOffset)
                    WriteFromTransformSync(NetworkPlayersModel[playerData.PlayerId].HeadOffset, playerData.Head);

                if (NetworkPlayersModel[playerData.PlayerId] && NetworkPlayersModel[playerData.PlayerId].LeftHandOffset)
                    WriteFromTransformSync(NetworkPlayersModel[playerData.PlayerId].LeftHandOffset, playerData.LeftHand);

                if (NetworkPlayersModel[playerData.PlayerId] && NetworkPlayersModel[playerData.PlayerId].RightHandOffset)
                    WriteFromTransformSync(NetworkPlayersModel[playerData.PlayerId].RightHandOffset, playerData.RightHand);

                if (NetworkPlayersModel[playerData.PlayerId] && NetworkPlayersModel[playerData.PlayerId].LeftHandAnimatorScript)
                    NetworkPlayersModel[playerData.PlayerId].LeftHandAnimatorScript.SetTrigger(playerData.TargetTriggerLeft);

                if (NetworkPlayersModel[playerData.PlayerId] && NetworkPlayersModel[playerData.PlayerId].RightHandAnimatorScript)
                    NetworkPlayersModel[playerData.PlayerId].RightHandAnimatorScript.SetTrigger(playerData.TargetTriggerRight);
            }
        }
    }*/

    /*void WriteFromTransformSync(GameObject destination, TransformSync source)
    {
        if (destination == null)
            return;

        destination.transform.localPosition = source.Position;
        destination.transform.localRotation = source.Rotation;
        //destination.transform.localScale = source.Scale;
    }*/

    public void SetLocalPlayerModel(XROriginNetworkSync p_LocalPlayer)
    {
        LocalPlayerModel = p_LocalPlayer;

        //NewSyncronizedPlayerDataServerRpc(LocalPlayer.GetPlayerId());
    }

    /*[ServerRpc(RequireOwnership = false)]
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

        NetworkPlayersData.Add(localPlayerData);
    }*/

    public void AddNetworkPlayerModel(XROriginNetworkSync p_remotePlayer)
    {
        NetworkPlayersModel.Add(p_remotePlayer.GetPlayerId(), p_remotePlayer);
    }

    public void RemoveNetworkPlayerModel(XROriginNetworkSync p_remotePlayer)
    {
        NetworkPlayersModel.Remove(p_remotePlayer.GetPlayerId());

        //Debug.Log("Unsinchronize player with id " + p_remotePlayer.GetPlayerId());

        /*if(IsServer)
        {
            int i = 0;
            for(i = 0; i < NetworkPlayersData.Count; i++)
            {
                if (NetworkPlayersData[i].PlayerId == p_remotePlayer.GetPlayerId())
                    break;
            }

            if (i == NetworkPlayersData.Count)
                return;

            NetworkPlayersData.RemoveAt(i);
        }*/
    }
}
