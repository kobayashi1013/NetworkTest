using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scenes.Lobby
{
    public class SessionData : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sessionNameTMP;

        private string _sessionName = "";

        public void Init(string name)
        {
            _sessionName = name;
            _sessionNameTMP.text = name;
        }
    }
}
