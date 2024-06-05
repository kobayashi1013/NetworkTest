using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using Utils;

namespace Network
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private GameObject _playerPrefab;
        public static NetworkRunner Runner;
        public static NetworkManager Instance;

        public List<SessionInfo> updatedSessionList = new List<SessionInfo>(); //セッションリスト(off-line)
        //public Dictionary<PlayerRef, UserInfo> userInfoList = new Dictionary<PlayerRef, UserInfo>(); //ユーザー情報リスト

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
            //自分
            /*if (runner.LocalPlayer == player)
            {
                //ユーザー情報送信
                PostUserInfo(player, UserInfo.MyInfo.username);
            }*/

            //ホスト
            if (runner.IsServer)
            {
                //プレイヤー生成
                NetworkObject networkObj = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                runner.SetPlayerObject(player, networkObj);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
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
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

        //ユーザー情報送信
        /*[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void PostUserInfo(PlayerRef player, string username)
        {
            Debug.Log("Get : PostUserInfo(" + player + ", " + username + ")");

            var userInfo = new UserInfo(username);
            userInfoList.Add(player, userInfo);
        }*/
    }
}
