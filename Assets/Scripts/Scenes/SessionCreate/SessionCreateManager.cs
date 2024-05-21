using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using Prefabs;
using Network;
using Constant;

namespace Scenes.LobbyCreate.Manager
{
    public class SessionCreateManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TMP_InputField _sessionNameInputField;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _backButton;
        [Header("Prefabs")]
        [SerializeField] private Dialog _dialogPrefab;
        [Header("Parameters")]
        [SerializeField] private int _maxPlayer = 2;

        //�Z�b�V�����쐬
        public async void OnPlayButton()
        {
            //�Z�b�V�������d������
            if (NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            if (NetworkManager.Instance.updatedSessionList.Exists(x => x.Name == _sessionNameInputField.text)) //���s
            {
                Debug.Log("exist name");

                //���͍��ڏ�����
                _sessionNameInputField.text = "";

                //�_�C�A���O�\��
                var obj = Instantiate(_dialogPrefab, _canvas.transform);
                obj.Init("existed name");
            }
            else //����
            {
                //�{�^�����b�N
                AllButtonLock();

                //�Z�b�V�����쐬
                var result = await NetworkManager.Runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Host, //�Q�[���ł̌���
                    Scene = SceneRef.FromIndex((int)SceneName.InGameScene), //���̃Q�[���V�[���̑I��
                    SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(), //Fusion�p��SceneManager�̎w��
                    SessionName = _sessionNameInputField.text, //�Z�b�V�������̌���
                    PlayerCount = _maxPlayer //�ő�l���̌���
                });

                if (result.Ok)
                {
                    Debug.Log("Host");
                }
                else
                {
                    Debug.LogError($"error : {result.ShutdownReason}");

                    //���b�N����
                    AllButtonRelease();
                }
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
            _playButton.interactable = false;
            _backButton.interactable = false;
        }

        //�S�Ẵ{�^��������
        private void AllButtonRelease()
        {
            _playButton.interactable = true;
            _backButton.interactable = true;
        }
    }
}
