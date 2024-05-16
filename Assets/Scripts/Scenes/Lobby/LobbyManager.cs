using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;

namespace Scenes.Lobby.Manager
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private GameObject _Canvas;
        [SerializeField] private GameObject _sessionListContent;
        [SerializeField] private Prefab.SessionData _sessionDataPrefab;
        [SerializeField] private Prefab.Dialog _dialogPrefab;

        public static LobbyManager Instance;

        void Awake()
        {
            //インスタンス化
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            //セッション表示更新
            SessionListUpdate();
        }

        //Create Session
        public void OnButton0()
        {
            SceneManager.LoadScene((int)Constant.SceneName.SessionCreateScene);
        }

        //Back
        public void OnButton1()
        {
            //ボタンロック
            var buttonList = FindObjectsOfType<Button>();
            foreach (var button in buttonList)
            {
                button.interactable = false;
            }

            //ロビーから離脱
            if (Network.NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            Network.NetworkManager.Runner.Shutdown();

            //スタートメニューシーンへ遷移
            SceneManager.LoadScene((int)Constant.SceneName.StartMenuScene);
        }

        //セッションビューの更新
        public void OnButton2()
        {
            SessionListUpdate();
        }

        //セッションビューの更新処理
        private void SessionListUpdate()
        {
            //セッション削除
            foreach (Transform child in _sessionListContent.transform)
            {
                Destroy(child.gameObject);
            }

            //セッション描画
            if (Network.NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            for (int i = 0; i < Network.NetworkManager.Instance.updatedSessionList.Count; i++)
            {
                var obj = Instantiate(_sessionDataPrefab, _sessionListContent.transform);
                obj.Init(Network.NetworkManager.Instance.updatedSessionList[i].Name);
            }
        }

        //セッションが存在しない
        public void NotExistedSession()
        {
            //ダイアログ表示
            var obj = Instantiate(_dialogPrefab, _Canvas.transform);
            obj.Init("not found session");

            //セッションビュー更新
            SessionListUpdate();
        }
    }
}
