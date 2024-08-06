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
            //インスタンス化
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            if (Runner == null) Runner = GetComponent<NetworkRunner>();
            else Destroy(this.gameObject);
        }

        /// <summary>
        /// セッションへの参加
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="args"></param> //StartGameArgs設定
        /// <returns></returns>
        public async Task<bool> JoinSession(NetworkRunner runner, StartGameArgs args)
        {
            var result = await runner.StartGame(args);

            if (result.Ok)
            {
                if (runner.IsServer)
                {
                    Debug.Log("Session Role : Host");
                    SessionInfoCache.Instance = new SessionInfoCache(); //セッションデータを管理するためのインスタンス
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
        /// プレイヤーの参加
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                //プレイヤーのトークンと一致しているNetworkObjectを調べる
                int token = new Guid(runner.GetPlayerConnectionToken(player)).GetHashCode();
                var playerInfoList = FindObjectsOfType<PlayerInfo>();
                var resumePlayer = playerInfoList.FirstOrDefault(player => player.connectionToken == token);

                if (resumePlayer != null) //一致オブジェクトあり
                {
                    //プレイヤーとの紐づけ
                    resumePlayer.Object.AssignInputAuthority(player);
                    var playerObj = resumePlayer.gameObject.GetComponent<NetworkObject>();

                    //ホストのトークンを"HOST"に変更
                    if (playerObj.InputAuthority.PlayerId == runner.LocalPlayer.PlayerId)
                    {
                        playerObj.GetComponent<NetworkObjectTrackingData>().token = "HOST";
                    }

                    //プレイヤーオブジェクトの登録
                    SessionInfoCache.Instance.playerList.Add(player, playerObj);
                }
                else //一致オブジェクトなし
                {
                    //あたらしいプレイヤーオブジェクトをスポーン
                    var playerObj = SessionInfoCache.Instance.sceneManager.SpawnPlayer(runner, player);
                    SessionInfoCache.Instance.playerList.Add(player, playerObj);
                }
            }
        }

        /// <summary>
        /// プレイヤーの退出
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                if (SessionInfoCache.Instance.playerList.TryGetValue(player, out NetworkObject networkObj))
                {
                    runner.Despawn(networkObj);　//プレイヤーオブジェクトのデスポーン
                    SessionInfoCache.Instance.playerList.Remove(player); //プレイヤーの削除
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
        /// あるセッション情報が更新された時、呼び出される
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="sessionList"></param>
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            //セッションリストの更新
            updatedSessionList = new List<SessionInfo>(sessionList);
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        /// <summary>
        /// ホストが退出した後に呼び出される
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="hostMigrationToken"></param>
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            //Runnerを削除するため、ホストマイグレーション用のインスタンスを作成
            var hostMigrationHandler = Instantiate(_hostMigrationHandler);
            hostMigrationHandler.RebootRunner(runner, hostMigrationToken);
        }

        /// <summary>
        /// シーンロード時に実行
        /// </summary>
        /// <param name="runner"></param>
        public void OnSceneLoadDone(NetworkRunner runner)
        {
            //ホストかつホストマイグレーション後ではない
            if (runner.IsServer && !runner.IsResume)
            {
                if (_sceneManagerTable.TryGetValue(SceneManager.GetActiveScene().buildIndex, out var sceneManagerPrefab))
                {
                    //シーンマネージャーオブジェクトのスポーン。シーンの管理に必要。
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