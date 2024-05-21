using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Prefabs
{
    public class Dialog : MonoBehaviour
    {
        [Header("Prefab Objects")]
        [SerializeField] private TMP_Text _messageTMP;

        private string _message;

        //èâä˙âª
        public void Init(string message)
        {
            _message = message;
            _messageTMP.text = message;
        }

        //Close
        public void OnCloseButton()
        {
            Destroy(this.gameObject);
        }
    }
}
