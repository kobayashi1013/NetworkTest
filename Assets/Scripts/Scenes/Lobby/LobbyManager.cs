using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;

namespace Scenes.Lobby.Manager
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private GameObject _Canvas;
        [SerializeField] private GameObject _sessionListContent;
        [SerializeField] private Prefab.SessionData _sessionDataPrefab;
        [SerializeField] private Prefab.Dialog _dialogPrefab;

        public static LobbyManager Instance;

        void Awake()
        {
            //�C���X�^���X��
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            //�Z�b�V�����\���X�V
            SessionListUpdate();
        }

        //Create Session
        public void OnButton0()
        {
            SceneManager.LoadScene((int)Constant.SceneName.SessionCreateScene);
        }

        //Back
        public void OnButton1()
        {
            //�{�^�����b�N
            var buttonList = FindObjectsOfType<Button>();
            foreach (var button in buttonList)
            {
                button.interactable = false;
            }

            //���r�[���痣�E
            if (Network.NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            Network.NetworkManager.Runner.Shutdown();

            //�X�^�[�g���j���[�V�[���֑J��
            SceneManager.LoadScene((int)Constant.SceneName.StartMenuScene);
        }

        //�Z�b�V�����r���[�̍X�V
        public void OnButton2()
        {
            SessionListUpdate();
        }

        //�Z�b�V�����r���[�̍X�V����
        private void SessionListUpdate()
        {
            //�Z�b�V�����폜
            foreach (Transform child in _sessionListContent.transform)
            {
                Destroy(child.gameObject);
            }

            //�Z�b�V�����`��
            if (Network.NetworkManager.Instance == null) Debug.LogError("error : Not Found Runner");
            for (int i = 0; i < Network.NetworkManager.Instance.updatedSessionList.Count; i++)
            {
                var obj = Instantiate(_sessionDataPrefab, _sessionListContent.transform);
                obj.Init(Network.NetworkManager.Instance.updatedSessionList[i].Name);
            }
        }

        //�Z�b�V���������݂��Ȃ�
        public void NotExistedSession()
        {
            //�_�C�A���O�\��
            var obj = Instantiate(_dialogPrefab, _Canvas.transform);
            obj.Init("not found session");

            //�Z�b�V�����r���[�X�V
            SessionListUpdate();
        }
    }
}
