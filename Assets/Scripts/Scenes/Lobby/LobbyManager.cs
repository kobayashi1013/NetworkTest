using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Scenes.Lobby.Manager
{
    public class LobbyManager : MonoBehaviour
    {
        //Create Session
        public void OnButton0()
        {
            SceneManager.LoadScene((int)Constant.SceneName.SessionCreateScene);
        }

        //Back
        public void OnButton1()
        {
            //ロビーから離脱
            Network.NetworkManager.Runner.Shutdown();

            //スタートメニューシーンへ遷移
            SceneManager.LoadScene((int)Constant.SceneName.StartMenuScene);
        }
    }
}
