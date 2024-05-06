using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Scenes.StartMenu.Manager
{
    public class StartMenuManager : MonoBehaviour
    {
        public void OnButton0()
        {
            SceneManager.LoadScene(Constant.SceneName.LobbyScene);
        }
    }
}
