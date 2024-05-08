using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

namespace Scenes.StartMenu.Manager
{
    public class StartMenuManager : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _networkRunnerPrefab;

        //���r�[�ւ̎Q��
        public async void OnButton0()
        {
            //NetworkRunner�̋N��
            var networkRunner = Instantiate(_networkRunnerPrefab);
            networkRunner.ProvideInput = true;
            DontDestroyOnLoad(networkRunner);

            //���r�[�ւ̎Q��
            var result = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

            if (result.Ok) Debug.Log("JoinLobby");
            else Debug.LogError($"error : {result.ShutdownReason}");

            //���r�[�V�[���֑J��
            SceneManager.LoadScene((int)Constant.SceneName.LobbyScene);
        }
    }
}
