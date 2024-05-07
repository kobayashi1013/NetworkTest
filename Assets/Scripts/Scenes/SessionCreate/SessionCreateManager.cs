using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

namespace Scenes.LobbyCreate.Manager
{
    public class SessionCreateManager : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _networkRunnerPrefab;

        public async void GameLauncher()
        {
            var networkRunner = Instantiate(_networkRunnerPrefab);
            networkRunner.ProvideInput = true;

            var result = await networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host,
                Scene = SceneRef.FromIndex((int)Constant.SceneName.LobbyScene),
                SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
            });

            if (result.Ok) Debug.Log("Host");
            else Debug.LogError("error : LobbyCreate");
        }

        public void OnButton1()
        {
            SceneManager.LoadScene((int)Constant.SceneName.LobbyScene);
        }
    }
}
