using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Network;
using Constant;
using Prefabs;

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
            //セッション存在判定
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionNameInputField.text)) //成功
            {
                //ボタンロック
                AllButtonLock();

                //セッションに参加
                var result = await NetworkManager.Runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    Scene = SceneRef.FromIndex((int)SceneName.InGameScene),
                    SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                    SessionName = _sessionNameInputField.text
                });

                if (result.Ok)
                {
                    Debug.Log("Client");
                }
                else
                {
                    Debug.LogError($"error : {result.ShutdownReason}");

                    //ロック解除
                    AllButtonRelease();
                }
            }
            else //セッションは存在しない
            {
                Debug.Log("error : SessionNotExisted");

                //入力項目初期化
                _sessionNameInputField.text = "";

                //ダイアログ表示
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("not found session");
            }
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
