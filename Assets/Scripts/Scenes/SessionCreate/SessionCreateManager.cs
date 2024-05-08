using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

namespace Scenes.LobbyCreate.Manager
{
    public class SessionCreateManager : MonoBehaviour
    {
        //ÉZÉbÉVÉáÉìçÏê¨
        public async void OnButton0()
        {
            var result = await Network.NetworkManager.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host,
                Scene = SceneRef.FromIndex((int)Constant.SceneName.InGameScene),
                SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                SessionName = "test",
                PlayerCount = 2
            });

            if (result.Ok)
            {
                Debug.Log("Host");
            }
            else Debug.LogError($"error : {result.ShutdownReason}");
        }

        //Back
        public void OnButton1()
        {
            SceneManager.LoadScene((int)Constant.SceneName.LobbyScene);
        }
    }
}
