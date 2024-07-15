using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Constant;
using Utils;

namespace Scenes.StartMenu.Manager
{
    public class StartMenuManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private Button _lobbyButton;
        [Header("Prefabs")]
        [SerializeField] private NetworkRunner _networkRunnerPrefab;

        void Awake()
        {
            //ゲーム設定
            if (UserInfo.MyInfo == null)
            {
                int userId = Random.Range(0, 100); //ユーザーID
                string userName = Random.Range(0, 100).ToString(); //ユーザー名

                UserInfo.MyInfo = new UserInfo(userId, userName);
            }
        }

        //ロビーへの参加
        public async void OnLobbyButton()
        {
            //ボタンロック
            AllButtonLock();

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
                SceneManager.LoadScene((int)SceneName.LobbyScene);
            }
            else
            {
                Debug.LogError($"error : {result.ShutdownReason}");

                //ロック解除
                AllButtonRelease();
            }
        }

        //全てのボタンをロック
        private void AllButtonLock()
        {
            _lobbyButton.interactable = false;
        }

        //全てのボタンをロック解除
        private void AllButtonRelease()
        {
            _lobbyButton.interactable = true;
        }
    }
}
