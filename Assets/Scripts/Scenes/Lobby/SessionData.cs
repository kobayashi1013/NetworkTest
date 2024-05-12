using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

namespace Scenes.Lobby
{
    public class SessionData : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sessionNameTMP;
        [SerializeField] private Button _joinButton;

        private string _sessionName = "";

        //������
        public void Init(string name)
        {
            _sessionName = name;
            _sessionNameTMP.text = name;
        }

        //�Z�b�V�����ɎQ��
        public async void PushJoinButton()
        {
            //�{�^�����b�N
            _joinButton.interactable = false;

            //�Z�b�V�����ɎQ��
            var result = await Network.NetworkManager.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                Scene = SceneRef.FromIndex((int)Constant.SceneName.InGameScene),
                SceneManager = this.gameObject.GetComponent<NetworkSceneManagerDefault>(),
                SessionName = _sessionName
            });

            if (result.Ok)
            {
                Debug.Log("Client");
            }
            else
            {
                Debug.LogError($"error : {result.ShutdownReason}");

                //���b�N����
                _joinButton.interactable = true;
            }
        }
    }
}
