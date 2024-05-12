using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

namespace Scenes.Lobby
{
    public class SessionData : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sessionNameTMP;
        [SerializeField] private Button _joinButton;

        private string _sessionName = "";

        //初期化
        public void Init(string name)
        {
            _sessionName = name;
            _sessionNameTMP.text = name;
        }

        //セッションに参加
        public async void PushJoinButton()
        {
            //ボタンロック
            _joinButton.interactable = false;

            //セッションに参加
            var result = await Network.NetworkManager.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                Scene = SceneRef.FromIndex((int)Constant.SceneName.InGameScene),
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
                _joinButton.interactable = true;
            }
        }
    }
}
