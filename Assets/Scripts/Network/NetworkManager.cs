using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using Prefabs;
using Utils;
using Scenes;

namespace Network
{
    [Serializable]
    public sealed class SceneManagerTable : SerializableDictionary<int, GameObject> { }

    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private HostMigrationHandler _hostMigrationHandler;
        [SerializeField] private SceneManagerTable _sceneManagerTable;

        public static NetworkManager Instance;
        public static NetworkRunner Runner;

        public List<SessionInfo> updatedSessionList = new List<SessionInfo>();

        private void Awake()
        {
            //�C���X�^���X��
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            if (Runner == null) Runner = GetComponent<NetworkRunner>();
            else Destroy(this.gameObject);
        }

        /// <summary>
        /// �Z�b�V�����ւ̎Q��
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="args"></param> //StartGameArgs�ݒ�
        /// <returns></returns>
        public async Task<bool> JoinSession(NetworkRunner runner, StartGameArgs args)
        {
            var result = await runner.StartGame(args);

            if (result.Ok)
            {
                if (runner.IsServer)
                {
                    Debug.Log("Session Role : Host");
                    SessionInfoCache.Instance = new SessionInfoCache(); //�Z�b�V�����f�[�^���Ǘ����邽�߂̃C���X�^���X
                }
                else Debug.Log("Session Role : Client");

                return true;
            }
            else
            {
                Debug.LogError($"Error : {result.ShutdownReason}");
                return false;
            }
        }

        /// <summary>
        /// �v���C���[�̎Q��
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                //�v���C���[�̃g�[�N���ƈ�v���Ă���NetworkObject�𒲂ׂ�
                int token = new Guid(runner.GetPlayerConnectionToken(player)).GetHashCode();
                var playerInfoList = FindObjectsOfType<PlayerInfo>();
                var resumePlayer = playerInfoList.FirstOrDefault(player => player.connectionToken == token);

                if (resumePlayer != null) //��v�I�u�W�F�N�g����
                {
                    //�v���C���[�Ƃ̕R�Â�
                    resumePlayer.Object.AssignInputAuthority(player);
                    var playerObj = resumePlayer.gameObject.GetComponent<NetworkObject>();

                    //�z�X�g�̃g�[�N����"HOST"�ɕύX
                    if (playerObj.InputAuthority.PlayerId == runner.LocalPlayer.PlayerId)
                    {
                        playerObj.GetComponent<NetworkObjectTrackingData>().token = "HOST";
                    }

                    //�v���C���[�I�u�W�F�N�g�̓o�^
                    SessionInfoCache.Instance.playerList.Add(player, playerObj);
                }
                else //��v�I�u�W�F�N�g�Ȃ�
                {
                    //�����炵���v���C���[�I�u�W�F�N�g���X�|�[��
                    var playerObj = SessionInfoCache.Instance.sceneManager.SpawnPlayer(runner, player);
                    SessionInfoCache.Instance.playerList.Add(player, playerObj);
                }
            }
        }

        /// <summary>
        /// �v���C���[�̑ޏo
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                if (SessionInfoCache.Instance.playerList.TryGetValue(player, out NetworkObject networkObj))
                {
                    runner.Despawn(networkObj);�@//�v���C���[�I�u�W�F�N�g�̃f�X�|�[��
                    SessionInfoCache.Instance.playerList.Remove(player); //�v���C���[�̍폜
                }
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            //data.Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            data.Buttons.Set(NetworkInputButtons.LeftArrow, Input.GetKey(KeyCode.LeftArrow));
            data.Buttons.Set(NetworkInputButtons.RightArrow, Input.GetKey(KeyCode.RightArrow));
            data.Buttons.Set(NetworkInputButtons.Space, Input.GetKey(KeyCode.Space));


            input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        /// <summary>
        /// ����Z�b�V������񂪍X�V���ꂽ���A�Ăяo�����
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="sessionList"></param>
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            //�Z�b�V�������X�g�̍X�V
            updatedSessionList = new List<SessionInfo>(sessionList);
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        /// <summary>
        /// �z�X�g���ޏo������ɌĂяo�����
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="hostMigrationToken"></param>
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            //Runner���폜���邽�߁A�z�X�g�}�C�O���[�V�����p�̃C���X�^���X���쐬
            var hostMigrationHandler = Instantiate(_hostMigrationHandler);
            hostMigrationHandler.RebootRunner(runner, hostMigrationToken);
        }

        /// <summary>
        /// �V�[�����[�h���Ɏ��s
        /// </summary>
        /// <param name="runner"></param>
        public void OnSceneLoadDone(NetworkRunner runner)
        {
            //�z�X�g���z�X�g�}�C�O���[�V������ł͂Ȃ�
            if (runner.IsServer && !runner.IsResume)
            {
                if (_sceneManagerTable.TryGetValue(SceneManager.GetActiveScene().buildIndex, out var sceneManagerPrefab))
                {
                    //�V�[���}�l�[�W���[�I�u�W�F�N�g�̃X�|�[���B�V�[���̊Ǘ��ɕK�v�B
                    var sceneManager = runner.Spawn(sceneManagerPrefab, onBeforeSpawned: (_, obj) =>
                    {
                        obj.GetComponent<NetworkObjectTrackingData>().token = Guid.NewGuid().ToString();
                    });

                    SessionInfoCache.Instance.SetSceneManager(sceneManager.GetComponent<ISceneManager>());
                }
            }
        }

        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    }
}