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
            //���r�[���痣�E
            Network.NetworkManager.Runner.Shutdown();

            //�X�^�[�g���j���[�V�[���֑J��
            SceneManager.LoadScene((int)Constant.SceneName.StartMenuScene);
        }
    }
}
