using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

namespace Scenes.StartMenu.Manager
{
    public class StartMenuManager : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _networkRunnerPrefab;
        [SerializeField] private Button _button0;

        //ロビーへの参加
        public async void OnButton0()
        {
            //ボタンロック
            _button0.interactable = false;

            //NetworkRunnerの起動
            var networkRunner = Instantiate(_networkRunnerPrefab);
            networkRunner.ProvideInput = true;
            DontDestroyOnLoad(networkRunner);

            //ロビーへの参加
            var result = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

            if (result.Ok)
            {
                Debug.Log("JoinLobby");

                //ロビーシーンへ遷移
                SceneManager.LoadScene((int)Constant.SceneName.LobbyScene);
            }
            else
            {
                Debug.LogError($"error : {result.ShutdownReason}");

                //ロック解除
                _button0.interactable = true;
            }
        }
    }
}
