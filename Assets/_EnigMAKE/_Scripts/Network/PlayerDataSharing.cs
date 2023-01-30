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
    //public Vector3 Scale;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
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

    XROriginNetworkSync LocalPlayerModel;

    Dictionary<int, XROriginNetworkSync> NetworkPlayersModel = new Dictionary<int, XROriginNetworkSync>();

    [SerializeField]
    float TimeBeforeRemoveUnsichronized = 2.0f;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
        {
            Debug.LogError("Multiple instances of PlayerDataSharing Singleton!");

            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LocalPlayerModel)
            SyncroniseLocalPlayerData();
    }

    void SyncroniseLocalPlayerData()
    {
        PlayerDataToSync localPlayerData;

        localPlayerData.PlayerId = LocalPlayerModel.GetPlayerId();

        localPlayerData.Body = new TransformSync();
        localPlayerData.Head = new TransformSync();
        localPlayerData.LeftHand = new TransformSync();
        localPlayerData.RightHand = new TransformSync();
        localPlayerData.TargetTriggerLeft = 0.0f;
        localPlayerData.TargetTriggerRight = 0.0f;

        WriteToTransformSync(ref localPlayerData.Body, LocalPlayerModel.gameObject);
        WriteToTransformSync(ref localPlayerData.Head, LocalPlayerModel.HeadOffset);
        WriteToTransformSync(ref localPlayerData.LeftHand, LocalPlayerModel.LeftHandOffset);
        WriteToTransformSync(ref localPlayerData.RightHand, LocalPlayerModel.RightHandOffset);

        localPlayerData.TargetTriggerLeft = LocalPlayerModel.LeftHandAnimatorScript.GetTrigger();
        localPlayerData.TargetTriggerRight = LocalPlayerModel.RightHandAnimatorScript.GetTrigger();

        UpdateLocalPlayerDataServerRpc(localPlayerData);
    }

    void WriteToTransformSync(ref TransformSync destination, GameObject source)
    {
        destination.Position = source.transform.localPosition;
        destination.Rotation = source.transform.localRotation;
        //destination.Scale = source.transform.localScale;
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateLocalPlayerDataServerRpc(PlayerDataToSync playerDataToSync)
    {
        UpdateLocalToRemotePlayerDataClientRpc(playerDataToSync);

        UpdateLocalToRemotePlayerData(playerDataToSync);
    }

    [ClientRpc]
    void UpdateLocalToRemotePlayerDataClientRpc(PlayerDataToSync playerDataToSync)
    {
        if (!LocalPlayerModel)
            return;

        if (playerDataToSync.PlayerId == LocalPlayerModel.GetPlayerId())
            return;

        UpdateLocalToRemotePlayerData(playerDataToSync);
    }

    void UpdateLocalToRemotePlayerData(PlayerDataToSync playerDataToSync)
    {
        if (!NetworkPlayersModel.ContainsKey(playerDataToSync.PlayerId))
            return;

        if (NetworkPlayersModel[playerDataToSync.PlayerId].LastUpdate > TimeBeforeRemoveUnsichronized)
            return;

        NetworkPlayersModel[playerDataToSync.PlayerId].LastUpdate = 0;

        WriteFromTransformSync(NetworkPlayersModel[playerDataToSync.PlayerId].gameObject, playerDataToSync.Body);

        WriteFromTransformSync(NetworkPlayersModel[playerDataToSync.PlayerId].HeadOffset, playerDataToSync.Head);

        WriteFromTransformSync(NetworkPlayersModel[playerDataToSync.PlayerId].LeftHandOffset, playerDataToSync.LeftHand);

        WriteFromTransformSync(NetworkPlayersModel[playerDataToSync.PlayerId].RightHandOffset, playerDataToSync.RightHand);

        NetworkPlayersModel[playerDataToSync.PlayerId].LeftHandAnimatorScript.SetTrigger(playerDataToSync.TargetTriggerLeft);

        NetworkPlayersModel[playerDataToSync.PlayerId].RightHandAnimatorScript.SetTrigger(playerDataToSync.TargetTriggerRight);
    }

    void WriteFromTransformSync(GameObject destination, TransformSync source)
    {
        if (destination == null)
            return;

        destination.transform.localPosition = source.Position;
        destination.transform.localRotation = source.Rotation;
        //destination.transform.localScale = source.Scale;
    }

    public void SetLocalPlayerModel(XROriginNetworkSync p_LocalPlayer)
    {
        LocalPlayerModel = p_LocalPlayer;
    }

    public void AddNetworkPlayerModel(XROriginNetworkSync p_remotePlayer)
    {
        NetworkPlayersModel.Add(p_remotePlayer.GetPlayerId(), p_remotePlayer);
    }

    public void RemoveNetworkPlayerModel(XROriginNetworkSync p_remotePlayer)
    {
        NetworkPlayersModel.Remove(p_remotePlayer.GetPlayerId());

        //Debug.Log("Unsinchronize player with id " + p_remotePlayer.GetPlayerId());
    }
}
