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
            //ボタンロック
            _button0.interactable = false;

            //セッション作成
            var result = await Network.NetworkManager.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host, //ゲームでの権限
                Scene = SceneRef.FromIndex((int)Constant.SceneName.InGameScene), //次のゲームシーンの選択
                SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(), //Fusion用のSceneManagerの指定
                SessionName = _inputField0.text, //セッション名の決定
                PlayerCount = _maxPlayer //最大人数の決定
            });

            if (result.Ok)
            {
                Debug.Log("Host");
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
