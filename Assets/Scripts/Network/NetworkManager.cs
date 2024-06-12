using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using Utils;
using Constant;

namespace Network
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [Header("SceneManagers")]
        [SerializeField] private GameObject _inGameMulti1Manager;
        [SerializeField] private GameObject _inGameMulti2Manager;
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;

        public static NetworkRunner Runner;
        public static NetworkManager Instance;

        public List<SessionInfo> updatedSessionList = new List<SessionInfo>(); //�Z�b�V�������X�g(off-line)
        public Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>(); //�Z�b�V�������v���C���[

        void Awake()
        {
            //�C���X�^���X��
            if (Runner == null) Runner = this.gameObject.GetComponent<NetworkRunner>();
            else Destroy(this.gameObject);

            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            //�z�X�g����
            if (runner.IsServer)
            {
                //�v���C���[����
                NetworkObject networkObj = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                playerList.Add(player, networkObj);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            //�z�X�g����
            if (runner.IsServer)
            {
                //�v���C���[�폜
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
            //�Z�b�V�����X�V
            updatedSessionList = new List<SessionInfo>(sessionList);
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        //�V�[���}�l�[�W���[�̏���
        public void OnSceneLoadDone(NetworkRunner runner)
        {
            //�z�X�g����
            if (runner.IsServer)
            {
                if (SceneManager.GetActiveScene().buildIndex == (int)SceneName.InGameMulti1)
                {
                    runner.Spawn(_inGameMulti1Manager, Vector3.zero, Quaternion.identity);
                }
                else if (SceneManager.GetActiveScene().buildIndex == (int)SceneName.InGameMulti2)
                {
                    runner.Spawn(_inGameMulti2Manager, Vector3.zero, Quaternion.identity);
                }
            }
        }

        //�Â��V�[���̃l�b�g���[�N�I�u�W�F�N�g���폜
        public void OnSceneLoadStart(NetworkRunner runner)
        {
            //�z�X�g����
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
