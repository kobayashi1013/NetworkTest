using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Network;
using Constant;
using Prefabs;
using Utils;

namespace Scenes.PrivateConnection.Manager
{
    public class PrivateConnectionManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TMP_InputField _sessionNameInputField;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _backButton;
        [Header("Prefabs")]
        [SerializeField] private Dialog _dialogPrefab;

        //セッション参加
        public async void OnJoinButton()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");

            //セッション最新情報取得
            var sessionInfo = NetworkManager.Instance.updatedSessionList.FirstOrDefault(x => x.Name == _sessionNameInputField.text);

            //セッション存在確認
            if (sessionInfo == null)
            {
                Debug.Log("error : SessionNotExisted");

                //入力項目初期化
                _sessionNameInputField.text = "";

                //ダイアログ表示
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("not found session");

                return;
            }

            //セッションが満員
            if (sessionInfo.PlayerCount == sessionInfo.MaxPlayers)
            {
                Debug.Log("error : SessionPlayer is Max");

                //入力項目初期化
                _sessionNameInputField.text = "";

                //ダイアログ表示
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("session is full");

                return;
            }

            //ボタンロック
            AllButtonLock();

            //セッションに参加
            var args = new StartGameArgs()
            {
                GameMode = GameMode.Client,
                Scene = SceneRef.FromIndex((int)SceneName.InGameScene),
                SceneManager = NetworkManager.Runner.GetComponent<NetworkSceneManagerDefault>(),
                SessionName = _sessionNameInputField.text,
                ConnectionToken = Guid.NewGuid().ToByteArray(),
            };

            var result = await NetworkManager.Instance.JoinSession(NetworkManager.Runner, args);

            //ロック解除
            if (!result) AllButtonRelease();
        }

        //Back
        public void OnBackButton()
        {
            SceneManager.LoadScene((int)SceneName.LobbyScene);
        }

        //全てのボタンをロック
        private void AllButtonLock()
        {
            _joinButton.interactable = false;
            _backButton.interactable = false;
        }

        //全てのボタンをロック解除
        private void AllButtonRelease()
        {
            _joinButton.interactable = true;
            _backButton.interactable = true;
        }
    }
}
