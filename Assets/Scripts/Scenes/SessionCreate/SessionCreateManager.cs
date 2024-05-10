using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;

namespace Scenes.LobbyCreate.Manager
{
    public class SessionCreateManager : MonoBehaviour
    {
        private const int _maxPlayer = 2; //二人ゲーム

        [SerializeField] private TMP_InputField _inputField0;
        [SerializeField] private Button _button0;

        //セッション作成
        public async void OnButton0()
        {
            var result = await Network.NetworkManager.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host,
                Scene = SceneRef.FromIndex((int)Constant.SceneName.InGameScene),
                SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                SessionName = _inputField0.text,
                PlayerCount = _maxPlayer
            });

            if (result.Ok)
            {
                Debug.Log("Host");

                //ボタンロック
                _button0.interactable = false;
            }
            else
            {
                Debug.LogError($"error : {result.ShutdownReason}");

                //ロック解除
                _button0.interactable = true;
            }
        }

        //Back
        public void OnButton1()
        {
            SceneManager.LoadScene((int)Constant.SceneName.LobbyScene);
        }
    }
}
