using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using Utils;
using Constant;
using Prefabs;

namespace Network
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [Header("SceneManagers")]
        [SerializeField] private GameObject _inLobbyMultiManager;
        [SerializeField] private GameObject _inGameMultiManager;
        [Header("Prefabs")]
        [SerializeField] private NetworkManagerHandler _networkManagerHandlerPrefab;
        [SerializeField] private GameObject _playerPrefab;

        public static NetworkRunner Runner;
        public static NetworkManager Instance;

        public List<SessionInfo> updatedSessionList = new List<SessionInfo>(); //セッションリスト(off-line)
        public Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>(); //セッション内プレイヤー

        void Awake()
        {
            //インスタンス化
            if (Runner == null) Runner = this.gameObject.GetComponent<NetworkRunner>();
            else Destroy(this.gameObject);

            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            //ホスト権限
            if (runner.IsServer)
            {
                //ホストマイグレーション時の復元の確認
                int token = new Guid(runner.GetPlayerConnectionToken(player)).GetHashCode();
                var playerInfoList = FindObjectsOfType<PlayerInfo>();
                var newPlayer = playerInfoList.FirstOrDefault(player => player.connectionToken == token);

                if (newPlayer != null)
                {
                    newPlayer.Object.AssignInputAuthority(player);
                    playerList.Add(player, newPlayer.gameObject.GetComponent<NetworkObject>());
                }
                else //新規プレイヤー
                {
                    //新規プレイヤー
                    var playerObj = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player,
                        (_, obj) =>
                        {
                            var playerInfo = obj.GetComponent<PlayerInfo>();
                            playerInfo.connectionToken = token;
                            playerInfo.hostId = runner.LocalPlayer.PlayerId;
                        });
                    playerList.Add(player, playerObj);
                }
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            //ホスト権限
            if (runner.IsServer)
            {
                //プレイヤー削除
                if (playerList.TryGetValue(player, out NetworkObject networkObj))
                {
                    runner.Despawn(networkObj);
                    playerList.Remove(player);
                }
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            //セッション更新
            updatedSessionList = new List<SessionInfo>(sessionList);
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            /*playerList.Clear();
            var networkRunnerHandler = Instantiate(_networkManagerHandlerPrefab);
            networkRunnerHandler.ResetNetworkRunner(runner, hostMigrationToken);*/
        }

        //シーンマネージャーの準備
        public void OnSceneLoadDone(NetworkRunner runner)
        {
            //ホスト権限
            if (runner.IsServer)
            {
                if (SceneManager.GetActiveScene().buildIndex == (int)SceneName.InLobbyMultiScene)
                {
                    runner.Spawn(_inLobbyMultiManager, Vector3.zero, Quaternion.identity);
                }
                else if (SceneManager.GetActiveScene().buildIndex == (int)SceneName.InGameMultiScene)
                {
                    runner.Spawn(_inGameMultiManager, Vector3.zero, Quaternion.identity);
                }
            }
        }

        //古いシーンのネットワークオブジェクトを削除
        public void OnSceneLoadStart(NetworkRunner runner)
        {
            //ホスト権限
            if (runner.IsServer)
            {
                var networkObjects = FindObjectsOfType<NetworkObject>();
                foreach (var networkObject in networkObjects)
                {
                    runner.Despawn(networkObject);
                }
            }
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    }
}
