using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Prefabs;
using Network;
using Constant;

namespace Scenes.LobbyCreate.Manager
{
    public class SessionCreateManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TMP_InputField _sessionNameInputField;
        [SerializeField] private Toggle _isVisibleToggle;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _backButton;
        [Header("Prefabs")]
        [SerializeField] private Dialog _dialogPrefab;
        [Header("Parameters")]
        [SerializeField] private int _maxPlayer = 2;
        [SerializeField] private int _sessionNameMin = 1;
        [SerializeField] private int _sessionNameMax = 12;

        //セッション作成
        public async void OnPlayButton()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");

            //セッション名重複判定
            if (NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionNameInputField.text)) //失敗
            {
                Debug.Log("exist name");

                //入力項目初期化
                _sessionNameInputField.text = "";

                //ダイアログ表示
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("existed name");

                return;
            }

            //文字数制限
            if (_sessionNameInputField.text.Length < _sessionNameMin ||
                _sessionNameInputField.text.Length > _sessionNameMax)
            {
                Debug.Log("word count error");

                //入力項目初期化
                _sessionNameInputField.text = "";

                //ダイアログ表示
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("word count error");

                return;
            }

            //ボタンロック
            AllButtonLock();

            //カスタムプロパティ
            var customProps = new Dictionary<string, SessionProperty>();
            customProps["visible"] = !_isVisibleToggle.isOn; //可視・不可視

            //セッション作成
            var result = await NetworkManager.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host, //ゲームでの権限
                Scene = SceneRef.FromIndex((int)SceneName.InGameMulti1), //次のゲームシーンの選択
                SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(), //Fusion用のSceneManagerの指定
                SessionName = _sessionNameInputField.text, //セッション名の決定
                SessionProperties = customProps,
                PlayerCount = _maxPlayer, //最大人数の決定
            });

            if (result.Ok)
            {
                Debug.Log("Host");
            }
            else
            {
                Debug.LogError($"error : {result.ShutdownReason}");

                //ロック解除
                AllButtonRelease();
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
            _playButton.interactable = false;
            _backButton.interactable = false;
        }

        //全てのボタンを解除
        private void AllButtonRelease()
        {
            _playButton.interactable = true;
            _backButton.interactable = true;
        }
    }
}
