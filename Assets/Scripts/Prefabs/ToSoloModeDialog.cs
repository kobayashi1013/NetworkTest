using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using TMPro;
using Prefabs;
using Constant;
using Network;
using Scenes.Lobby.Manager;

namespace Prefabs
{
    public class ToSoloModeDialog : MonoBehaviour
    {
        [Header("Prefab Objects")]
        [SerializeField] private TMP_Text _messageTMP;

        private string _message;

        //初期化
        public void Init(string message)
        {
            _message = message;
            _messageTMP.text = message;
        }

        //ToSoloMode
        public void OnToButton()
        {
            //エラー処理
            if (LobbyManager.Instance == null)
            {
                Debug.LogError("error : Not Found LobbyManager");
                return;
            }

            if (NetworkManager.Runner == null)
            {
                Debug.LogError("error : Not Found Runner");
                return;
            }

            //ボタンロック
            LobbyManager.Instance.AllButtonLock();

            //ロビーから離脱
            NetworkManager.Runner.Shutdown();

            //ソロモードへ遷移
            SceneManager.LoadScene((int)SceneName.InGameSoloScene);
        }
    }
}
