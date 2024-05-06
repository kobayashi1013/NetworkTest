using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Scenes.Lobby.Manager
{
    public class LobbyManager : MonoBehaviour
    {
        public void OnButton1()
        {
            SceneManager.LoadScene(Constant.SceneName.StartMenuScene);
        }
    }
}
