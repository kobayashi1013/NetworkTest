using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    private NetworkRunner _runner;
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        this.gameObject.AddComponent<RunnerSimulatePhysics2D>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        //var scene = SceneRef.FromIndex(5);
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Test",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

    }



    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    [SerializeField]
    NetworkPrefabRef stickPrefab;
    [SerializeField]
    Transform PlayerSpawnPos;
    int PlayerNum = 0;
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            //var SpawnPos = PlayerSpawnPos.position;
            if (PlayerNum == 0)
            {
                var SpawnPos = new Vector3(0, -4f, 0);
                Debug.Log(SpawnPos);
                NetworkObject playerObj = runner.Spawn(stickPrefab, SpawnPos, Quaternion.identity, player);
                PlayerNum++;
            }
            else
            {
                var SpawnPos = new Vector3(0, -4.4f, 0);
                Debug.Log(SpawnPos);
                NetworkObject playerObj = runner.Spawn(stickPrefab, SpawnPos, Quaternion.identity, player);
            }



            //stickSpawner.SpawnPlayers(runner, player);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
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
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}