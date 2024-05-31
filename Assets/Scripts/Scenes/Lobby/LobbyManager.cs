using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Network;
using Prefabs;
using Constant;

namespace Scenes.Lobby.Manager
{
    public class LobbyManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private GameObject _sessionListContent;
        [SerializeField] private TMP_Text _lobbyPlayersTMP;
        [Header("Prefabs")]
        [SerializeField] private SessionData _sessionDataPrefab;
        [SerializeField] private Dialog _dialogPrefab;
        [SerializeField] private ToSoloModeDialog _toSoloModeDialogPrefab;

        public static LobbyManager Instance;
        private int _lobbyPlayers = 0; //ロビーの人数

        void Awake()
        {
            //インスタンス化
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            //セッション表示更新
            SessionViewUpdate();
        }

        //Create Session
        public void OnCreateSessionButton()
        {
            SceneManager.LoadScene((int)SceneName.SessionCreateScene);
        }

        //Back
        public void OnBackButton()
        {
            //全ボタンロック
            AllButtonLock();

            //ロビーから離脱
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            NetworkManager.Runner.Shutdown();

            //スタートメニューシーンへ遷移
            SceneManager.LoadScene((int)SceneName.StartMenuScene);
        }

        //セッションビューの更新
        public void OnUpdateButton()
        {
            SessionViewUpdate();
        }

        public void OnPrivateConnectionButton()
        {
            //プライベート参加シーンへ遷移
            SceneManager.LoadScene((int)SceneName.PrivateConnectionScene);
        }

        //セッションビューの更新処理
        private void SessionViewUpdate()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");

            //ロビーの人数を初期化
            _lobbyPlayers = 0;

            //セッション削除
            foreach (Transform child in _sessionListContent.transform)
            {
                Destroy(child.gameObject);
            }

            //セッション描画
            for (int i = 0; i < NetworkManager.Instance.updatedSessionList.Count; i++)
            {
                //セッション最新情報取得
                var sessionInfo = NetworkManager.Instance.updatedSessionList[i];

                //ロビー人数カウント
                _lobbyPlayers += sessionInfo.PlayerCount;

                //セッション表示
                if (sessionInfo.Properties.TryGetValue("visible", out var property))
                {
                    bool isVisible = (bool)property.PropertyValue;
                    if (isVisible && sessionInfo.PlayerCount != sessionInfo.MaxPlayers) //Publicである
                    {
                        var obj = Instantiate(_sessionDataPrefab, _sessionListContent.transform); //親オブジェクトを設定
                        obj.Init(sessionInfo); //情報の受け渡し
                    }
                }
            }

            //ロビー人数表示
            _lobbyPlayersTMP.text = _lobbyPlayers.ToString();
        }

        //セッションが存在しない
        public void NotExistedSession()
        {
            //ダイアログ表示
            var obj = Instantiate(_dialogPrefab, _canvas.transform);
            obj.Init("not found session");

            //セッションビュー更新
            SessionViewUpdate();
        }

        //セッション人数が上限
        public void SessionPlayerMax()
        {
            var obj = Instantiate(_toSoloModeDialogPrefab, _canvas.transform);
            obj.Init("session is full");

            //セッションビュー更新
            SessionViewUpdate();
        }

        //全てのボタンをロック
        public void AllButtonLock()
        {
            var allButtonList = FindObjectsOfType<Button>();
            foreach (var button in allButtonList)
            {
                button.interactable = false;
            }
        }

        //全てのボタンをロック解除
        public void AllButtonRelease()
        {
            var allButtonList = FindObjectsOfType<Button>();
            foreach (var button in allButtonList)
            {
                button.interactable = true;
            }
        }
    }
}
