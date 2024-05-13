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
        [SerializeField] private GameObject _sessionListObj;
        [SerializeField] private Prefab.SessionData _sessionDataPrefab;
        [SerializeField] private int _sessionViewPadding = 0;

        void Awake()
        {
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
            foreach (Transform child in _sessionListObj.transform)
            {
                Destroy(child.gameObject);
            }

            //セッション描画
            for (int i = 0; i < Network.NetworkManager.Instance.updatedSessionList.Count; i++)
            {
                var obj = Instantiate(_sessionDataPrefab);
                obj.transform.SetParent(_sessionListObj.transform, false);
                obj.transform.localPosition = new Vector3(0, -1 * i * _sessionViewPadding, 0);
                obj.Init(Network.NetworkManager.Instance.updatedSessionList[i].Name);
            }
        }
    }
}
