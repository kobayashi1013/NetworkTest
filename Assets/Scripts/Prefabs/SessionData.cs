using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Scenes.Lobby.Manager;
using Network;
using Constant;

namespace Prefabs
{
    public class SessionData : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private TMP_Text _sessionNameTMP;

        private string _sessionName = "";

        //初期化
        public void Init(string name)
        {
            _sessionName = name;
            _sessionNameTMP.text = name;
        }

        //セッションに参加
        public async void OnJoinButton()
        {
            //セッション存在確認
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionName)) //存在する
            {
                //ボタンロック
                LobbyManager.Instance.AllButtonLock();

                //セッションに参加
                var result = await NetworkManager.Runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    Scene = SceneRef.FromIndex((int)SceneName.InGameScene),
                    SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                    SessionName = _sessionName
                });

                if (result.Ok)
                {
                    Debug.Log("Client");
                }
                else
                {
                    Debug.LogError($"error : {result.ShutdownReason}");

                    //ロック解除
                    LobbyManager.Instance.AllButtonRelease();
                }
            }
            else //セッションは存在しない
            {
                Debug.Log("error : SessionNotExisted");

                //表示削除
                if (LobbyManager.Instance == null) Debug.LogError("error : Not Found LobbyManager");
                LobbyManager.Instance.NotExistedSession();
            }
        }
    }
}
