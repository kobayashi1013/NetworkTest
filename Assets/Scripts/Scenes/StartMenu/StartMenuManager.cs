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
            //�{�^�����b�N
            var buttonList = FindObjectsOfType<Button>();
            foreach (var button in buttonList)
            {
                button.interactable = false;
            }

            //NetworkRunner�̋N��
            var networkRunner = Instantiate(_networkRunnerPrefab);
            networkRunner.ProvideInput = true;
            DontDestroyOnLoad(networkRunner);

            //���r�[�ւ̎Q��
            var result = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

            if (result.Ok)
            {
                Debug.Log("JoinLobby");

                //���r�[�V�[���֑J��
                SceneManager.LoadScene((int)Constant.SceneName.LobbyScene);
            }
            else
            {
                Debug.LogError($"error : {result.ShutdownReason}");

                //���b�N����
                foreach (var button in buttonList)
                {
                    button.interactable = true;
                }
            }
        }
    }
}
