using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private TMP_Text _sessionPlayerCountTMP;

        private SessionInfo _sessionInfo;

        //初期化
        public void Init(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;

            _sessionNameTMP.text = _sessionInfo.Name;
            _sessionPlayerCountTMP.text = _sessionInfo.PlayerCount.ToString() + "/" + _sessionInfo.MaxPlayers.ToString();
        }

        //セッションに参加
        public async void OnJoinButton()
        {
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (LobbyManager.Instance == null) Debug.LogError("error : Not Found LobbyManager");

            //セッション最新情報取得
            var sessionInfo = NetworkManager.Instance.updatedSessionList.FirstOrDefault(x => x.Name == _sessionInfo.Name);

            //セッション存在確認
            if (sessionInfo == null)
            {
                Debug.Log("error : SessionNotExisted");
                LobbyManager.Instance.NotExistedSession(); //ダイアログ

                return;
            }

            //セッションが満員
            if (sessionInfo.PlayerCount == sessionInfo.MaxPlayers)
            {
                Debug.Log("error : SessionPlayer is Max");
                LobbyManager.Instance.SessionPlayerMax();

                return;
            }

            //ボタンロック
            LobbyManager.Instance.AllButtonLock();

            //セッションに参加
            var result = await NetworkManager.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                Scene = SceneRef.FromIndex((int)SceneName.InGameMulti1),
                SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                SessionName = _sessionInfo.Name
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
    }
}
