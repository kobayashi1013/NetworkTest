using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using Fusion.Sockets;
using System;

public interface ISpawner
{  
    
}
public class StickSpawner : MonoBehaviour,ISpawner, INetworkRunnerCallbacks
{
    [SerializeField]
    NetworkPrefabRef stickPrefab;
    [SerializeField]
    Transform PlayerSpawnPos;

    public void SpawnPlayers(NetworkRunner runner, PlayerRef player)
    {
        
        var SpawnPos = PlayerSpawnPos.position;
        if(runner.IsServer)
        {
            Debug.Log(SpawnPos);
                NetworkObject playerObj = runner.Spawn(stickPrefab, SpawnPos, Quaternion.identity, player);
                SpawnPos.y -= 0.2f;
            
        }
    }

    #region UnusedCallbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    #endregion

}

