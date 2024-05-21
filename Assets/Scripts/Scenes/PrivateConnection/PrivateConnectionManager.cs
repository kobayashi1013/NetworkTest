using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Network;
using Constant;
using Prefabs;

namespace Scenes.PrivateConnection.Manager
{
    public class PrivateConnectionManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TMP_InputField _sessionNameInputField;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _backButton;
        [Header("Prefabs")]
        [SerializeField] private Dialog _dialogPrefab;

        //�Z�b�V�����Q��
        public async void OnJoinButton()
        {
            //�Z�b�V�������ݔ���
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionNameInputField.text)) //����
            {
                //�{�^�����b�N
                AllButtonLock();

                //�Z�b�V�����ɎQ��
                var result = await NetworkManager.Runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    Scene = SceneRef.FromIndex((int)SceneName.InGameScene),
                    SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                    SessionName = _sessionNameInputField.text
                });

                if (result.Ok)
                {
                    Debug.Log("Client");
                }
                else
                {
                    Debug.LogError($"error : {result.ShutdownReason}");

                    //���b�N����
                    AllButtonRelease();
                }
            }
            else //�Z�b�V�����͑��݂��Ȃ�
            {
                Debug.Log("error : SessionNotExisted");

                //���͍��ڏ�����
                _sessionNameInputField.text = "";

                //�_�C�A���O�\��
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("not found session");
            }
        }

        //Back
        public void OnBackButton()
        {
            SceneManager.LoadScene((int)SceneName.LobbyScene);
        }

        //�S�Ẵ{�^�������b�N
        private void AllButtonLock()
        {
            _joinButton.interactable = false;
            _backButton.interactable = false;
        }

        //�S�Ẵ{�^�������b�N����
        private void AllButtonRelease()
        {
            _joinButton.interactable = true;
            _backButton.interactable = true;
        }
    }
}
