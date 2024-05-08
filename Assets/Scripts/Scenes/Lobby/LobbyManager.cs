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
        [SerializeField] private GameObject _sessionList;
        [SerializeField] private GameObject _sessionPrefab;

        public static LobbyManager Instance;
        private List<SessionInfo> _prevSessionList = new List<SessionInfo>();

        void Awake()
        {
            if (Instance == null) Instance = this;
        }

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

        public void SessionListUpdate(List<SessionInfo> sessionList)
        {
            //�Z�b�V�����폜
            foreach (Transform child in _sessionList.transform)
            {
                Destroy(child.gameObject);
            }

            //�Z�b�V�����\��
            foreach (var session in sessionList)
            {
                var obj = Instantiate(_sessionPrefab);
                obj.transform.SetParent(_sessionList.transform, false);
                obj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = session.Name;
            }
        }
    }
}
