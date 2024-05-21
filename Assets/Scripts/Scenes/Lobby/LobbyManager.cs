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
        [Header("Prefabs")]
        [SerializeField] private SessionData _sessionDataPrefab;
        [SerializeField] private Dialog _dialogPrefab;

        public static LobbyManager Instance;

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

        //セッションビューの更新処理
        private void SessionViewUpdate()
        {
            //セッション削除
            foreach (Transform child in _sessionListContent.transform)
            {
                Destroy(child.gameObject);
            }

            //セッション描画
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            for (int i = 0; i < NetworkManager.Instance.updatedSessionList.Count; i++)
            {
                var obj = Instantiate(_sessionDataPrefab, _sessionListContent.transform); //親オブジェクトを設定
                obj.Init(NetworkManager.Instance.updatedSessionList[i].Name); //情報の受け渡し
            }
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
