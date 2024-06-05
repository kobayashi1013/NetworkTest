using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Constant;
using Scenes;

namespace Scenes.StartMenu.Manager
{
    public class StartMenuManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private Button _lobbyButton;
        [Header("Prefabs")]
        [SerializeField] private NetworkRunner _networkRunnerPrefab;

        void Awake()
        {
            //�Q�[���ݒ�
            UserInfo.Instance = new UserInfo("user01");
        }

        //���r�[�ւ̎Q��
        public async void OnLobbyButton()
        {
            //�{�^�����b�N
            AllButtonLock();

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
                SceneManager.LoadScene((int)SceneName.LobbyScene);
            }
            else
            {
                Debug.LogError($"error : {result.ShutdownReason}");

                //���b�N����
                AllButtonRelease();
            }
        }

        //�S�Ẵ{�^�������b�N
        private void AllButtonLock()
        {
            _lobbyButton.interactable = false;
        }

        //�S�Ẵ{�^�������b�N����
        private void AllButtonRelease()
        {
            _lobbyButton.interactable = true;
        }
    }
}
