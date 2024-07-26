using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;
using Fusion;
namespace Scenes.InGame.Block
{
    public class Block : NetworkBehaviour, IDamagable
    {
        [Header("�u���b�N�̃p�����[�^")]
        [SerializeField, Tooltip("�u���b�N�̑ϋv�x")]
        private int _hp = 1;

        public void Break()
        {
            Destroy(gameObject);
        }

        public void Damange(int damage)
        {
            if (damage < 0) return;//�_���[�W�����̏ꍇ�͏�����Ԃ�
            _hp = _hp - damage;
            if (_hp <= 0)
            {
                Manager.InGameManager.Instance.BlockDestroy();
                Break();
            }
        }
    }
}